using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Planet.Utils;
using UnityEngine;

namespace Planet.Views
{
	public class MarkerTransformer : MonoBehaviour
	{
		[SerializeField]
		Transform _markerRoot;

		[SerializeField]
		Transform _mover;

		[SerializeField]
		Transform _scaler;

		[SerializeField]
		float _latitude;

		[SerializeField]
		float _longitude;

		[SerializeField]
		float _heightOffset;

		const float PlanetRadius = 0.5f;

		public float Scale => _scaler.localScale.x;
		public Transform Anchor => _scaler;

		void Reset()
		{
			_markerRoot = transform;
			_mover = _markerRoot.GetChild(0);
		}

		void OnValidate()
		{
			SetPosition(_latitude, _longitude);
		}

		void UpdatePosition()
		{
			_markerRoot.localRotation = PlanetMath.GpsToSpherical(_latitude, _longitude);
			_mover.localPosition = new Vector3(
				0, 0, PlanetRadius + _heightOffset);
		}

		public void SetPosition(float latitude, float longitude)
		{
			_latitude = latitude;
			_longitude = longitude;

			UpdatePosition();
		}

		public void SetScale(float scale)
		{
			_scaler.localScale = Vector3.one * scale;
		}
	}

	static class MarkerTransformerUtils
	{
		public static TweenerCore<float, float, FloatOptions> DoScale(this MarkerTransformer self, float scale, float duration)
		{
			return DOTween.To(() => self.Scale, s => self.SetScale(s), scale, duration);
		}
	}
}