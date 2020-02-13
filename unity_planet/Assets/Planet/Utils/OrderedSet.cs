using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Planet.Utils
{
	public sealed class OrderedSet<T> : ISet<T>
	{
		readonly HashSet<T> _set;
		readonly List<T> _list;

		public OrderedSet()
		{
			_set = new HashSet<T>();
			_list = new List<T>();
		}

		public T this[int index] => _list[index];

		public int Count => _list.Count;
		public bool IsReadOnly => false;
		public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);
		public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);
		public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);
		public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);
		public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);
		public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);
		public bool Contains(T item) => _set.Contains(item);
		public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
		void ICollection<T>.Add(T item) => Add(item);

		// Modifies the current object to contain only elements that are
		// present either in that object or in the specified collection,
		// but not both.
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			for (var i = _list.Count - 1; i >= 0; i--)
			{
				var e = _list[i];
				if (other.Contains(e))
				{
					_list.RemoveAt(i);
				}
			}

			foreach (var e in other)
			{
				if (_set.Contains(e))
				{
					_list.Remove(e);
				}
			}

			_set.SymmetricExceptWith(other);
		}

		public void UnionWith(IEnumerable<T> other)
		{
			foreach (var e in other)
			{
				if (!_set.Contains(e))
				{
					_list.Add(e);
				}
			}

			_set.UnionWith(other);
		}

		public void ExceptWith(IEnumerable<T> other)
		{
			foreach (var e in other)
			{
				if (_set.Contains(e))
				{
					_list.Remove(e);
				}
			}

			_set.ExceptWith(other);
		}

		// Modifies the current object to contain only elements that are
		// present in that object and in the specified collection.
		public void IntersectWith(IEnumerable<T> other)
		{
			for (var i = _list.Count - 1; i >= 0; i--)
			{
				var e = _list[i];
				if (!other.Contains(e))
				{
					_list.RemoveAt(i);
				}
			}

			foreach (var e in other)
			{
				if (!_set.Contains(e))
				{
					_list.Remove(e);
				}
			}

			_set.IntersectWith(other);
		}

		public bool Add(T item)
		{
			if (_set.Add(item))
			{
				_list.Add(item);
				return true;
			}

			return false;
		}

		public void Clear()
		{
			_set.Clear();
			_list.Clear();
		}

		public bool Remove(T item)
		{
			if (_set.Remove(item))
			{
				_list.Remove(item);
				return true;
			}

			return false;
		}

		public void RemoveAt(int index)
		{
			var value = _list[index];
			_list.RemoveAt(index);
			_set.Remove(value);
		}
	}
}