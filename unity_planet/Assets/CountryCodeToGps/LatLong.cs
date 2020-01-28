namespace CountryCodeToGps
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
	}
}