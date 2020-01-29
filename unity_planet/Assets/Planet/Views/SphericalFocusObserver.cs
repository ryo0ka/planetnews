using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Planet.CountryCodeToGps;
using UnityEngine;
using UnityEngine.Profiling;

namespace Planet.Views
{
	[Serializable]
	public class SphericalFocusObserver
	{
		[SerializeField]
		int _maxFocusedMarkerCount;

		IEnumerable<string> _countries;
		CountryGpsDictionary _countryGpsDictionary;
		HashSet<string> _focusedCountries;
		LinkedList<(string Country, float ViewAngle)> _markerViewAngles;

		[UsedImplicitly] // for tests
		public int MaxFocusedMarkerCount
		{
			get => _maxFocusedMarkerCount;
			set => _maxFocusedMarkerCount = value;
		}

		public void Initialize(
			IEnumerable<string> countries,
			CountryGpsDictionary countryGpsDictionary)
		{
			_countries = countries;
			_countryGpsDictionary = countryGpsDictionary;
			_markerViewAngles = new LinkedList<(string, float)>();
			_focusedCountries = new HashSet<string>();
		}

		public void UpdateFocus(Quaternion look)
		{
			Profiler.BeginSample("CountryFocusObserver.UpdateFocus()");

			_markerViewAngles.Clear();
			_focusedCountries.Clear();

			var focusedCountries =
				_countries
					.Select(country =>
					{
						var angle = CountryToSphericalAngle(country);
						var viewAngle = Quaternion.Angle(angle, look);
						return (Country: country, ViewAngle: viewAngle);
					})
					.OrderBy(p => p.ViewAngle)
					.Take(_maxFocusedMarkerCount);

			foreach (var (focusedCountry, _) in focusedCountries)
			{
				_focusedCountries.Add(focusedCountry);
			}

			Profiler.EndSample();
		}

		Quaternion CountryToSphericalAngle(string country)
		{
			var gps = _countryGpsDictionary[country];
			return PlanetMath.GpsToSpherical(gps.Latitude, gps.Longitude);
		}

		public bool IsFocused(string country)
		{
			return _focusedCountries.Contains(country);
		}
	}
}