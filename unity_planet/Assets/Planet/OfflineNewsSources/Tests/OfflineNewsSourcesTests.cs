using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Planet.OfflineNewsSources.Tests
{
	public sealed class OfflineNewsSourcesTests
	{
		[Test]
		public void DiskSerialization()
		{
			var usArticles = new List<OfflineNewsArticle>();
			var brArticles = new List<OfflineNewsArticle>();

			usArticles.Add(new OfflineNewsArticle(
				title: "us title 1",
				description: "us description 1",
				url: "us url 1",
				imageUrl: "us image url 1",
				publishDate: DateTime.Now,
				language: "en"));

			usArticles.Add(new OfflineNewsArticle(
				title: "us title 2",
				description: "us description 2",
				url: "us url 2",
				imageUrl: "us image url 2",
				publishDate: DateTime.Now,
				language: "en"));

			brArticles.Add(new OfflineNewsArticle(
				title: "br title 1",
				description: "br description 1",
				url: "br url 1",
				imageUrl: "br image url 1",
				publishDate: DateTime.Now,
				language: "en"));

			brArticles.Add(new OfflineNewsArticle(
				title: "br title 2",
				description: "br description 2",
				url: "br url 2",
				imageUrl: "br image url 2",
				publishDate: DateTime.Now,
				language: "en"));

			var sourceWriter = new OfflineNewsSourceBuilder();
			sourceWriter.Add("US", usArticles);
			sourceWriter.Add("BR", brArticles);
			var source = sourceWriter.Build();

			var sourceText = JsonConvert.SerializeObject(source);
			var sourceDeserialized = JsonConvert.DeserializeObject<OfflineNewsSource>(sourceText);

			Assert.IsTrue(sourceDeserialized.HasCountry("US"));
			CollectionAssert.AreEqual(usArticles, sourceDeserialized["US"]);

			Assert.IsTrue(sourceDeserialized.HasCountry("BR"));
			CollectionAssert.AreEqual(brArticles, sourceDeserialized["BR"]);
		}
	}
}