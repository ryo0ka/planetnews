using System;
using System.Collections.Generic;
using System.Linq;

namespace Planet.Utils
{
	public static class LinqUtils
	{
		public static bool TryFirst<T>(this IEnumerable<T> self, Func<T, bool> f, out T first)
		{
			first = self.FirstOrDefault(f);
			return first != null;
		}
	}
}