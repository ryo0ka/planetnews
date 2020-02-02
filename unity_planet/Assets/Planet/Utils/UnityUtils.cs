using UnityEngine;

namespace Planet.Utils
{
	public static class UnityUtils
	{
		public static Vector3 Scaled(this Vector3 self, float x, float y, float z)
		{
			return new Vector3(self.x * x, self.y * y, self.z * z);
		}

		public static Vector3 Scaled(this Vector3 self, Vector3 by)
		{
			return self.Scaled(by.x, by.y, by.z);
		}

		public static Vector3 OfMagnitude(this Vector3 self, float magnitude)
		{
			return self.normalized * magnitude;
		}

		public static Vector3 MovedTo(this Vector3 self, Vector3 target, float distance)
		{
			return self + (target - self).OfMagnitude(distance);
		}

		public static void SetLocalEulerAngle(this Transform self, float? x = null, float? y = null, float? z = null)
		{
			var n = self.localEulerAngles;
			n.x = x ?? n.x;
			n.y = y ?? n.y;
			n.z = z ?? n.z;
			self.localEulerAngles = n;
		}
	}
}