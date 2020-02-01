using System.Text.RegularExpressions;
using NewsAPI.Models;
using NUnit.Framework;
using Planet.Data;
using UnityEngine;
using UnityEngine.TestTools;

namespace Planet.Tests
{
	public class NewsApiEventTests
	{
		[Test]
		public void AttachId()
		{
			var factory = new NewsApiEventFactory(new[]
			{
				new NewsApiSource("", "source A", "", "", "", "", ""),
			});

			var article1 = new NewsApiArticle(new NewsApiSourceShort("", "source A"), "", "", "", "", "", null);
			var article2 = new NewsApiArticle(new NewsApiSourceShort("", "source A"), "", "", "", "", "", null);
			factory.TryMakeEvent(article1, "", out var event1);
			factory.TryMakeEvent(article2, "", out var event2);
			Assert.AreEqual(0, event1.Id);
			Assert.AreEqual(1, event2.Id);
		}

		[Test]
		public void AttachLanguage()
		{
			var factory = new NewsApiEventFactory(new[]
			{
				new NewsApiSource("", "source A", "", "", "", "lang A", ""),
				new NewsApiSource("", "source B", "", "", "", "lang B", ""),
				new NewsApiSource("", "source C", "", "", "", "lang C", ""),
			});

			var article1 = new NewsApiArticle(new NewsApiSourceShort("", "source B"), "", "", "", "", "", null);
			factory.TryMakeEvent(article1, "", out var event1);
			Assert.AreEqual("lang B", event1.Language);
		}

		[Test]
		public void FailArticlesWithoutSource()
		{
			var factory = new NewsApiEventFactory(new[]
			{
				new NewsApiSource("source A", "", "", "", "", "", ""),
			});

			var article1 = new NewsApiArticle(new NewsApiSourceShort("source B", ""), "", "", "", "", "", null);
			factory.TryMakeEvent(article1, "", out var event1);
			LogAssert.Expect(LogType.Log, new Regex("^Source not found"));
			Assert.IsNull(event1);
		}
	}
}