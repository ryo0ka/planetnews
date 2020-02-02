using System;
using System.Collections.Generic;
using System.Linq;
using Planet.Data;
using Planet.Models;
using Planet.Utils;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace Planet.Views
{
	[SelectionBase]
	public class PlanetViewController : MonoBehaviour
	{
		[SerializeField]
		MarkerCollectionView _markers;

		[SerializeField]
		LineCollectionView _lines;

		[SerializeField]
		CountryFocusObserver _focusObserver;

		[SerializeField, DisableInPlayMode]
		EventHeadlineView[] _eventViews;

		IEventStreamer _eventStreamer;
		EventViewMapper _eventViewMapper;
		IEventFilter _eventFilter;
		List<string> _viewMapping;

		[Inject]
		public void Inject(IEventStreamer eventStreamer)
		{
			_eventStreamer = eventStreamer;
		}

		[Inject]
		public void Inject(IEventFilter eventFilter)
		{
			_eventFilter = eventFilter;
		}

		void Start()
		{
			_eventViewMapper = new EventViewMapper(_eventViews.Length);
			_viewMapping = new List<string>();

			_focusObserver
				.OnFocusedCountriesUpdated
				.Subscribe(_ => OnFocusChanged())
				.AddTo(this);

			_eventStreamer
				.OnEventAdded
				.Subscribe(e => OnEventAdded(e))
				.AddTo(this);

			_eventStreamer.StartStreaming();
		}

		void OnEventAdded(IEvent ev)
		{
			var country = ev.Country;

			// Instantiate markers for new countries
			_markers.AddMarker(country);

			// Enable to focus on viewable countries
			var events = _eventStreamer.GetEvents(country);
			var viewable = _eventFilter.Filter(events).Any();
			//Debug.Log($"{events.Count()} {viewable}");
			_markers.SetMarkerViewable(country, viewable);
			_focusObserver.SetViewable(country, viewable);
		}

		void OnFocusChanged()
		{
			// Apply focus state to marker views
			foreach (var country in _eventStreamer.Countries)
			{
				var focused = _focusObserver.IsFocused(country);
				_markers.SetMarkerFocused(country, focused);
			}

			// Update event view mapping
			var focusedCountries = _focusObserver.FocusedCountries;
			_eventViewMapper.UpdateMapping(focusedCountries);

			var viewMapping = _eventViewMapper.MappedCountries;
			if (!viewMapping.SequenceEqual(_viewMapping))
			{
				_viewMapping.Clear();
				_viewMapping.AddRange(viewMapping);
				OnMappingChanged();
			}
		}

		void OnMappingChanged()
		{
			var mappedCountryCount = _viewMapping.Count;
			_lines.SetLength(mappedCountryCount);
			for (var i = 0; i < mappedCountryCount; i++)
			{
				var country = _viewMapping[i];
				var eventView = _eventViews[i];

				if (country == null)
				{
					eventView.Hide();
					_lines.Disconnect(i);
					continue;
				}

				var events = _eventStreamer.GetEvents(country);
				if (_eventFilter.Filter(events).TryLast(out var ev))
				{
					eventView.Load(ev).Forget(Debug.LogException);

					var markerAnchor = _markers.GetAnchor(country);
					var eventViewAnchor = eventView.transform;
					_lines.Connect(i, markerAnchor, eventViewAnchor);

					//Debug.Log(ev);
					continue;
				}

				Debug.LogError($"No viewable events in focused country: {country}");
			}
		}
	}
}