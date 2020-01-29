using System;
using System.IO;
using JetBrains.Annotations;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Planet.OfflineNewsSources.Editor
{
	[UsedImplicitly]
	static class OfflineNewsSourceTool
	{
		const string NewsApiKeyFileName = "GitIgnore/NewsApiKey";
		const string SourceFilePath = "Assets/Planet/OfflineNewsSources/Resources/source.json";

		[MenuItem("Tools/Fetch For Offline News")]
		static async void FetchForOfflineNews()
		{
			if (!Application.isPlaying)
			{
				Debug.LogError("Can't do this during edit mode");
				return;
			}

			var newsKeyFile = Resources.Load<TextAsset>(NewsApiKeyFileName);
			var newsClient = new NewsApiClient(newsKeyFile.text);
			var countries = (Countries[]) Enum.GetValues(typeof(Countries));
			var sourceBuilder = new OfflineNewsSourceBuilder();
			
			// check writing
			WriteToFile(SourceFilePath, "");

			foreach (var country in countries)
			{
				Debug.Log($"Fetching country '{country}'...");

				var request = new TopHeadlinesRequest
				{
					Language = Languages.EN,
					Country = country,
					Page = 0,
					PageSize = 100,
				};

				var result = await newsClient.GetTopHeadlinesAsync(request);

				if (result.Status != Statuses.Ok)
				{
					Debug.LogError($"Failed fetching country: {country}, " +
					               $"for code: {result.Error.Code}, message: {result.Error.Message}");
					return;
				}

				sourceBuilder.Add(country, result.Articles);
			}

			var source = sourceBuilder.Build();

			WriteToFile(SourceFilePath, JsonConvert.SerializeObject(source, Formatting.Indented));
			AssetDatabase.Refresh();

			Debug.Log("Finished fething & saving all countries");
			Debug.Log(SourceFilePath);
		}

		static void WriteToFile(string filePath, string text)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			File.WriteAllText(SourceFilePath, text);
		}
	}
}