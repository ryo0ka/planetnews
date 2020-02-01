using UnityEngine;

namespace Planet.Utils
{
	public static class PlanetMath
	{
		public static Quaternion GpsToSpherical(float latitude, float longitude)
		{
			return Quaternion.Euler(new Vector2(latitude * -1f, longitude * -1f));
		}
	}
}