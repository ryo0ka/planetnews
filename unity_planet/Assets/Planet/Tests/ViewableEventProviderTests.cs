using NUnit.Framework;
using Planet.Data;

namespace Planet.Tests
{
	public class ViewableEventProviderTests
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
	}
}