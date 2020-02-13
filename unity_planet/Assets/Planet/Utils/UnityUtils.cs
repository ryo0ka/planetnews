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

		public static void DrawLine(Color color, float normalLength, params Vector3[] points)
		{
			var totalLength = 0f;
			for (var i = 0; i < points.Length - 1; i++)
			{
				var p1 = points[i];
				var p2 = points[i + 1];

				var patialLength = (p2 - p1).magnitude;
				totalLength += patialLength;
			}

			var restLength = totalLength * normalLength;
			for (var i = 0; i < points.Length - 1; i++)
			{
				var p1 = points[i];
				var p2 = points[i + 1];

				var partialLength = (p2 - p1).magnitude;
				var targetLength = Mathf.Min(partialLength, restLength);
				restLength -= targetLength;

				var p2p = p1 + (p2 - p1).OfMagnitude(targetLength);

				DrawLine(color, p1, p2p);
			}
		}

		public static void DrawLine(Color color, Vector3 p1, Vector3 p2)
		{
			IMDraw.Line3D(p1, p2, color, 0f);
		}
	}
}