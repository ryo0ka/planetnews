using System.Collections.Generic;
using NewsAPI.Constants;

namespace Planet.CountryCodeToGps
{
	public sealed class CountryGpsDictionary
	{
		readonly IReadOnlyDictionary<Countries, LatLong> _coordinates;

		public CountryGpsDictionary(IReadOnlyDictionary<Countries, LatLong> map)
		{
			_coordinates = map;
		}

		public LatLong this[Countries country] => _coordinates[country];
	}
}