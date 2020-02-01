using NewsAPI.Models;
using NUnit.Framework;
using Planet.Data;

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
			var event1 = factory.MakeEvent(article1, "");
			var event2 = factory.MakeEvent(article2, "");
			Assert.AreEqual(0, event1.Id);
			Assert.AreEqual(1, event2.Id);
		}

		[Test]
		public void AttachLanguage()
		{
			var factory = new NewsApiEventFactory(new[]
			{
				new NewsApiSource("source A", "", "", "", "", "lang A", ""),
				new NewsApiSource("source B", "", "", "", "", "lang B", ""),
				new NewsApiSource("source C", "", "", "", "", "lang C", ""),
			});

			var article1 = new NewsApiArticle(new NewsApiSourceShort("source B", ""), "", "", "", "", "", null);
			var event1 = factory.MakeEvent(article1, "");
			Assert.AreEqual("lang B", event1.Language);
		}

		[Test]
		public void AttachNullLanguage()
		{
			var factory = new NewsApiEventFactory(new[]
			{
				new NewsApiSource("source A", "", "", "", "", null, ""),
			});

			var article1 = new NewsApiArticle(new NewsApiSourceShort("source A", ""), "", "", "", "", "", null);
			var event1 = factory.MakeEvent(article1, "");
			Assert.AreEqual(null, event1.Language);
		}

		[Test]
		public void AttachNullSource()
		{
			var factory = new NewsApiEventFactory(new[]
			{
				new NewsApiSource("source A", "", "", "", "", "lang A", ""),
			});

			var article1 = new NewsApiArticle(new NewsApiSourceShort("source B", ""), "", "", "", "", "", null);
			var event1 = factory.MakeEvent(article1, "");
			Assert.AreEqual(null, event1.Language);
		}
	}
}