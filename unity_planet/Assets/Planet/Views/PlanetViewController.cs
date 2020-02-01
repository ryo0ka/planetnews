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
		MarkerViewController _markers;

		[SerializeField]
		CountryFocusObserver _focusObserver;

		[SerializeField, DisableInPlayMode]
		EventHeadlineView[] _eventHeadlineViews;

		[SerializeField]
		Material _lineMaterial;

		IEventStreamer _eventStreamer;
		EventViewMapper _eventViewMapper;
		ViewableEventFilter _viewableFilter;
		List<string> _viewMapping;

		[Inject]
		public void Inject(IEventStreamer eventStreamer)
		{
			_eventStreamer = eventStreamer;
		}

		void Start()
		{
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
			_markers.AddMarker(country);

			// Enable to focus on viewable countries
			var events = _eventStreamer.GetEvents(country);
			var viewable = _viewableFilter.Filter(events).Any();
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
				_markers.TryGetMarkerWorldPosition(mappedCountry, out var mappedMarkerPositionn);

				RenderGlLine(
					_lineMaterial,
					mappedMarkerPositionn,
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