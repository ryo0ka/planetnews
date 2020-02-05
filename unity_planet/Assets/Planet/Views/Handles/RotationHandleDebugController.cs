using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Planet.Views.Handles
{
	public class RotationHandleDebugController :
		MonoBehaviour,
		IRotationHandleController,
		IBeginDragHandler,
		IDragHandler,
		IEndDragHandler
	{
		[SerializeField]
		Collider _collider;

		RotationHandle _handle;
		bool _canGrab;
		bool _isGrabbing;
		PointerEventData _lastPointer;

		void Awake()
		{
			// only show up in editor
			gameObject.SetActive(Application.isEditor);
		}

		void Start()
		{
			_collider
				.OnTriggerEnterAsObservable()
				.Where(c => _handle.IsPingCollider(c))
				.Subscribe(_ => OnHandleTriggerEnter())
				.AddTo(this);

			_collider
				.OnTriggerExitAsObservable()
				.Where(c => _handle.IsPingCollider(c))
				.Subscribe(_ => OnHandleTriggetExit())
				.AddTo(this);
		}

		void Update()
		{
			IMDraw.Line3D(transform.position, _handle.transform.position, Color.green, 0f);
		}

		void OnHandleTriggerEnter()
		{
			_canGrab = true;
			_handle.StartPing();
		}

		void OnHandleTriggetExit()
		{
			_canGrab = false;
			_handle.EndPing();
		}

		void IRotationHandleController.TrackRotationHandle(RotationHandle handle)
		{
			_handle = handle;
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			_lastPointer = eventData;

			if (_canGrab)
			{
				_isGrabbing = true;
				_handle.StartGrabSpatial(() => GetPositionOnPlane(_lastPointer));
			}
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			_lastPointer = eventData;

			var pos = GetPositionOnPlane(eventData);
			transform.position = pos;
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			_lastPointer = eventData;

			if (_isGrabbing)
			{
				_isGrabbing = false;
				_handle.EndGrabSpatial();
			}
		}

		Vector3 GetPositionOnPlane(PointerEventData pointer)
		{
			var cam = pointer.pressEventCamera;
			var ray = cam.ScreenPointToRay(pointer.position);
			var plane = new Plane(Vector3.up, transform.position);
			plane.Raycast(ray, out var distance);
			return ray.GetPoint(distance);
		}
	}
}