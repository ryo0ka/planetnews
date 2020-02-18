using Sirenix.OdinInspector;
using UniRx.Async;
using UnityEngine;

namespace Planet.Views.Handles
{
	public class PlanetTransformHandle : MonoBehaviour, IHandle
	{
		[SerializeField]
		PlanetHandleView _view;

		[SerializeField]
		Collider _sphereCollider;

		[SerializeField]
		Transform _rotator;

		public Transform Planet => _rotator;

		public bool Test(Collider c)
		{
			return c == _sphereCollider;
		}

		public void SetTeasing(bool teasing)
		{
			if (teasing)
			{
				_view.StartHighlightingHalf();
			}
			else
			{
				_view.EndHighlightingHalf();
			}
		}

		public void SetHandling(bool handling)
		{
			if (handling)
			{
				_view.StartHighlighting();
			}
			else
			{
				_view.EndHighlighting();
			}
		}

		public void Blink()
		{
			_view.BlinkHighlight();
		}

		[Button]
		public async void ResetRotation()
		{
			var initRotation = _rotator.localRotation;

			const float Duration = 0.3f;
			var ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

			var startTime = Time.time;
			var done = false;
			while (!done)
			{
				var time = (Time.time - startTime) / Duration;
				done = time >= 1f;
				var t = Mathf.Clamp01(time);
				t = ease.Evaluate(t);
				var rot = Quaternion.Slerp(initRotation, Quaternion.identity, t);
				_rotator.localRotation = rot;

				await UniTask.Yield();
			}
		}
	}
}