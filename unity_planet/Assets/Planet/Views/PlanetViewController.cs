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
		CountryFocusObserver _focusObserver;

		[SerializeField, DisableInPlayMode]
		EventHeadlineView[] _eventHeadlineViews;

		[SerializeField]
		Material _lineMaterial;

		CountryGpsDictionary _countryGpsDictionary;
		IEventSource _eventSource;
		Dictionary<string, MarkerView> _markers;
		EventViewMapper _eventViewMapper;
		ViewableEventsFilter _contentFilter;

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
			_contentFilter = new ViewableEventsFilter();

			_focusObserver
				.OnFocusedCountriesUpdated
				.Subscribe(_ => OnFocusChanged())
				.AddTo(this);

			var postRender = Camera.main.gameObject.AddComponent<CameraPostRenderObserver>();
			postRender
				.ObservePostRender
				.Subscribe(_ => OnMainCameraPostRender())
				.AddTo(this);

			_eventSource.StartLoading();
			OnEventsAdded();
		}

		void OnEventsAdded()
		{
			// Apply filter to newly loaded events
			foreach (var country in _eventSource.Countries)
			foreach (var ev in _eventSource.GetEvents(country))
			{
				_contentFilter.AddEvent(country, ev);
			}

			// Enable focusing on filtered (=viewable) countries
			_focusObserver.Initialize(_contentFilter.ViewableCountries);

			// Instantiate markers for all countries
			// TODO Destroy existing markers
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

		void OnFocusChanged()
		{
			// Apply focus state to marker views
			foreach (var pair in _markers)
			{
				var country = pair.Key;
				var markerView = pair.Value;

				var isFocused = _focusObserver.IsFocused(country);
				markerView.SetFocused(isFocused);
			}

			// Update event view mapping
			var focusedCountries = _focusObserver.FocusedCountries;
			_eventViewMapper.UpdateMapping(focusedCountries);

			// Update event views
			var mappedCountries = _eventViewMapper.MappedCountries;
			var mappedEventViews = mappedCountries.Zip(_eventHeadlineViews, (c, v) => (c, v));
			foreach (var (country, eventView) in mappedEventViews)
			{
				if (country != null)
				{
					var featuredEvent = _contentFilter.GetTopEvent(country);
					eventView.Load(featuredEvent).Forget(Debug.LogException);
				}
				else
				{
					eventView.Hide();
				}
			}
		}

		void OnMainCameraPostRender()
		{
			// Connect markers and event views with lines
			var mappedCountries = _eventViewMapper.MappedCountries;
			for (var i = 0; i < mappedCountries.Count; i++)
			{
				var mappedCountry = mappedCountries[i];
				var mappedEventView = _eventHeadlineViews[i];
				var mappedCountryMarker = _markers[mappedCountry];

				RenderGlLine(
					_lineMaterial,
					mappedCountryMarker.WorldPosition,
					mappedEventView.transform.position);
			}
		}

		static void RenderGlLine(Material mat, Vector3 v1, Vector3 v2)
		{
			GL.Begin(GL.LINES);
			mat.SetPass(0);
			GL.Vertex3(v1.x, v1.y, v1.z);
			GL.Vertex3(v2.x, v2.y, v2.z);
			GL.End();
		}
	}
}