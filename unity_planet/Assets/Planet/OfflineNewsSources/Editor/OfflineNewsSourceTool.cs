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
		const string SourceFilePath = "Assets/Planet/OfflineNewsSources/Resources/OfflineNewsSource.json";

		static NewsApiClient MakeClient()
		{
			var newsKeyFile = Resources.Load<TextAsset>(NewsApiKeyFileName);
			var newsClient = new NewsApiClient(newsKeyFile.text);
			return newsClient;
		}

		[MenuItem("Tools/Offline News Source Tool/Ping Top Headlines")]
		static async void FetchTopHeadlines()
		{
			var newsClient = MakeClient();

			var request = new NewsApiTopHeadlinesRequest
			{
				Language = NewsApiLanguage.EN,
				Country = NewsApiCountry.US,
				Page = 0,
				PageSize = 3,
			};

			newsClient.LogJsonResponses = true;
			await newsClient.GetTopHeadlines(request);
		}

		[MenuItem("Tools/Offline News Source Tool/Fetch & Save Top Headlines")]
		static async void FetchForOfflineNews()
		{
			if (!Application.isPlaying)
			{
				Debug.LogError("Can't do this during edit mode");
				return;
			}

			var newsClient = MakeClient();
			var countries = (NewsApiCountry[]) Enum.GetValues(typeof(NewsApiCountry));
			var sourceBuilder = new OfflineNewsSourceBuilder();

			// check writing
			WriteToFile(SourceFilePath, "");

			foreach (var country in countries)
			{
				Debug.Log($"Fetching country '{country}'...");

				var request = new NewsApiTopHeadlinesRequest
				{
					Language = NewsApiLanguage.EN,
					Country = country,
					Page = 0,
					PageSize = 100,
				};

				var result = await newsClient.GetTopHeadlines(request);

				if (result.Status != NewsApiStatus.Ok)
				{
					Debug.LogError($"Failed fetching country: {country}. Code: {result.Code}, message: {result.Message}");
					return;
				}

				sourceBuilder.Add(country, "en", result.Articles);
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