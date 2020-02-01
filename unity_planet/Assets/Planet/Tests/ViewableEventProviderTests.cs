using NUnit.Framework;
using Planet.Data;

namespace Planet.Tests
{
	public class ViewableEventProviderTests
	{
		[Test]
		public void ViewEnglishEvent()
		{
			var viewableEvents = new ViewableEventProvider();
			var country = "country";
			var ev = new MockEvent(language: "en");
			viewableEvents.AddEvent(country, ev);
			Assert.IsTrue(viewableEvents.TryGetTopViewableEvent(country, out _));
		}

		[Test]
		public void FilterNonEnglishEvent()
		{
			var viewableEvents = new ViewableEventProvider();
			var country = "country";
			var ev1 = new MockEvent(language: "fr");
			var ev2 = new MockEvent(language: null);
			viewableEvents.AddEvent(country, ev1);
			viewableEvents.AddEvent(country, ev2);
			Assert.IsFalse(viewableEvents.TryGetTopViewableEvent(country, out _));
		}
	}
}