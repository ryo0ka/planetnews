using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Planet.Utils
{
	public struct EmptyReadonlyList<T> : IReadOnlyList<T>
	{
		public IEnumerator<T> GetEnumerator() => Enumerable.Empty<T>().GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public int Count => 0;
		public T this[int index] => throw new IndexOutOfRangeException();
	}
}