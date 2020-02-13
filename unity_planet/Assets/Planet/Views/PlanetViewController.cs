using System;
using System.Collections.Generic;
using System.Linq;
using Planet.Data;
using Planet.Models;
using Planet.Utils;
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
		HeadlinePanelCollectionView _panels;

		Camera _camera;
		IEventStreamer _events;
		List<string> _countries;
		List<(string, int)?> _mappedEvents;
		List<(string, int)?> _lastMappedEvents;
		List<(string, int)?> _tmpLastMappedEvents;

		const float MaxViewAngle = 90;

		void Awake()
		{
			_countries = new List<string>();
			_mappedEvents = new List<(string, int)?>();
			_lastMappedEvents = new List<(string, int)?>();
			_tmpLastMappedEvents = new List<(string, int)?>();
		}

		[Inject]
		public void Inject(IEventStreamer eventStreamer)
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
			_countries.Add(country);

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
			_mappedEvents.Fill(0, _panels.Count, null);

			_tmpLastMappedEvents.Clear();
			_tmpLastMappedEvents.AddRange(_lastMappedEvents);

			var tmpViewAngles = new Dictionary<string, Vector2>();

			// sample view angles
			foreach (var country in _countries)
			{
				var viewAngles = _markers.CalcViewAngles(country, _camera.transform);
				if (TestThresholdAngles(viewAngles))
				{
					tmpViewAngles[country] = viewAngles;
				}
			}

			var tmpCountriesSortedByViewAngle =
				tmpViewAngles
					.OrderBy(p => p.Value, new ViewAngleComparer())
					.Select(p => p.Key)
					.Take(_panels.Count)
					.ToList();

			var tmpCountriesSortedByVerticalViewAngle =
				tmpCountriesSortedByViewAngle
					.OrderBy(c => tmpViewAngles[c].y)
					.ToList();

			var tmpEventCounts = new Dictionary<string, int>();

			//Debug.Log("0 old: " + string.Join(", ", _lastMappedCountries));

			var mappedCount = 0;
			while (mappedCount < _panels.Count)
			{
				var hasMoreEvents = false;

				// iterating thru countries sorted in vertical view angles (top to bottom)
				foreach (var country in tmpCountriesSortedByVerticalViewAngle)
				{
					var eventIndex = tmpEventCounts.GetValueOrElse(country, 0);
					var eventCount = _events.GetEvents(country).Count;
					if (eventIndex >= eventCount) continue; // no more events to show

					var foundSameEventInLastMapping = false;
					var foundIndex = -1;
					for (var i = 0; i < _tmpLastMappedEvents.Count; i++)
					{
						var lastMappedEvent = _tmpLastMappedEvents[i];
						if (lastMappedEvent == (country, eventIndex))
						{
							foundSameEventInLastMapping = true;
							foundIndex = i;
							break;
						}
					}

					if (!foundSameEventInLastMapping)
					{
						for (var i = 0; i < _tmpLastMappedEvents.Count; i++)
						{
							var lastMappedEvent = _tmpLastMappedEvents[i];
							if (lastMappedEvent?.Item1 == country)
							{
								foundSameEventInLastMapping = true;
								foundIndex = i;
								break;
							}
						}
					}

					if (foundSameEventInLastMapping)
					{
						_tmpLastMappedEvents[foundIndex] = null;
						_mappedEvents[foundIndex] = (country, eventIndex);
					}
					else if (_mappedEvents.IndexOf(null) is var emptyIndex &&
					         emptyIndex >= 0)
					{
						_mappedEvents[emptyIndex] = (country, eventIndex);
					}

					tmpEventCounts[country] = eventIndex + 1;
					mappedCount += 1;
					hasMoreEvents = true;
				}

				if (!hasMoreEvents) break;
			}

			if (!_mappedEvents.SequenceEqual(_lastMappedEvents))
			{
				Debug.Log("1 new: " + string.Join(", ", _mappedEvents.Select(n => $"({n?.Item1}, {n?.Item2})")) +
				          " <- " + string.Join(", ", _tmpLastMappedEvents));

				OnMappingChanged();

				_lastMappedEvents.Clear();
				_lastMappedEvents.AddRange(_mappedEvents);
			}
		}

		bool TestThresholdAngles(Vector2 viewAngles)
		{
			return Mathf.Abs(viewAngles.x) < MaxViewAngle && Mathf.Abs(viewAngles.y) < MaxViewAngle;
		}

		void OnMappingChanged()
		{
			_lines.PrepareUpdateConnections();
			_markers.SetAllMarkersHighlighted(false);

			for (var i = 0; i < _panels.Count; i++)
			{
				var panel = _panels[i];
				if (_mappedEvents[i] is ValueTuple<string, int> n)
				{
					var (country, eventIndex) = n;
					var events = _events.GetEvents(country);
					var ev = events[events.Count - eventIndex - 1];
					panel.Load(ev).Forget(Debug.LogException);

					var markerAnchor = _markers.GetAnchor(country);
					var panelAnchor = panel.transform;
					_lines.Connect(markerAnchor, panelAnchor);

					_markers.SetMarkerHighlighted(country, true);
				}
				else
				{
					panel.Hide();
				}
			}

			_lines.UpdateConnections();
		}

		struct ViewAngleComparer : IComparer<Vector2>
		{
			public int Compare(Vector2 a, Vector2 b)
			{
				return Comparer<float>.Default.Compare(a.magnitude, b.magnitude);
			}
		}
	}
}