using System;
using System.Collections.Generic;
using System.Linq;

namespace Planet.Utils
{
	public static class LinqUtils
	{
		public static bool TryLast<T>(this IEnumerable<T> self, Func<T, bool> f, out T last)
		{
			last = self.LastOrDefault(f);
			return last != null;
		}

		public static bool TryLast<T>(this IEnumerable<T> self, out T last)
		{
			last = self.LastOrDefault();
			return last != null;
		}
	}
}