using Planet.Utils;
using UniRx;
using UnityEngine;

namespace Planet.Views
{
	public class PlanetTransformer : MonoBehaviour
	{
		[SerializeField]
		PlanetRealtimeRotator _realtimeRotator;

		[SerializeField]
		Collider _planetCollider;

		[SerializeField]
		Collider _leftControllerCollider;

		bool _canHold;

		Vector3 _leftControllerPositionOffset;
		Quaternion _leftControllerRotationOffset;

		void Start()
		{
			_planetCollider
				.OnTriggerStayingAsObservable(_leftControllerCollider)
				.Subscribe(staying => _canHold = staying)
				.AddTo(this);
		}

		void LateUpdate()
		{
			var leftController = _leftControllerCollider.transform;

			if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
			{
				_leftControllerPositionOffset = leftController.InverseTransformVector(transform.position - leftController.position);
				_leftControllerRotationOffset = Quaternion.Inverse(leftController.rotation) * transform.rotation;
			}

			if (_canHold && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
			{
				transform.position = leftController.position + leftController.TransformVector(_leftControllerPositionOffset);
				transform.rotation = leftController.rotation * _leftControllerRotationOffset;
				_realtimeRotator.CanRotate = false;
			}
			else
			{
				_realtimeRotator.CanRotate = true;
			}
		}
	}
}