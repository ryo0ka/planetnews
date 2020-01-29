using UnityEngine;

namespace Planet.Views
{
	public class PlanetTransformer : MonoBehaviour
	{
		[SerializeField]
		Transform _planet;

		[SerializeField]
		float _spinSpeed;

		void Update()
		{
			_planet.Rotate(Vector3.up, Time.deltaTime * _spinSpeed, Space.Self);
		}
	}
}