using Planet.Utils;
using UnityEngine;

namespace Planet.Views
{
	[ExecuteInEditMode]
	public class PlanetCanvasTransformer : MonoBehaviour
	{
		[SerializeField]
		Transform _canvasRoot;

		[SerializeField]
		Transform _planetScaler;

		[SerializeField]
		Canvas _planetCanvas;

		[SerializeField]
		RectTransform _leftPanelCenter;

		[SerializeField]
		RectTransform _leftPanelRoot;

		[SerializeField]
		RectTransform _rightPanelCenter;

		[SerializeField]
		RectTransform _rightPanelRoot;

		Transform _mainCameraTransform;

		void Start()
		{
			_mainCameraTransform = Camera.main.transform;
			_planetCanvas.worldCamera = Camera.main;
		}

		void LateUpdate()
		{
			_canvasRoot.position = _planetScaler.position;

			UpdatePanelTransform(true, _leftPanelRoot, _leftPanelCenter);
			UpdatePanelTransform(false, _rightPanelRoot, _rightPanelCenter);
		}

		void UpdatePanelTransform(bool left, RectTransform panelRoot, RectTransform panelCenter)
		{
			// Calculate margin between the panel & planet
			var planetScale = _planetScaler.lossyScale.x;
			var canvasScale = _planetCanvas.transform.lossyScale.x;
			var marginSize = planetScale / canvasScale / 2;

			// Calculate the angle that keeps the panel against the camera
			var p = _mainCameraTransform.position;
			var o = _planetScaler.position;
			var op = (p - o).MultipliedBy(1, 0, 1).magnitude / canvasScale;
			var h = (panelCenter.position - o).MultipliedBy(1, 0, 1).magnitude / canvasScale;
			var np = Mathf.Sqrt(op * op - h * h);
			var od = Mathf.Atan(np / h) * Mathf.Rad2Deg;
			var odlr = left ? od - 90 : 90 - od;
			odlr = float.IsNaN(odlr) ? 0 : odlr;

			//Debug.Log($"{marginSize: 0.00} {panel.sizeDelta.x: 0.00}");
			Debug.DrawLine(o, panelCenter.position, Color.white);

			// Apply rotation
			panelRoot.LookAt(_mainCameraTransform.position, Vector3.up);
			panelRoot.Rotate(0, 180, 0, Space.Self);
			panelRoot.Rotate(0, odlr, 0, Space.World);

			// Apply margin
			var sizeDelta = panelRoot.sizeDelta;
			sizeDelta.x = marginSize;
			panelRoot.sizeDelta = sizeDelta;
		}
	}
}