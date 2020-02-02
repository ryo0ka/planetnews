using UnityEngine;

namespace Planet.Utils
{
	public static class UnityUtils
	{
		public static Vector3 MultipliedBy(this Vector3 self, float x, float y, float z)
		{
			return new Vector3(self.x * x, self.y * y, self.z * z);
		}

		public static Vector3 MultipliedBy(this Vector3 self, Vector3 by)
		{
			return self.MultipliedBy(by.x, by.y, by.z);
		}
	}
}