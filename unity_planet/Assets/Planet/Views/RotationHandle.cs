using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Planet.Views
{
	public class RotationHandle : MonoBehaviour
	{
		[SerializeField]
		Collider _collider;

		[SerializeField]
		RotationHandleView _view;

		[SerializeField]
		Transform _target;

		Func<Vector3> _positionGenerator;
		Vector3 _startGrabPosition;
		bool _isGrabbing;
		float _lastAngleSinceStart;

		//TODO not sure about this ;(
		[Inject]
		public void Inject(IEnumerable<IRotationHandleController> controllers)
		{
			foreach (var controller in controllers)
			{
				Debug.Log(controller);
				controller.TrackRotationHandle(this);
			}
		}

		void Update()
		{
			if (_isGrabbing)
			{
				var worldGrabPosition = _positionGenerator();
				var grabPosition = transform.InverseTransformVector(worldGrabPosition);
				var angleSinceStart = SignedAngleInXZ(_startGrabPosition, grabPosition);
				var deltaAngle = angleSinceStart - _lastAngleSinceStart;

				_view.StayGrab(deltaAngle);
				_target.Rotate(0, deltaAngle, 0, Space.Self);

				_lastAngleSinceStart = angleSinceStart;
			}
		}

		public bool IsPingCollider(Collider other)
		{
			return other == _collider;
		}

		public void StartPing()
		{
			if (!_isGrabbing)
			{
				_view.StartPing();
			}
		}

		public void EndPing()
		{
			if (!_isGrabbing)
			{
				_view.EndPing();
			}
		}

		public void StartGrabSpatial(Func<Vector3> positionGenerator)
		{
			_isGrabbing = true;
			_positionGenerator = positionGenerator;
			var worldGrabPosition = positionGenerator();
			_startGrabPosition = transform.InverseTransformVector(worldGrabPosition);
			_lastAngleSinceStart = 0;
			_view.StartGrab();
		}

		public void EndGrabSpatial()
		{
			_isGrabbing = false;
			_positionGenerator = null;
			_view.EndGrab();
		}

		static float SignedAngleInXZ(Vector3 from, Vector3 to)
		{
			return Vector2.SignedAngle(DropY(from), DropY(to));
		}

		static Vector2 DropY(Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}
	}
}