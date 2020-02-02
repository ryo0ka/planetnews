using System;

namespace Planet.Data
{
	public sealed class FallbackFilterEntry<T>
	{
		public bool CanFallback { get; }
		public Func<T, bool> Filter { get; }

		public FallbackFilterEntry(bool canFallback, Func<T, bool> filter)
		{
			CanFallback = canFallback;
			Filter = filter;
		}
	}
}