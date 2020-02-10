using System.Collections.Generic;
using Planet.CountryCodeToGps;
using Planet.Utils;
using UnityEngine;
using Zenject;

namespace Planet.Views
{
	public sealed class MarkerCollectionView : MonoBehaviour
	{
		[SerializeField]
		Transform _markerRoot;

		[SerializeField]
		MarkerView _markerPrefab;

		[SerializeField]
		float _heightOffset;

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

			var marker = Instantiate(_markerPrefab, _markerRoot);
			marker.name = $"Marker ({country})";
			marker.SetFocused(false);
			_markers.Add(country, marker);

			var latlong = _countryGpsDictionary[country];
			var rot = PlanetMath.GpsToSpherical(latlong.Latitude, latlong.Longitude);
			var pos = rot * (Vector3.forward * (0.5f + _heightOffset));
			var markerT = marker.transform;
			markerT.localRotation = rot;
			markerT.localPosition = pos;
			markerT.localScale = Vector3.one;
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

		public Transform GetAnchor(string country)
		{
			return _markers[country].transform;
		}
	}
}