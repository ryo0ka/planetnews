using System;
using System.Collections.Generic;
using NewsAPI.Constants;
using UnityEngine;

namespace Planet.CountryCodeToGps
{
	public static class CountryGpsFactory
	{
		public static CountryGpsDictionary FromResource()
		{
			const string CsvFilePath = "countries";
			var file = Resources.Load<TextAsset>(CsvFilePath);
			return FromCsv(file.text, ',');
		}

		static CountryGpsDictionary FromCsv(string lines, char separator)
		{
			var map = new Dictionary<Countries, LatLong>();

			ReadCsv(lines.Split('\n'), separator, line =>
			{
				if (Enum.TryParse(line[0], true, out Countries country))
				{
					var latitude = float.Parse(line[1]);
					var longitude = float.Parse(line[2]);
					map[country] = new LatLong(latitude, longitude);
				}
			});

			return new CountryGpsDictionary(map);
		}

		//TODO move to utility
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