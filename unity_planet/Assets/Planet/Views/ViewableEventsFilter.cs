using System.Collections.Generic;
using System.Linq;
using Planet.Models;

namespace Planet.Views
{
	public sealed class ViewableEventsFilter
	{
		readonly Dictionary<string, List<IEventHeadline>> _events;
		readonly HashSet<string> _viewableCountries;

		public ViewableEventsFilter()
		{
			_events = new Dictionary<string, List<IEventHeadline>>();
			_viewableCountries = new HashSet<string>();
		}

		public IEnumerable<string> ViewableCountries => _viewableCountries;

		public IEventHeadline GetTopEvent(string country)
		{
			return _events[country].First();
		}

		public void AddEvent(string country, IEventHeadline ev)
		{
			if (!_events.TryGetValue(country, out var events))
			{
				events = _events[country] = new List<IEventHeadline>();
			}

			events.Add(ev);

			var viewable = events.Any(e => IsViewable(e));
			if (viewable)
			{
				_viewableCountries.Add(country);
			}
			else
			{
				_viewableCountries.Remove(country);
			}
		}

		bool IsViewable(IEventHeadline ev)
		{
			return ev.Language.ToLower() == "en";
		}
	}
}