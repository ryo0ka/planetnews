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
		MarkerTransformer _markerPrefab;

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

			marker.SetPosition(latlong.Latitude, latlong.Longitude);
		}
	}
}