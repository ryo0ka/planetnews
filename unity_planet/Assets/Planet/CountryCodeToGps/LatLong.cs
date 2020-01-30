namespace Planet.CountryCodeToGps
{
	public struct LatLong
	{
		public readonly float Latitude;
		public readonly float Longitude;

		public LatLong(float latitude, float longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		public override string ToString()
		{
			return $"{nameof(Latitude)}: {Latitude}, {nameof(Longitude)}: {Longitude}";
		}
	}
}