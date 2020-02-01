using System.Collections.Generic;
using Planet.CountryCodeToGps;
using UnityEngine;
using Zenject;

namespace Planet.Views
{
	public sealed class MarkerViewController : MonoBehaviour
	{
		[SerializeField]
		Transform _markerRoot;

		[SerializeField]
		MarkerView _markerPrefab;

		Dictionary<string, MarkerView> _markers;
		CountryGpsDictionary _countryGpsDictionary;

		[Inject]
		public void Inject(CountryGpsDictionary countryGpsDictionary)
		{
			_countryGpsDictionary = countryGpsDictionary;
		}

		void Awake()
		{
			_markers = new Dictionary<string, MarkerView>();
		}

		public void AddMarker(string country)
		{
			if (_markers.ContainsKey(country)) return;

			var latlong = _countryGpsDictionary[country];
			var marker = Instantiate(_markerPrefab, _markerRoot);
			marker.name = $"Marker ({country})";
			marker.SetPosition(latlong.Latitude, latlong.Longitude);
			marker.SetFocused(false);

			_markers.Add(country, marker);
		}

		public void SetMarkerViewable(string country, bool viewable)
		{
			if (!_markers.TryGetValue(country, out var marker))
			{
				Debug.LogError($"Marker not found for {country}");
				return;
			}

			marker.SetViewable(viewable);
		}

		public void SetMarkerFocused(string country, bool focused)
		{
			if (!_markers.TryGetValue(country, out var marker))
			{
				Debug.LogError($"Marker not found for {country}");
				return;
			}

			marker.SetFocused(focused);
		}

		public bool TryGetMarkerWorldPosition(string country, out Vector3 worldPosition)
		{
			if (!_markers.TryGetValue(country, out var marker))
			{
				worldPosition = default;
				return false;
			}

			worldPosition = marker.WorldPosition;
			return true;
		}
	}
}