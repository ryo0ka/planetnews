using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Planet.Views;

namespace Planet.Tests
{
	public class EventViewMapperTests
	{
		[Test]
		public void MapOnceFit()
		{
			var viewCount = 3;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries = new[] {"JP", "US", "UK"};

			mapper.UpdateMapping(focusedCountries);
			var mappedCountries = mapper.MappedCountries;

			CollectionAssert.IsSubsetOf(focusedCountries, mappedCountries);
			CollectionAssert.IsSupersetOf(focusedCountries, mappedCountries);
		}

		[Test]
		public void MapOnceLong()
		{
			var viewCount = 4;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries = new[] {"JP", "US", "UK"};

			mapper.UpdateMapping(focusedCountries);
			var mappedCountries = mapper.MappedCountries;
			var mappedCountriesNoNull = mappedCountries.Where(c => c != null);

			CollectionAssert.Contains(mappedCountries, null);
			CollectionAssert.IsSubsetOf(focusedCountries, mappedCountriesNoNull);
			CollectionAssert.IsSupersetOf(focusedCountries, mappedCountriesNoNull);
		}

		[Test]
		public void MapOnceShort()
		{
			var viewCount = 2;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries = new[] {"JP", "US", "UK"};

			mapper.UpdateMapping(focusedCountries);
			var mappedCountries = mapper.MappedCountries;

			Assert.AreEqual(viewCount, mappedCountries.Count);
			CollectionAssert.AllItemsAreUnique(mappedCountries);
			CollectionAssert.IsSubsetOf(mappedCountries, focusedCountries);
		}

		[Test]
		public void MapTwiceReferencialEqual()
		{
			var viewCount = 3;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries = new[] {"JP", "US", "UK"};

			mapper.UpdateMapping(focusedCountries);
			var mappedCountries1 = mapper.MappedCountries;

			mapper.UpdateMapping(focusedCountries);
			var mappedCountries2 = mapper.MappedCountries;

			Assert.AreNotSame(mappedCountries1, mappedCountries2);
		}

		[Test]
		public void MapTwiceNoDiffFit()
		{
			var viewCount = 3;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries = new[] {"JP", "US", "UK"};

			mapper.UpdateMapping(focusedCountries);
			var mappedCountries1 = mapper.MappedCountries;

			mapper.UpdateMapping(focusedCountries);
			var mappedCountries2 = mapper.MappedCountries;

			CollectionAssert.DoesNotContain(mappedCountries2, null);
			CollectionAssert.AllItemsAreUnique(mappedCountries2);
			CollectionAssert.AreEqual(mappedCountries1, mappedCountries2);
		}

		[Test]
		public void MapTwiceNoDiffLong()
		{
			var viewCount = 4;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries = new[] {"JP", "US", "UK"};

			mapper.UpdateMapping(focusedCountries);
			var mappedCountries1 = mapper.MappedCountries;

			mapper.UpdateMapping(focusedCountries);
			var mappedCountries2 = mapper.MappedCountries;

			CollectionAssert.Contains(mappedCountries2, null);
			CollectionAssert.AllItemsAreUnique(mappedCountries2);
			CollectionAssert.AreEqual(mappedCountries1, mappedCountries2);
		}

		[Test]
		public void MapTwiceNoDiffShort()
		{
			var viewCount = 2;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries = new[] {"JP", "US", "UK"};

			mapper.UpdateMapping(focusedCountries);
			mapper.UpdateMapping(focusedCountries);
			var mappedCountries2 = mapper.MappedCountries;

			CollectionAssert.DoesNotContain(mappedCountries2, null);
			CollectionAssert.AllItemsAreUnique(mappedCountries2);
			Assert.AreEqual(viewCount, mappedCountries2.Count);

			// Undefined whether the previous mapping
			// should be presereved or not in this case.
		}

