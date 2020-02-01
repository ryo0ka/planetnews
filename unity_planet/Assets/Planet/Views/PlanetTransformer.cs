using Planet.Utils;
using UniRx;
using UnityEngine;

namespace Planet.Views
{
	public class PlanetTransformer : MonoBehaviour
	{
		[SerializeField]
		Transform _planet;

		[SerializeField]
		Collider _planetCollider;

		[SerializeField]
		Collider _leftControllerCollider;

		[SerializeField]
		float _spinSpeed;

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
				_leftControllerPositionOffset = leftController.InverseTransformVector(_planet.position - leftController.position);
				_leftControllerRotationOffset = Quaternion.Inverse(leftController.rotation) * _planet.rotation;
			}

			if (_canHold && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
			{
				_planet.position = leftController.position + leftController.TransformVector(_leftControllerPositionOffset);
				_planet.rotation = leftController.rotation * _leftControllerRotationOffset;
			}
			else
			{
				_planet.Rotate(Vector3.up, Time.deltaTime * _spinSpeed, Space.Self);
			}
		}
	}
}