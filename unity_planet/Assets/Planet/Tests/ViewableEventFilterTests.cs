using NUnit.Framework;
using Planet.Data;

namespace Planet.Tests
{
	public class ViewableEventFilterTests
	{
		[Test]
		public void FilterNonEnglishEvent()
		{
			var ev0 = new MockEvent(language: "en");
			var ev1 = new MockEvent(language: "fr");
			var ev2 = new MockEvent(language: null);
			var filter = new ViewableEventFilter();

			Assert.IsTrue(filter.Filter(ev0));
			Assert.IsFalse(filter.Filter(ev1));
			Assert.IsFalse(filter.Filter(ev2));
		}

		[Test]
		public void FilterListMatch()
		{
			var ev0 = new MockEvent(language: "en", title: "ev0");
			var ev1 = new MockEvent(language: "en", title: "ev1");
			var ev2 = new MockEvent(language: null, title: "ev2");
			var filter = new ViewableEventFilter();

			CollectionAssert.AreEqual(new[] {ev0, ev1}, filter.Filter(new[] {ev0, ev1, ev2}));
		}

		[Test]
		public void FilterListNoMatch()
		{
			var ev0 = new MockEvent(language: "jp", title: "ev0");
			var ev1 = new MockEvent(language: "fr", title: "ev1");
			var ev2 = new MockEvent(language: null, title: "ev2");
			var filter = new ViewableEventFilter();

			CollectionAssert.AreEqual(new[] {ev2}, filter.Filter(new[] {ev0, ev1, ev2}));
		}
	}
}