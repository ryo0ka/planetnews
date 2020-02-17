using System.Collections.Generic;
using System.Linq;
using Planet.Data;
using Planet.Data.Events;
using Planet.Models;
using Planet.Utils;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Profiling;
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
		HeadlinePanelCollectionView _panels;

		Camera _camera;
		IEventRepository _events;
		List<(string, int)?> _mappedEvents;
		List<(string, int)?> _lastMappedEvents;
		List<(string, int)?> _lastMappedEventsCopy;
		List<(string, Vector2)> _countriesSorted;
		Dictionary<string, int> _eventCounts;
		ViewAngleComparer _viewAngleComparer;
		VerticalViewAngleComparer _verticalViewAngleComparer;

		const float MaxViewAngle = 60;

		void Awake()
		{
			_mappedEvents = new List<(string, int)?>();
			_lastMappedEvents = new List<(string, int)?>();
			_lastMappedEventsCopy = new List<(string, int)?>();
			_countriesSorted = new List<(string, Vector2)>();
			_eventCounts = new Dictionary<string, int>();
			_viewAngleComparer = new ViewAngleComparer();
			_verticalViewAngleComparer = new VerticalViewAngleComparer();
		}

		[Inject]
		public void Inject(IEventRepository eventStreamer)
		{
			_events = eventStreamer;
		}

		void Start()
		{
			_camera = Camera.main;

			_events
				.OnEventAdded
				.Subscribe(e => OnEventAdded(e))
				.AddTo(this);

			_events.StartStreaming();
		}

		void OnEventAdded(IEvent ev)
		{
			var country = ev.Country;
			
			// Instantiate markers for new countries
			_markers.AddMarker(country);
			_markers.SetMarkerViewable(country, true);
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if (!Application.isPlaying) return;

			foreach (var c in _mappedEvents)
			{
				if (!c.HasValue) return;
				var (country, _) = c.Value;

				var viewAngle = _markers.CalcViewAngles(country, _camera.transform);
				var angle = viewAngle.magnitude;
				var angleNormal = Mathf.InverseLerp(0, MaxViewAngle, angle);
				UnityEditor.Handles.color = Color.Lerp(Color.green, Color.red, angleNormal);

				var anchor = _markers.GetAnchor(country);
				UnityEditor.Handles.SphereHandleCap(
					c.GetHashCode(),
					anchor.position,
					Quaternion.identity,
					0.02f,
					Event.current.type);

				var eventCount = _events.GetEvents(country).Count;
				UnityEditor.Handles.Label(anchor.position, $"{eventCount}");
			}
		}
