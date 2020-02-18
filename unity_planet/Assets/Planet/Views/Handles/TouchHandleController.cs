using JetBrains.Annotations;
using Planet.Rider;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Planet.Views.Handles
{
	public sealed class TouchHandleController : MonoBehaviour,
		IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
	{
		[SerializeField]
		float _viewportToAngle;

		Camera _mainCamera;
		IHandleRepository _handleRepository;
		bool _dragging;
		Vector2 _dragInitPosition;
		PlanetTransformHandle _planetHandle;
		Quaternion _initPlanetRotation;
		Quaternion _initPlanetLocalRotation;
		Vector3 _initPlanetDirection;

		[Inject, UsedImplicitly]
		public void Inject(IHandleRepository handleRepository)
		{
			_handleRepository = handleRepository;
		}

		void Start()
		{
			_mainCamera = Camera.main;
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			var pointerPosition = eventData.position;
			if (TestPlanetTransformHandle(pointerPosition, out _planetHandle))
			{
				_dragging = true;
				_dragInitPosition = pointerPosition;
				_planetHandle.SetHandling(true);
				_initPlanetRotation = _planetHandle.Planet.rotation;
				_initPlanetLocalRotation = _planetHandle.Planet.localRotation;

				var dir = Quaternion.Euler(0, 0, 0) * Vector3.forward;
				var worldDir = _mainCamera.transform.rotation * dir;
				var localDir = Quaternion.Inverse(_initPlanetRotation) * worldDir;
				_initPlanetDirection = localDir;
			}
		}

		[FrequentlyCalledMethod]
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (!_dragging) return;

			var pointerPosition = (Vector2) eventData.position;
			var displacement = pointerPosition - _dragInitPosition;
			var screen = new Vector2(1f / Screen.width, 1f / Screen.height);
			var angles = Vector2.Scale(displacement, screen) * _viewportToAngle;

			var dir = Quaternion.Euler(angles.y, angles.x * -1f, 0) * Vector3.forward;
			var worldDir = _mainCamera.transform.rotation * dir;
			var localDir = Quaternion.Inverse(_initPlanetRotation) * worldDir;
			var rot = Quaternion.FromToRotation(_initPlanetDirection, localDir);

			_planetHandle.Planet.localRotation = _initPlanetLocalRotation * rot;
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			_dragging = false;
			_planetHandle?.SetHandling(false);
			_planetHandle = null;
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (_dragging) return;

			if (TestPlanetTransformHandle(eventData.pressPosition, out _planetHandle))
			{
				_planetHandle.Blink();
				_planetHandle.ResetRotation();
			}
		}

		bool TestPlanetTransformHandle(Vector3 pointerPosition, out PlanetTransformHandle handle)
		{
			var ray = _mainCamera.ScreenPointToRay(pointerPosition);
			if (_handleRepository.Test(ray, out var h) &&
			    h is PlanetTransformHandle planetHandle)
			{
				handle = planetHandle;
				return true;
			}

			handle = null;
			return false;
		}
	}
}