using System.Collections.Generic;

namespace Planet.CountryCodeToGps
{
	public sealed class CountryGpsDictionary
	{
		readonly IReadOnlyDictionary<string, LatLong> _coordinates;

		public CountryGpsDictionary(IReadOnlyDictionary<string, LatLong> map)
		{
			_coordinates = map;
		}

		public LatLong this[string country] => _coordinates[country];
	}
}