#endif

		void Update()
		{
			Profiler.BeginSample("Planet/PlanetViewController.Update()");

			Profiler.BeginSample("Initialize");

			_mappedEvents.Fill(0, _panels.Count, null);

			_lastMappedEventsCopy.Clear();
			_lastMappedEventsCopy.AddRange(_lastMappedEvents);

			_countriesSorted.Clear();
			_eventCounts.Clear();

			Profiler.EndSample();
			Profiler.BeginSample("Sample view angles");

			// sample view angles
			foreach (var country in _events.Countries)
			{
				var viewAngles = _markers.CalcViewAngles(country, _camera.transform);
				if (TestThresholdAngles(viewAngles))
				{
					_countriesSorted.Add((country, viewAngles));
				}
			}

			Profiler.EndSample();
			Profiler.BeginSample("Sort by view angles");

			// Sort by view angles from the camera
			_countriesSorted.Sort(_viewAngleComparer);

			// Get rid of countries outside bound (panel count)
			_countriesSorted.TrimLength(_panels.Count);

			Profiler.EndSample();
			Profiler.BeginSample("Sort by vertical view angles");

			// Sort by vertical view angles
			_countriesSorted.Sort(_verticalViewAngleComparer);

			Profiler.EndSample();
			Profiler.BeginSample("Map countries");

			var mappedCount = 0;
			while (mappedCount < _panels.Count)
			{
				var hasMoreEvents = false;

				// iterating thru countries sorted in vertical view angles (top to bottom)
				foreach (var (country, _) in _countriesSorted)
				{
					Profiler.BeginSample("Test event count");

					var eventIndex = _eventCounts.GetValueOrElse(country, 0);
					var eventCount = _events.GetEvents(country).Count;

					Profiler.EndSample();

					if (eventIndex >= eventCount) continue; // got no more events to show

					hasMoreEvents = true;

					Profiler.BeginSample("Search matching event from last mapping");

					var foundMatchInLastMapping =
						TryGetExactMatch((country, eventIndex), _lastMappedEventsCopy, out var matchedIndex) ||
						TryGetMatch((country, eventIndex), _lastMappedEventsCopy, out matchedIndex);

					Profiler.EndSample();

					if (foundMatchInLastMapping)
					{
						// keep the matching event on the same panel
						_mappedEvents[matchedIndex] = (country, eventIndex);

						// remove the event from the lookup
						_lastMappedEventsCopy[matchedIndex] = null;
					}
					else if (_mappedEvents.TryIndexOf(null, out var mappableIndex))
					{
						// insert the event in an empty panel
						_mappedEvents[mappableIndex] = (country, eventIndex);
					}

					_eventCounts[country] = eventIndex + 1;
					mappedCount += 1;
				}

				if (!hasMoreEvents) break;
			}

			Profiler.EndSample();

			if (!_mappedEvents.SequenceEqual(_lastMappedEvents))
			{
				OnMappingChanged();

				_lastMappedEvents.Clear();
				_lastMappedEvents.AddRange(_mappedEvents);
			}

			Profiler.EndSample();
		}

		bool TestThresholdAngles(Vector2 viewAngles)
		{
			return Mathf.Abs(viewAngles.x) < MaxViewAngle && Mathf.Abs(viewAngles.y) < MaxViewAngle;
		}

		bool TryGetExactMatch((string, int) e, IReadOnlyList<(string, int)?> mapping, out int matchIndex)
		{
			for (var i = 0; i < mapping.Count; i++)
			{
				var lastMappedEvent = mapping[i];
				if (lastMappedEvent == e)
				{
					matchIndex = i;
					return true;
				}
			}

			matchIndex = -1;
			return false;
		}

		bool TryGetMatch((string, int) e, IReadOnlyList<(string, int)?> mapping, out int matchIndex)
		{
			for (var i = 0; i < mapping.Count; i++)
			{
				var lastMappedEvent = mapping[i];
				if (lastMappedEvent?.Item1 == e.Item1)
				{
					matchIndex = i;
					return true;
				}
			}

			matchIndex = -1;
			return false;
		}

		void OnMappingChanged()
		{
			Profiler.BeginSample("Planet/Update lines and markers");

			Profiler.BeginSample("Initialize");

			_lines.PrepareUpdateConnections();
			_markers.SetAllMarkersHighlighted(false);

			Profiler.EndSample();

			for (var i = 0; i < _panels.Count; i++)
			{
				var panel = _panels[i];
				var pair = _mappedEvents[i];
				if (pair.HasValue)
				{
					Profiler.BeginSample("Connect marker and panel");

					var (country, eventIndex) = pair.Value;
					var events = _events.GetEvents(country);
					var ev = events[events.Count - eventIndex - 1];

					Profiler.BeginSample("Load panel");

					panel.Load(ev).Forget(Debug.LogException);

					Profiler.EndSample();

					var markerAnchor = _markers.GetAnchor(country);
					var panelAnchor = panel.transform;
					_lines.Connect(markerAnchor, panelAnchor);

					_markers.SetMarkerHighlighted(country, true);

					Profiler.EndSample();
				}
				else
				{
					panel.Hide();
				}
			}

			_lines.UpdateConnections();

			Profiler.EndSample();
		}

		sealed class ViewAngleComparer : IComparer<(string, Vector2)>
		{
			public int Compare((string, Vector2) a, (string, Vector2) b)
			{
				return Comparer<float>.Default.Compare(a.Item2.magnitude, b.Item2.magnitude);
			}
		}

		sealed class VerticalViewAngleComparer : IComparer<(string, Vector2)>
		{
			public int Compare((string, Vector2) a, (string, Vector2) b)
			{
				return Comparer<float>.Default.Compare(a.Item2.y, b.Item2.y);
			}
		}
	}
}