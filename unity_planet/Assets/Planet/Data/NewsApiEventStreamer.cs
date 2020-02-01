using System;
using System.Collections.Generic;
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

		public bool TryGetEvents(string country, out IEnumerable<IEvent> events)
		{
			if (_events.TryGetValue(country, out var eventList))
			{
				events = eventList;
				return true;
			}

			events = null;
			return false;
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
				}

				foreach (var article in articles)
				{
					if (_eventFactory.TryMakeEvent(article, country, out var ev))
					{
						events.Add(ev);
						_onEventAdded.OnNext(ev);
					}
				}

				_events[country] = events;
			}
		}

		public void Dispose()
		{
			_onEventAdded.Dispose();
		}
	}
}