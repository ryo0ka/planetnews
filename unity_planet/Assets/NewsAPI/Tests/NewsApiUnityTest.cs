using System.Collections;
using System.Linq;
using NewsAPI.Constants;
using NewsAPI.Models;
using NUnit.Framework;
using UniRx.Async;
using UnityEngine;
using UnityEngine.TestTools;

namespace NewsAPI.Tests
{
	public class NewsApiUnityTest
	{
		const string ApiKeyFileName = "GitIgnore/NewsApiKey";

		NewsApiClient CreateClient()
		{
			var keyFile = Resources.Load<TextAsset>(ApiKeyFileName);
			Assert.IsNotNull(keyFile, "NewsAPI key file not found in Resources");

			var key = keyFile.text;
			Assert.IsNotEmpty(key, "NewsAPI key cannot be an empty string");

			return new NewsApiClient(key);
		}

		[UnityTest]
		public IEnumerator GetTopHeadlines()
		{
			yield return DoGetTopHeadlines().ToCoroutine();
		}

		async UniTask DoGetTopHeadlines()
		{
			var client = CreateClient();
			var result = await client.GetTopHeadlines(new NewsApiTopHeadlinesRequest
			{
				Language = NewsApiLanguage.EN,
				Country = NewsApiCountry.US,
				Page = 0,
				PageSize = 10,
			});

			Assert.AreNotEqual(NewsApiStatus.Unknown, result.Status, "status not read properly");
			Assert.AreNotEqual(NewsApiStatus.Error, result.Status, $"Error code: {result.Code}, Message: {result.Message}");
			Assert.IsNotNull(result.Articles, "null article list");
			Assert.GreaterOrEqual(result.TotalResults, result.Articles.Count(), "count didnt match");

			Debug.Log(string.Join("\n", result.Articles.Select(a => $"{a.Source.Name}: {a.Title}")));
		}

		[UnityTest]
		public IEnumerator GetSources()
		{
			yield return DoGetSources().ToCoroutine();
		}

		async UniTask DoGetSources()
		{
			var client = CreateClient();
			client.LogJsonResponses = true;
			var result = await client.GetSources();

			Assert.AreNotEqual(NewsApiStatus.Unknown, result.Status, "status not read properly");
			Assert.AreNotEqual(NewsApiStatus.Error, result.Status, $"Error code: {result.Code}, Message: {result.Message}");
			Assert.IsNotNull(result.Sources, "null source list");
			Assert.Greater(result.Sources.Count(), 0, "no sources");
		}
	}
}