using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Planet.Views
{
	public class OculusTouchController : MonoBehaviour, IRotationHandleController
	{
		class RotationHandleController
		{
			public RotationHandleController(RotationHandle handle)
			{
				Handle = handle;
			}

			RotationHandle Handle { get; }
			bool CanGrab { get; set; }
			bool IsGrabbing { get; set; }

			public bool IsPingCollider(Collider other)
			{
				return Handle.IsPingCollider(other);
			}

			public void StartPing()
			{
				CanGrab = true;
				Handle.StartPing();
			}

			public void EndPing()
			{
				CanGrab = false;
				Handle.EndPing();
			}

			public void TryGrab(Func<Vector3> position)
			{
				if (CanGrab)
				{
					IsGrabbing = true;
					Handle.StartGrabSpatial(position);
				}
			}

			public void EndGrab()
			{
				if (IsGrabbing)
				{
					IsGrabbing = false;
					Handle.EndGrabSpatial();
				}
			}
		}

		[SerializeField]
		Collider _collider;

		List<RotationHandleController> _rotationHandles;

		public void TrackRotationHandle(RotationHandle handle)
		{
			// initialize
			if (_rotationHandles == null)
			{
				_rotationHandles = new List<RotationHandleController>();
			}

			_rotationHandles.Add(new RotationHandleController(handle));
		}

		void Start()
		{
			_collider
				.OnTriggerEnterAsObservable()
				.SelectMany(c => _rotationHandles.Where(h => h.IsPingCollider(c)))
				.Subscribe(h => h.StartPing())
				.AddTo(this);

			_collider
				.OnTriggerExitAsObservable()
				.SelectMany(c => _rotationHandles.Where(h => h.IsPingCollider(c)))
				.Subscribe(h => h.EndPing())
				.AddTo(this);
		}

		void LateUpdate()
		{
			var button = OVRInput.Button.PrimaryIndexTrigger;
			var mask = OVRInput.Controller.LTouch;

			if (OVRInput.GetDown(button, mask))
			{
				foreach (var rotationHandle in _rotationHandles)
				{
					rotationHandle.TryGrab(() => _collider.transform.position);
				}
			}

			if (OVRInput.GetUp(button, mask))
			{
				foreach (var rotationHandle in _rotationHandles)
				{
					rotationHandle.EndGrab();
				}
			}
		}
	}
}