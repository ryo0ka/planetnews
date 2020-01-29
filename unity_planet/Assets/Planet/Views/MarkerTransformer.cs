using UnityEngine;

namespace Planet.Views
{
	public class MarkerTransformer : MonoBehaviour
	{
		[SerializeField]
		Transform _markerRoot;

		[SerializeField]
		Transform _marker;

		[SerializeField]
		float _latitude;

		[SerializeField]
		float _longitude;

		[SerializeField]
		float _heightOffset;

		const float PlanetRadius = 0.5f;

		void Reset()
		{
			_markerRoot = transform;
			_marker = _markerRoot.GetChild(0);
		}

		void OnValidate()
		{
			SetPosition(_latitude, _longitude);
		}

		void UpdatePosition()
		{
			_markerRoot.localEulerAngles = new Vector2(
				_latitude * -1f,
				_longitude * -1f);

			_marker.localPosition = new Vector3(
				0, 0, PlanetRadius + _heightOffset);
		}

		public void SetPosition(float latitude, float longitude)
		{
			_latitude = latitude;
			_longitude = longitude;
			UpdatePosition();
		}

		public void SetHeightOffset(float offset)
		{
			_heightOffset = offset;
			UpdatePosition();
		}
	}
}