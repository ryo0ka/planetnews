using UnityEngine;

namespace Planet.Views
{
	public class PlanetRealtimeRotator : MonoBehaviour
	{
		[SerializeField]
		float _spinSpeed;

		public bool CanRotate { private get; set; }

		void Update()
		{
			if (CanRotate)
			{
				transform.Rotate(Vector3.up, Time.deltaTime * _spinSpeed, Space.Self);
			}
		}
	}
}