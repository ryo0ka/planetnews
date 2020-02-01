using System.Collections.Generic;
using System.Linq;
using Planet.CountryCodeToGps;
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
		IEventStreamer _eventStreamer;
		Dictionary<string, MarkerView> _markers;
		EventViewMapper _eventViewMapper;
		ViewableEventFilter _viewableFilter;
		List<string> _viewMapping;

		[Inject]
		public void Inject(CountryGpsDictionary countryGpsDictionary)
		{
			_countryGpsDictionary = countryGpsDictionary;
		}

		[Inject]
		public void Inject(IEventStreamer eventStreamer)
		{
			_eventStreamer = eventStreamer;
		}

		void Start()
		{
			_markers = new Dictionary<string, MarkerView>();
			_eventViewMapper = new EventViewMapper(_eventHeadlineViews.Length);
			_viewableFilter = new ViewableEventFilter();
			_viewMapping = new List<string>();

			_focusObserver
				.OnFocusedCountriesUpdated
				.Subscribe(_ => OnFocusChanged())
				.AddTo(this);

			var postRender = Camera.main.gameObject.AddComponent<CameraPostRenderObserver>();
			postRender
				.ObservePostRender
				.Subscribe(_ => OnMainCameraPostRender())
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
			if (!_markers.ContainsKey(country))
			{
				var latlong = _countryGpsDictionary[country];
				var marker = Instantiate(_markerPrefab, _markerRoot);
				marker.name = $"Marker ({country})";
				marker.SetPosition(latlong.Latitude, latlong.Longitude);
				marker.SetFocused(false);

				_markers.Add(country, marker);
			}

			// Enable to focus on viewable countries
			var events = _eventStreamer.GetEvents(country);
			var viewable = _viewableFilter.Filter(events).Any();
			//Debug.Log($"{events.Count()} {viewable}");
			_markers[country].SetViewable(viewable);
			_focusObserver.SetViewable(country, viewable);
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
			var mappedEventViews = _viewMapping.Zip(_eventHeadlineViews, (c, v) => (c, v));
			foreach (var (country, eventView) in mappedEventViews)
			{
				if (country == null)
				{
					eventView.Hide();
					continue;
				}

				var events = _eventStreamer.GetEvents(country);
				if (_viewableFilter.Filter(events).TryLast(out var ev))
				{
					eventView.Load(ev).Forget(Debug.LogException);
					//Debug.Log(ev);
					continue;
				}

				Debug.LogError($"No viewable events in focused country: {country}");
			}
		}

		void OnMainCameraPostRender()
		{
			// Connect markers and event views with lines
			var mappedCountries = _eventViewMapper.MappedCountries;
			for (var i = 0; i < mappedCountries.Count; i++)
			{
				var mappedCountry = mappedCountries[i];
				if (mappedCountry == null) continue;

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