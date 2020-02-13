using System;
using UnityEngine;

namespace Planet.Views
{
	public class PlanetRealtimeRotator : MonoBehaviour
	{
		[SerializeField]
		float _spinSpeedScale;

		const float HourPerDay = 24;
		const float DegPerDay = -360;
		const float DegPerSec = DegPerDay / (HourPerDay * 360);

		void Update()
		{
			var degPerSec = DegPerSec * _spinSpeedScale;
			transform.Rotate(Vector3.up, Time.deltaTime * degPerSec, Space.Self);
		}
	}
}