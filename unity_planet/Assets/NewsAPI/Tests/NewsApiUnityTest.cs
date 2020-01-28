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

		[Test]
		public void _TestApiKeyResource()
		{
			CreateClient();
		}

		NewsApiClient CreateClient()
		{
			var keyFile = Resources.Load<TextAsset>(ApiKeyFileName);
			Assert.IsNotNull(keyFile, "NewsAPI key file not found in Resources");

			var key = keyFile.text;
			Assert.IsNotEmpty(key, "NewsAPI key cannot be an empty string");

			return new NewsApiClient(key);
		}

		[UnityTest]
		public IEnumerator TestApiTopHeadlines()
		{
			yield return DoTestApiTopHeadlines().ToCoroutine();
		}

		async UniTask DoTestApiTopHeadlines()
		{
			var client = CreateClient();

			var request = new TopHeadlinesRequest
			{
				Language = Languages.EN,
				Country = Countries.US,
				Page = 0,
				PageSize = 10,
			};

			var result = await client.GetTopHeadlinesAsync(request);
			Assert.AreEqual(result.Status == Statuses.Ok, result.Error == null, "Error status and error object presence didn't match");
			Assert.Null(result.Error, "Error returned. Code: {0}, Message: {1}", result.Error?.Code, result.Error?.Message);
			Assert.GreaterOrEqual(result.TotalResults, result.Articles.Count, "TotalResults and the number of returned articles didn't match");

			Debug.Log(string.Join("\n", result.Articles.Select(a => $"{a.Source.Name}: {a.Title}")));
		}
	}
}