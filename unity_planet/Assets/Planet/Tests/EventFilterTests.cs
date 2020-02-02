using NUnit.Framework;
using Planet.Data;

namespace Planet.Tests
{
	public class EventFilterTests
	{
		FallbackFilter<string> _filters;

		[SetUp]
		public void SetUp()
		{
			_filters = new FallbackFilter<string>();
			_filters.AddFilter(new FallbackFilterEntry<string>(true, e => e.Contains("us")));
			_filters.AddFilter(new FallbackFilterEntry<string>(false, e => e.Contains("url")));
		}

		[Test]
		public void NonePassed()
		{
			var result = _filters.Filter(new[] {"foo", "bar"});
			CollectionAssert.IsEmpty(result);
		}

		[Test]
		public void AllPassed()
		{
			var result = _filters.Filter(new[] {"foo_us_url", "foo_us_url"});
			CollectionAssert.AreEqual(new[] {"foo_us_url", "foo_us_url"}, result);
		}

		[Test]
		public void FallbackPassed()
		{
			var result = _filters.Filter(new[] {"foo_us", "foo_url", "foo"});
			CollectionAssert.AreEqual(new[] {"foo_url"}, result);
		}

		[Test]
		public void SomePassed()
		{
			var result = _filters.Filter(new[] {"foo_us", "foo_us_url", "foo"});
			CollectionAssert.AreEqual(new[] {"foo_us_url"}, result);
		}
	}
}