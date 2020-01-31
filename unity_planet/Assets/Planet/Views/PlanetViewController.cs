using System.Collections.Generic;
using System.Linq;
using Planet.CountryCodeToGps;
using Planet.Data;
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
		Transform _markerRoot;

		[SerializeField]
		MarkerView _markerPrefab;

		[SerializeField]
		CountryFocusObserver _countryFocusObserveer;

		[SerializeField, DisableInPlayMode]
		EventHeadlineView[] _eventHeadlineViews;

		CountryGpsDictionary _countryGpsDictionary;
		IEventSource _eventSource;
		Dictionary<string, MarkerView> _markers;
		EventViewMapper _eventViewMapper;

		[Inject]
		public void Inject(CountryGpsDictionary countryGpsDictionary)
		{
			_countryGpsDictionary = countryGpsDictionary;
		}

		[Inject]
		public void Inject(IEventSource eventSource)
		{
			_eventSource = eventSource;
		}

		void Start()
		{
			_markers = new Dictionary<string, MarkerView>();
			_eventViewMapper = new EventViewMapper(_eventHeadlineViews.Length);

			_eventSource
				.OnEventsAddedToCountry
				.ThrottleFirstFrame(1)
				.Subscribe(_ => OnEventSourceUpdated())
				.AddTo(this);

			_countryFocusObserveer
				.OnFocusedCountriesUpdated
				.Subscribe(_ => OnFocusedCountriesUpdated())
				.AddTo(this);
		}

		void OnEventSourceUpdated()
		{
			_countryFocusObserveer.Initialize(_eventSource.Countries);

			foreach (var existingMarker in _markers.Values)
			{
				Destroy(existingMarker.gameObject);
			}

			_markers.Clear();

			foreach (var country in _eventSource.Countries)
			{
				var latlong = _countryGpsDictionary[country];
				var marker = Instantiate(_markerPrefab, _markerRoot);
				marker.name = $"Marker ({country})";
				marker.SetPosition(latlong.Latitude, latlong.Longitude);
				marker.SetFocused(false);

				_markers.Add(country, marker);
			}
		}

		void OnFocusedCountriesUpdated()
		{
			foreach (var pair in _markers)
			{
				var country = pair.Key;
				var isFocused = _countryFocusObserveer.IsFocused(country);

				// set marker view focused
				pair.Value.SetFocused(isFocused);
			}

			var focusedCountries = _countryFocusObserveer.FocusedCountries;
			focusedCountries = focusedCountries.Where(c => _eventSource[c].Any());

			_eventViewMapper.UpdateMapping(focusedCountries);

			var mappedCountries = _eventViewMapper.MappedCountries;
			var mappedEventViews = mappedCountries.Zip(_eventHeadlineViews, (c, v) => (c, v));

			foreach (var (country, eventView) in mappedEventViews)
			{
				if (country != null)
				{
					var featuredEvent = _eventSource[country].First();
					eventView.Load(featuredEvent).Forget(Debug.LogException);
				}
				else
				{
					eventView.Hide();
				}
			}

			// Debug.Log("focused: " + string.Join(", ", focusedCountries));
			// Debug.Log("mapped: " + string.Join(", ", _eventViewMapper.MappedCountries));
		}
	}
}