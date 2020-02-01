using System;
using System.Threading.Tasks;
using NewsAPI.Constants;
using NewsAPI.Models;
using UnityEditor;
using UnityEngine;

namespace NewsAPI.OfflineCopies.Editor
{
	public class NewsApiOfflineCopyWindow : EditorWindow
	{
		string _fileName = "tmp";

		[MenuItem("Tools/News API Offline Copy Window")]
		static void CreateWindow()
		{
			GetWindow<NewsApiOfflineCopyWindow>().Show();
		}

		void OnGUI()
		{
			if (!Application.isPlaying)
			{
				if (GUILayout.Button("Enter Play Mode"))
				{
					EditorApplication.EnterPlaymode();
				}

				GUI.enabled = false;
			}

			_fileName = EditorGUILayout.TextField("File Name", _fileName);

			if (GUILayout.Button("Download Sources"))
			{
				DownloadSources().ConfigureAwait(false);
			}

			if (GUILayout.Button("Download US Top Headlines"))
			{
				DownloadUsTopHeadlines().ConfigureAwait(false);
			}

			if (GUILayout.Button("Download All Countries Top Headlines"))
			{
				DownloadAllCountriesTopHeadlines().ConfigureAwait(false);
			}

			GUI.enabled = true;

			if (GUILayout.Button("Open Folder"))
			{
				EditorUtility.RevealInFinder(NewsApiOfflineCopy.ResourceFilePath);
			}
		}

		async Task DownloadSources()
		{
			var client = NewsApiClient.FromResource();
			var response = await client.GetSources();

			if (CatchError(response)) return;

			var sources = response.Sources;
			var io = NewsApiOfflineCopy.FromResources(_fileName);
			io.WriteSources(sources);
			AssetDatabase.Refresh();

			Debug.Log("Finished downloading");
		}

		async Task DownloadUsTopHeadlines()
		{
			var client = NewsApiClient.FromResource();
			var response = await client.GetTopHeadlines(new NewsApiTopHeadlinesRequest
			{
				Language = NewsApiLanguage.EN,
				Country = NewsApiCountry.US,
				Page = 0,
				PageSize = 10,
			});

			if (CatchError(response)) return;

			var articles = response.Articles;
			var io = NewsApiOfflineCopy.FromResources(_fileName);
			io.WriteArticles("ustop", articles);
			AssetDatabase.Refresh();

			Debug.Log("Finished downloading");
		}

		async Task DownloadAllCountriesTopHeadlines()
		{
			var client = NewsApiClient.FromResource();
			var countries = (NewsApiCountry[]) Enum.GetValues(typeof(NewsApiCountry));

			foreach (var country in countries)
			{
				var response = await client.GetTopHeadlines(new NewsApiTopHeadlinesRequest
				{
					Language = NewsApiLanguage.EN,
					Country = country,
					Page = 0,
					PageSize = 10,
				});

				if (CatchError(response)) return;

				var articles = response.Articles;
				var io = NewsApiOfflineCopy.FromResources(_fileName);
				io.WriteArticles(country.ToString(), articles);

				Debug.Log($"Finished downloading {country}");
			}

			AssetDatabase.Refresh();

			Debug.Log("Finished downloading");
		}

		static bool CatchError(INewsApiResponse response)
		{
			if (response.Status != NewsApiStatus.Ok)
			{
				Debug.LogError($"Code: {response.Code}, message: {response.Message}");
				return true;
			}

			return false;
		}
	}
}