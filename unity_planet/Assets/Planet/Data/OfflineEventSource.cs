using System;
using System.Collections.Generic;
using System.Linq;
using Planet.Models;
using Planet.OfflineNewsSources;
using UniRx;

namespace Planet.Data
{
	public class OfflineEventSource : IEventSource, IDisposable
	{
		readonly Dictionary<string, IEnumerable<IEventDetailed>> _events;
		readonly ReplaySubject<string> _onAdded;

		public OfflineEventSource(OfflineNewsSource source)
		{
			_events = new Dictionary<string, IEnumerable<IEventDetailed>>();
			_onAdded = new ReplaySubject<string>();
			foreach (var country in source.Countries)
			{
				_events[country] = source[country].Select(a => new OfflineEvent(a));
				_onAdded.OnNext(country);
			}
		}

		public IEnumerable<IEventDetailed> this[string country] => _events[country];
		public bool HasCountry(string country) => _events.ContainsKey(country);
		public IEnumerable<string> Countries => _events.Keys;
		public IObservable<string> OnEventsAddedToCountry => _onAdded;

		public void Dispose()
		{
			_onAdded?.Dispose();
		}
	}
}