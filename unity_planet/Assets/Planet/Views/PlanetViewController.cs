using System.Collections.Generic;
using System.Linq;
using Planet.CountryCodeToGps;
using Planet.Data;
using Planet.Models;
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

		[SerializeField]
		EventHeadlineView[] _eventHeadlineViews;

		CountryGpsDictionary _countryGpsDictionary;
		IEventSource _eventSource;
		Dictionary<string, MarkerView> _markers;

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

			int index = 0;
			foreach (var focusedCountry in _countryFocusObserveer.FocusedCountries)
			{
				if (_eventSource[focusedCountry].FirstOrDefault() is IEventHeadline firstEvent)
				{
					_eventHeadlineViews[index].Load(firstEvent).Forget(Debug.LogException);
					index += 1;
				}
			}
		}
	}
}