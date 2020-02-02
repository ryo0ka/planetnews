using System.Collections.Generic;

namespace Planet.Data
{
	public class FallbackFilter<T>
	{
		readonly List<FallbackFilterEntry<T>> _filters;

		public FallbackFilter()
		{
			_filters = new List<FallbackFilterEntry<T>>();
		}

		public void AddFilter(FallbackFilterEntry<T> filter)
		{
			_filters.Add(filter);
		}

		public IEnumerable<T> Filter(IEnumerable<T> elements)
		{
			var lastFallback = default(T);
			var fallbackFound = false;
			var nonePassed = true;
			foreach (var e in elements)
			{
				var passedAll = true;
				var canFallback = true;
				foreach (var filter in _filters)
				{
					if (!filter.Filter(e)) // failed
					{
						passedAll = false;
						if (!filter.CanFallback)
						{
							canFallback = false;
						}
					}
				}

				if (passedAll)
				{
					nonePassed = false;
					yield return e;
				}
				else if (canFallback)
				{
					lastFallback = e;
					fallbackFound = true;
				}
			}

			if (nonePassed && fallbackFound)
			{
				yield return lastFallback;
			}
		}
	}
}