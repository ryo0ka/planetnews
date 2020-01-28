using System;
using System.Collections.Generic;
using NewsAPI.Constants;
using UnityEngine;

namespace CountryCodeToGps
{
	public sealed class CountryGpsCsv
	{
		readonly IDictionary<Countries, LatLong> _coordinates;

		public CountryGpsCsv(string lines, char separator)
		{
			_coordinates = new Dictionary<Countries, LatLong>();

			ReadCsv(lines.Split('\n'), separator, line =>
			{
				if (Enum.TryParse(line[0], true, out Countries country))
				{
					var latitude = float.Parse(line[1]);
					var longitude = float.Parse(line[2]);
					_coordinates[country] = new LatLong(latitude, longitude);
				}
			});
		}

		public LatLong this[Countries country] => _coordinates[country];

		static void ReadCsv(IEnumerable<string> sourceLines, char separator, Action<IReadOnlyList<string>> f)
		{
			foreach (var sourceLine in sourceLines)
			{
				var line = sourceLine.Split(separator);
				f(line);
			}
		}
	}
}