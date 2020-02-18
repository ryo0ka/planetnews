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

		public static void CopyTo<K, V>(this IReadOnlyDictionary<K, V> self, IDictionary<K, V> other)
		{
			foreach (var pair in self)
			{
				other[pair.Key] = pair.Value;
			}
		}

		public static void Fill<T>(this IList<T> self, int offset, int length, T element)
		{
			for (var i = offset; i < length; i++)
			{
				if (self.Count == i)
				{
					self.Add(element);
				}
				else if (self.Count > i)
				{
					self[i] = element;
				}
				else if (self.Count < i)
				{
					throw new IndexOutOfRangeException($"{self.Count} < {i}");
				}
			}
		}

		public static V GetValueOrElse<K, V>(this IReadOnlyDictionary<K, V> self, K key, V def = default)
		{
			if (self.TryGetValue(key, out var value))
			{
				return value;
			}

			return def;
		}

		public static IReadOnlyList<T> EmptyList<T>()
		{
			return new EmptyReadonlyList<T>();
		}

		public static void TrimLength<T>(this List<T> self, int length)
		{
			if (self.Count <= length) return;
			self.RemoveRange(length, self.Count - length);
		}

		public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> other)
		{
			foreach (var e in other)
			{
				self.Add(e);
			}
		}

		public static void RemoveRange<T>(this ICollection<T> self, IEnumerable<T> other)
		{
			foreach (var e in other)
			{
				self.Remove(e);
			}
		}

		public static (K, V) Decompose<K, V>(this KeyValuePair<K, V> self)
		{
			return (self.Key, self.Value);
		}

		public static bool TryIndexOf<T>(this IList<T> self, T element, out int index)
		{
			index = self.IndexOf(element);
			return index >= 0;
		}
	}
}