using UnityEngine;

namespace Planet.Utils
{
	public static class UnityUtils
	{
		public static Vector3 Multiply(this Vector3 self, float x, float y, float z)
		{
			return new Vector3(self.x * x, self.y * y, self.z * z);
		}
	}
}