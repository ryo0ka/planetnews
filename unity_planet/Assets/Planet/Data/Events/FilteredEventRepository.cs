using System;
using System.Collections.Generic;
using Planet.Models;
using Planet.Utils;
using UniRx;

namespace Planet.Data.Events
{
	public sealed class FilteredEventRepository : IEventRepository, IDisposable
	{
		readonly IEventSource _source;
		readonly IEventFilter _filter;
		readonly CompositeDisposable _disposable;
		readonly Subject<IEvent> _eventsAdded;
		readonly Dictionary<string, List<IEvent>> _events;
		readonly List<string> _countries;

		public FilteredEventRepository(IEventSource source, IEventFilter filter)
		{
			_source = source;
			_filter = filter;
			_disposable = new CompositeDisposable();
			_eventsAdded = new Subject<IEvent>().AddTo(_disposable);
			_events = new Dictionary<string, List<IEvent>>();
			_countries = new List<string>();

			_source.OnEventAdded
			       .Subscribe(e => AddSourceEvent(e))
			       .AddTo(_disposable);
		}

		public IEnumerable<string> Countries => _countries;
		public IObservable<IEvent> OnEventAdded => _eventsAdded;

		void AddSourceEvent(IEvent e)
		{
			if (!_filter.Test(e)) return;

			if (!_events.TryGetValue(e.Country, out var events))
			{
				events = new List<IEvent>();
				_events[e.Country] = events;
			}

			events.Add(e);
			_countries.Add(e.Country);
			_eventsAdded.OnNext(e);
		}

		public IReadOnlyList<IEvent> GetEvents(string country)
		{
			if (_events.TryGetValue(country, out var events))
			{
				return events;
			}

			return LinqUtils.EmptyList<IEvent>();
		}

		public void StartStreaming() => _source.StartStreaming();

		public void Dispose() => _disposable.Dispose();
	}
}