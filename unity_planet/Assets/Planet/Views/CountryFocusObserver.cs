using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Planet.CountryCodeToGps;
using Planet.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Planet.Views
{
	public class CountryFocusObserver : MonoBehaviour
	{
		[SerializeField]
		Transform _planet;

		[SerializeField]
		int _maxFocusedMarkerCount;

		IEnumerable<string> _countries;
		CountryGpsDictionary _countryGpsDictionary;
		HashSet<string> _focusedCountries;
		HashSet<string> _lastFocusedCountries;
		Camera _mainCamera;
		Subject<Unit> _updated;

		[UsedImplicitly] // for tests
		public int MaxFocusedMarkerCount
		{
			get => _maxFocusedMarkerCount;
			set => _maxFocusedMarkerCount = value;
		}

		public IObservable<Unit> OnFocusedCountriesUpdated => _updated;
		public IEnumerable<string> FocusedCountries => _focusedCountries;
		public bool IsFocused(string country) => _focusedCountries.Contains(country);

		void Awake()
		{
			_focusedCountries = new HashSet<string>();
			_lastFocusedCountries = new HashSet<string>();
			_updated = new Subject<Unit>().AddTo(this);
			_mainCamera = Camera.main;
		}

		[Inject]
		public void Inject(CountryGpsDictionary countryGpsDictionary)
		{
			_countryGpsDictionary = countryGpsDictionary;
		}

		public void Initialize(IEnumerable<string> countries)
		{
			_countries = countries;
		}

		void Update()
		{
			// Camera position relative to the planet object
			var cameraPos = _planet.InverseTransformPoint(_mainCamera.transform.position);
			var look = Quaternion.LookRotation(cameraPos, Vector3.up);

			_lastFocusedCountries.Clear();
			_lastFocusedCountries.UnionWith(_focusedCountries);
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

			if (!_focusedCountries.SetEquals(_lastFocusedCountries))
			{
				_updated.OnNext(Unit.Default);
			}
		}

		Quaternion CountryToSphericalAngle(string country)
		{
			var gps = _countryGpsDictionary[country];
			return PlanetMath.GpsToSpherical(gps.Latitude, gps.Longitude);
		}
	}
}