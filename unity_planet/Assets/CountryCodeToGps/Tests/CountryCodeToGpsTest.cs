using System;
using NewsAPI.Constants;
using NUnit.Framework;
using UnityEngine;

namespace CountryCodeToGps.Tests
{
	public class CountryCodeToGpsTest
	{
		[Test]
		public void TestCountryCodePresence()
		{
			const string CsvFilePath = "countries";
			var countryCodes = (Countries[]) Enum.GetValues(typeof(Countries));

			var file = Resources.Load<TextAsset>(CsvFilePath);
			Assert.IsNotNull(file, "Country CSV file not found in Resources");

			var text = file.text;
			Assert.IsNotEmpty(text, "Country CSV file empty");

			var reader = new CountryGpsCsv(text, ',');
			foreach (var countryCode in countryCodes)
			{
				var latLong = reader[countryCode];
				Assert.AreNotEqual(0f, latLong.Latitude, "Default value in latitude");
				Assert.AreNotEqual(0f, latLong.Longitude, "Default value in longitude");
			}
		}
	}
}