		[Test]
		public void MapTwiceSomeDiffFit()
		{
			var viewCount = 4;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries1 = new[] {"AE", "SG", "IN", "PH"};
			var focusedCountries2 = new[] {"IN", "AE", "MY", "SA"};

			mapper.UpdateMapping(focusedCountries1);
			mapper.UpdateMapping(focusedCountries2);
			var mappedCountries2 = mapper.MappedCountries;

			CollectionAssert.AreEqual(new[] {"AE", "MY", "IN", "SA"}, mappedCountries2);
		}

		[Test]
		public void MapTwiceSomeDiffLong()
		{
			var viewCount = 4;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries1 = new[] {"JP", "US", "UK"};
			var focusedCountries2 = new[] {"IT", "US", "UK"};

			mapper.UpdateMapping(focusedCountries1);
			var mappedCountries1 = mapper.MappedCountries;
			mapper.UpdateMapping(focusedCountries2);
			var mappedCountries2 = mapper.MappedCountries;

			CollectionAssert.DoesNotContain(mappedCountries2, null);
			CollectionAssert.AllItemsAreUnique(mappedCountries2);
			CollectionAssert.AreEqual(Replace(mappedCountries1, null, "IT"), mappedCountries2);
		}

		[Test]
		public void MapTwiceSomeDiffShort()
		{
			var viewCount = 2;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries1 = new[] {"JP", "US", "UK"};
			var focusedCountries2 = new[] {"IT", "US", "UK"};

			mapper.UpdateMapping(focusedCountries1);
			mapper.UpdateMapping(focusedCountries2);
			var mappedCountries2 = mapper.MappedCountries;

			CollectionAssert.DoesNotContain(mappedCountries2, null);
			CollectionAssert.AllItemsAreUnique(mappedCountries2);
			Assert.AreEqual(viewCount, mappedCountries2.Count);

			// Undefined whether the previous mapping
			// should be presereved or not in this case.
		}

		[Test]
		public void MapTwiceAllDiffFit()
		{
			var viewCount = 3;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries1 = new[] {"JP", "US", "UK"};
			var focusedCountries2 = new[] {"FR", "GE", "IT"};

			mapper.UpdateMapping(focusedCountries1);
			mapper.UpdateMapping(focusedCountries2);
			var mappedCountries2 = mapper.MappedCountries;

			CollectionAssert.DoesNotContain(mappedCountries2, null);
			CollectionAssert.IsSubsetOf(focusedCountries2, mappedCountries2);
			CollectionAssert.IsSupersetOf(focusedCountries2, mappedCountries2);
		}

		[Test]
		public void MapTwiceAllDiffLong()
		{
			var viewCount = 4;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries1 = new[] {"JP", "US", "UK"};
			var focusedCountries2 = new[] {"FR", "GE", "IT"};

			mapper.UpdateMapping(focusedCountries1);
			mapper.UpdateMapping(focusedCountries2);
			var mappedCountries2 = mapper.MappedCountries;

			CollectionAssert.DoesNotContain(mappedCountries2, null);
			CollectionAssert.IsSubsetOf(focusedCountries2, mappedCountries2);
			CollectionAssert.IsSubsetOf(mappedCountries2, focusedCountries1.Concat(focusedCountries2));
			Assert.AreEqual(viewCount, mappedCountries2.Count);
		}

		[Test]
		public void MapTwiceAllDiffShort()
		{
			var viewCount = 2;
			var mapper = new EventViewMapper(viewCount);
			var focusedCountries1 = new[] {"JP", "US", "UK"};
			var focusedCountries2 = new[] {"FR", "GE", "IT"};

			mapper.UpdateMapping(focusedCountries1);
			mapper.UpdateMapping(focusedCountries2);
			var mappedCountries2 = mapper.MappedCountries;

			CollectionAssert.DoesNotContain(mappedCountries2, null);
			CollectionAssert.IsSubsetOf(mappedCountries2, focusedCountries2);
			Assert.AreEqual(viewCount, mappedCountries2.Count);
		}

		static IEnumerable<T> Replace<T>(IEnumerable<T> self, T old, T @new)
		{
			var cloned = new List<T>(self);
			for (var i = 0; i < cloned.Count; i++)
			{
				if (Equals(cloned[i], old))
				{
					cloned[i] = @new;
				}
			}

			return cloned;
		}
	}
}