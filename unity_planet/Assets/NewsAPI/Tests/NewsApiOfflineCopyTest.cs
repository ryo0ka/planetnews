using System;
using System.Collections.Generic;
using System.IO;
using NewsAPI.Models;
using NewsAPI.OfflineCopies;
using NUnit.Framework;
using UnityEngine;

namespace NewsAPI.Tests
{
	public class NewsApiOfflineCopyTest
	{
		const string SubdirPath = "foo";
		string _dirPath;

		[SetUp]
		public void SetUp()
		{
			_dirPath = Path.Combine(Application.temporaryCachePath, SubdirPath);
		}

		[Test]
		public void WriteAndReadSources()
		{
			var sources1 = new List<NewsApiSource>
			{
				new NewsApiSource("1", "name 1", "desc 1", "url 1", "category 1", "language 1", "country 1"),
				new NewsApiSource("2", "name 2", "desc 2", "url 2", "category 2", "language 2", "country 2"),
				new NewsApiSource("3", "name 3", "desc 3", "url 3", "category 3", "language 3", "country 3"),
			};

			var io = new NewsApiOfflineCopy(_dirPath, "tmp");
			io.WriteSources(sources1);
			var sources2 = io.ReadSources();

			CollectionAssert.AreEqual(sources1, sources2);
		}

		[Test]
		public void WriteAndReadArticles()
		{
			var articles1 = new List<NewsApiArticle>
			{
				new NewsApiArticle(new NewsApiSourceShort("id 1", "name 1"), "author 1", "title 1", "desc 1", "url 1", "image 1", DateTime.Now),
				new NewsApiArticle(new NewsApiSourceShort("id 2", "name 2"), "author 2", "title 2", "desc 2", "url 2", "image 2", DateTime.Now),
				new NewsApiArticle(new NewsApiSourceShort("id 3", "name 3"), "author 3", "title 3", "desc 3", "url 3", "image 3", DateTime.Now),
			};

			var io = new NewsApiOfflineCopy(_dirPath, "tmp");
			var postfix = "bar";
			io.WriteArticles(postfix, articles1);
			var articles2 = io.ReadArticles(postfix);

			CollectionAssert.AreEqual(articles1, articles2);
		}
	}
}