using UnityEngine;

namespace Planet.Utils
{
	public static class MathUtils
	{
		public static Quaternion GpsToSpherical(float latitude, float longitude)
		{
			return Quaternion.Euler(new Vector2(latitude * -1f, longitude * -1f));
		}

		public static Vector3 NormalizedEulerAngles(this Vector3 self)
		{
			return new Vector3(
				NormalizeEulerAngle(self.x),
				NormalizeEulerAngle(self.y),
				NormalizeEulerAngle(self.z));
		}

		public static float NormalizeEulerAngle(float angle)
		{
			while (angle > +180) angle -= 360;
			while (angle < -180) angle += 360;
			return angle;
		}
	}
}