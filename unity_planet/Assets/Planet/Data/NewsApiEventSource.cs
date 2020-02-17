using System;
using System.Collections.Generic;
using NewsAPI.Models;
using Planet.Models;
using UniRx;

namespace Planet.Data
{
	public class NewsApiEventSource : IEventSource, IDisposable
	{
		readonly NewsApiEventFactory _eventFactory;
		readonly List<(string, IEnumerable<NewsApiArticle>)> _source;
		readonly Subject<IEvent> _onEventAdded;

		public NewsApiEventSource(NewsApiEventFactory eventFactory)
		{
			_eventFactory = eventFactory;
			_source = new List<(string, IEnumerable<NewsApiArticle>)>();
			_onEventAdded = new Subject<IEvent>();
		}

		public IObservable<IEvent> OnEventAdded => _onEventAdded;

		/// <summary>
		/// Add articles to this source.
		/// Should be all invoked before `StartStreaming()` is invoked.
		/// </summary>
		public void SetArticles(string country, IEnumerable<NewsApiArticle> articles)
		{
			_source.Add((country, articles));
		}

		public void StartStreaming()
		{
			foreach (var (country, articles) in _source)
			foreach (var article in articles)
			{
				var ev = _eventFactory.MakeEvent(article, country);
				_onEventAdded.OnNext(ev);
			}
		}

		public void Dispose()
		{
			_onEventAdded.Dispose();
		}
	}
}