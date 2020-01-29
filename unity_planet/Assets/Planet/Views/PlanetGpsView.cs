using Planet.CountryCodeToGps;
using Planet.OfflineNewsSources;
using UnityEngine;

namespace Planet.Views
{
	public class PlanetGpsView : MonoBehaviour
	{
		[SerializeField]
		Transform _markerRoot;

		[SerializeField]
		Transform _markerPrefab;

		[SerializeField]
		float _markerHeightOffset;

		CountryGpsDictionary _countryGpsDictionary;
		OfflineNewsSource _newsSource;

		void Start()
		{
			_countryGpsDictionary = CountryGpsFactory.FromResource();
			_newsSource = OfflineNewsSourceReader.FromResource();
			foreach (var country in _newsSource.Countries)
			{
				SetMarker(country);
			}
		}

		void SetMarker(string country)
		{
			var latlong = _countryGpsDictionary[country];

			var marker = Instantiate(_markerPrefab, _markerRoot);
			marker.name = $"Marker ({country})";

			marker.localEulerAngles = new Vector2(
				latlong.Latitude * -1f,
				latlong.Longitude * -1f);

			var view = marker.GetChild(0);
			view.localPosition = new Vector3(
				0, 0, 0.5f + _markerHeightOffset); // assume the diameter be 1f
		}
	}
}