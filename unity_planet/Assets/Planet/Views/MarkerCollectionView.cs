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
			marker.SetHighlighted(false);
			_markers.Add(country, marker);

			var latlong = _countryGpsDictionary[country];
			var rot = MathUtils.GpsToSpherical(latlong.Latitude, latlong.Longitude);
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

		public void SetMarkerHighlighted(string country, bool highlighted)
		{
			if (!_markers.TryGetValue(country, out var marker))
			{
				Debug.LogError($"Marker not found for {country}");
				return;
			}

			marker.SetHighlighted(highlighted);
		}

		public void SetAllMarkersHighlighted(bool highlighted)
		{
			foreach (var pair in _markers)
			{
				pair.Value.SetHighlighted(highlighted);
			}
		}

		public Transform GetAnchor(string country)
		{
			return _markers[country].transform;
		}

		public Vector2 CalcViewAngles(string country, Transform viewer)
		{
			var anchor = GetAnchor(country);
			var forwardVector = viewer.position - anchor.position;
			var forward = Quaternion.LookRotation(forwardVector, Vector3.up);

			var horizontalPlane = forward * Vector3.up;
			var horizontalVector = Vector3.ProjectOnPlane(anchor.forward, horizontalPlane);
			var horizontalAngle = Vector3.SignedAngle(forwardVector, horizontalVector, horizontalPlane);

			var verticalPlane = forward * Vector3.right;
			var verticalVector = Vector3.ProjectOnPlane(anchor.forward, verticalPlane);
			var verticalAngle = Vector3.SignedAngle(forwardVector, verticalVector, verticalPlane);

			DrawDebugLineToward(anchor.position, forward, 0.1f, Color.magenta);
			DrawDebugLineToward(anchor.position, Quaternion.LookRotation(horizontalVector, Vector3.up), 0.1f, Color.green);
			DrawDebugLineToward(anchor.position, Quaternion.LookRotation(verticalVector, Vector3.up), 0.1f, Color.yellow);

			return new Vector2(horizontalAngle, verticalAngle);
		}

		void DrawDebugLineToward(Vector3 position, Quaternion rotation, float length, Color color)
		{
			Debug.DrawLine(position, position + rotation * (Vector3.forward * length), color);
		}
	}
}