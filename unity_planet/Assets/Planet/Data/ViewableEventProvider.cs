using System.Collections.Generic;
using System.Linq;
using Planet.Models;

namespace Planet.Data
{
	public sealed class ViewableEventProvider
	{
		readonly Dictionary<string, List<IEvent>> _viewableEvents;

		public ViewableEventProvider()
		{
			_viewableEvents = new Dictionary<string, List<IEvent>>();
		}

		public IEnumerable<string> ViewableCountries => _viewableEvents.Keys;

		public bool TryGetTopViewableEvent(string country, out IEvent ev)
		{
			_viewableEvents.TryGetValue(country, out var events);
			ev = events?.FirstOrDefault();
			return ev != null;
		}

		public void AddEvent(string country, IEvent ev)
		{
			if (!_viewableEvents.ContainsKey(country))
			{
				_viewableEvents[country] = new List<IEvent>();
			}

			if (IsViewable(ev))
			{
				_viewableEvents[country].Add(ev);
			}
		}

		bool IsViewable(IEvent ev)
		{
			return ev.Language?.ToLower() == "en";
		}
	}
}