using System;
using System.Collections.Generic;
using NewsAPI.Models;
using Planet.Models;

namespace Planet.Data
{
	public class NewsApiEventStreamer : IEventStreamer, IDisposable
	{
		readonly NewsApiEventFactory _eventFactory;
		readonly Dictionary<string, List<IEvent>> _events;

		public NewsApiEventStreamer(NewsApiEventFactory eventFactory)
		{
			_eventFactory = eventFactory;
			_events = new Dictionary<string, List<IEvent>>();
		}

		public IEnumerable<string> Countries => _events.Keys;
		public IEnumerable<IEvent> GetEvents(string country) => _events[country];

		public void SetArticles(string country, IEnumerable<NewsApiArticle> articles)
		{
			if (!_events.TryGetValue(country, out var events))
			{
				events = new List<IEvent>();
			}

			foreach (var article in articles)
			{
				if (_eventFactory.TryMakeEvent(article, country, out var ev))
				{
					events.Add(ev);
				}
			}

			_events[country] = events;
		}

		public void StartStreaming()
		{
		}

		public void Dispose()
		{
		}
	}
}