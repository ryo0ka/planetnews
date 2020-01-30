using System;
using System.Collections.Generic;
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
			var map = new Dictionary<string, LatLong>();

			ReadCsv(lines.Split('\n'), separator, line =>
			{
				if (line[0] is string country &&
				    !string.IsNullOrEmpty(country) &&
				    float.TryParse(line[1], out var latitude) &&
				    float.TryParse(line[2], out var longitude))
				{
					map[country] = new LatLong(latitude, longitude);
				}
				else
				{
					Debug.LogWarning($"Skipped malformed line: '{string.Join(", ", line)}'");
				}
			});

			return new CountryGpsDictionary(map);
		}

		//TODO move to utility
		static void ReadCsv(IReadOnlyList<string> sourceLines, char separator, Action<IReadOnlyList<string>> f)
		{
			for (var i = 0; i < sourceLines.Count; i++)
			{
				var components = sourceLines[i].Split(separator);
				try
				{
					f(components);
				}
				catch (Exception)
				{
					Debug.LogError($"Exception caught at line {i}. Content: {string.Join(", ", components)}");
					throw;
				}
			}
		}
	}
}