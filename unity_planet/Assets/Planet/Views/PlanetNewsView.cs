using System.Collections.Generic;
using Planet.CountryCodeToGps;
using Planet.OfflineNewsSources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Planet.Views
{
	public class PlanetNewsView : MonoBehaviour
	{
		[SerializeField]
		Transform _planet;

		[SerializeField]
		Transform _markerRoot;

		[SerializeField]
		MarkerView _markerPrefab;

		[SerializeField, InlineProperty, HideLabel]
		SphericalFocusObserver sphericalFocusObserveer;

		CountryGpsDictionary _countryGpsDictionary;
		OfflineNewsSource _newsSource;
		Dictionary<string, MarkerView> _markers;
		Camera _mainCamera;

		void Start()
		{
			_countryGpsDictionary = CountryGpsFactory.FromResource();
			_newsSource = OfflineNewsSourceReader.FromResource();
			_markers = new Dictionary<string, MarkerView>();
			_mainCamera = Camera.main;

			sphericalFocusObserveer.Initialize(
				_newsSource.Countries, _countryGpsDictionary);

			foreach (var country in _newsSource.Countries)
			{
				SetMarker(country);
			}
		}

		void Update()
		{
			// Camera position relative to the planet object
			var cameraPos = _planet.InverseTransformPoint(_mainCamera.transform.position);
			var lookRotation = Quaternion.LookRotation(cameraPos, Vector3.up);
			sphericalFocusObserveer.UpdateFocus(lookRotation);
			foreach (var pair in _markers)
			{
				var focused = sphericalFocusObserveer.IsFocused(pair.Key);
				pair.Value.SetFocused(focused);
			}
		}

		void SetMarker(string country)
		{
			var latlong = _countryGpsDictionary[country];
			var marker = Instantiate(_markerPrefab, _markerRoot);
			marker.name = $"Marker ({country})";
			marker.SetPosition(latlong.Latitude, latlong.Longitude);
			marker.SetFocused(false);

			_markers.Add(country, marker);
		}
	}
}