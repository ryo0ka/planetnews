using System;
using System.Collections.Generic;
using System.Linq;
using NewsAPI.Constants;
using NewsAPI.OfflineCopies;
using Planet.Models;
using UnityEngine;

namespace Planet.Data
{
	public class OfflineEventSource : IEventSource, IDisposable
	{
		readonly NewsApiOfflineCopy _io;
		readonly Dictionary<string, List<IEventHeadline>> _events;

		bool _loaded;

		public OfflineEventSource(NewsApiOfflineCopy io)
		{
			_io = io;
			_events = new Dictionary<string, List<IEventHeadline>>();
			_loaded = false;
		}

		public IEnumerable<string> Countries => _events.Keys;
		public IEnumerable<IEventHeadline> GetEvents(string country) => _events[country];

		public void StartLoading()
		{
			if (_loaded) return;
			_loaded = true;

			var id = 0;
			var sources = _io.ReadSources().ToDictionary(s => s.Name);
			var countries = (NewsApiCountry[]) Enum.GetValues(typeof(NewsApiCountry));
			foreach (var country in countries)
			{
				var countryStr = country.ToString();
				var events = _events[countryStr] = new List<IEventHeadline>();
				foreach (var article in _io.ReadArticles(countryStr))
				{
					var sourceName = article.Source.Name;
					if (!sources.TryGetValue(sourceName, out var source) || source == null)
					{
						Debug.LogWarning($"Source not found: {sourceName}");
					}

					var language = source.Language;
					var ev = new NewsApiEvent(id++, article, language);
					events.Add(ev);
				}

				_events.Add(countryStr, events);
			}
		}

		public void Dispose()
		{
		}
	}
}