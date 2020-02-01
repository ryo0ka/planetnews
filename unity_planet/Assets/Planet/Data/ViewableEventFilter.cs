using System.Collections.Generic;
using System.Linq;
using Planet.Models;
using Planet.Utils;

namespace Planet.Data
{
	public sealed class ViewableEventFilter
	{
		public bool Filter(IEvent ev)
		{
			return ev.Language?.ToLower() == "en";
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