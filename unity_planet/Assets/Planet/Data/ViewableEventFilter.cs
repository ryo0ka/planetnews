using System;
using System.Collections.Generic;
using System.Linq;
using Planet.Models;
using Planet.Utils;

namespace Planet.Data
{
	public sealed class ViewableEventFilter
	{
		readonly List<Func<IEvent, bool>> _filters;

		public ViewableEventFilter()
		{
			_filters = new List<Func<IEvent, bool>>
			{
				e => e.Language?.ToLower() == "en",
				e => !string.IsNullOrEmpty(e.ThumbnailUrl),
			};
		}

		bool Filter(IEvent ev)
		{
			return _filters.All(f => f(ev));
		}

		public IEnumerable<IEvent> Filter(IEnumerable<IEvent> events)
		{
			var filtered = events.Where(e => Filter(e));
			if (filtered.Any()) return filtered;

			var whatevers = new List<IEvent>();
			if (events.TryLast(out var whatever))
			{
				whatevers.Add(whatever);
			}

			return whatevers;
		}
	}
}