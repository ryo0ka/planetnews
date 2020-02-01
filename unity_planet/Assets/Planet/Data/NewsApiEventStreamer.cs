using System;
using System.Collections.Generic;
using System.Linq;
using NewsAPI.Models;
using Planet.Models;
using UniRx;

namespace Planet.Data
{
	public class NewsApiEventStreamer : IEventStreamer, IDisposable
	{
		readonly NewsApiEventFactory _eventFactory;
		readonly Dictionary<string, List<IEvent>> _events;
		readonly List<(string, IEnumerable<NewsApiArticle>)> _source;
		readonly Subject<IEvent> _onEventAdded;

		public NewsApiEventStreamer(NewsApiEventFactory eventFactory)
		{
			_eventFactory = eventFactory;
			_events = new Dictionary<string, List<IEvent>>();
			_source = new List<(string, IEnumerable<NewsApiArticle>)>();
			_onEventAdded = new Subject<IEvent>();
		}

		public IEnumerable<string> Countries => _events.Keys;
		public IObservable<IEvent> OnEventAdded => _onEventAdded;

		public IEnumerable<IEvent> GetEvents(string country)
		{
			if (_events.TryGetValue(country, out var events))
			{
				return events;
			}

			return Enumerable.Empty<IEvent>();
		}

		public void SetArticles(string country, IEnumerable<NewsApiArticle> articles)
		{
			_source.Add((country, articles));
		}

		public void StartStreaming()
		{
			foreach (var (country, articles) in _source)
			{
				if (!_events.TryGetValue(country, out var events))
				{
					events = new List<IEvent>();
					_events[country] = events;
				}

				foreach (var article in articles)
				{
					var ev = _eventFactory.MakeEvent(article, country);
					events.Add(ev);
					_onEventAdded.OnNext(ev);
				}
			}
		}

		public void Dispose()
		{
			_onEventAdded.Dispose();
		}
	}
}