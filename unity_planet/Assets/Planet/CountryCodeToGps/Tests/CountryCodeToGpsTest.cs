using System;
using NewsAPI.Constants;
using NUnit.Framework;

namespace Planet.CountryCodeToGps.Tests
{
	public class CountryCodeToGpsTest
	{
		[Test]
		public void CountryCodePresence()
		{
			var dictionary = CountryGpsFactory.FromResource();
			var countryCodes = (NewsApiCountry[]) Enum.GetValues(typeof(NewsApiCountry));
			foreach (var countryCode in countryCodes)
			{
				var latLong = dictionary[countryCode.ToString()];
				Assert.AreNotEqual(0f, latLong.Latitude, "Default value in latitude");
				Assert.AreNotEqual(0f, latLong.Longitude, "Default value in longitude");
			}
		}
	}
}