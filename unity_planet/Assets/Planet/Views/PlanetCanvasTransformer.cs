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
		RectTransform _leftPanel;

		[SerializeField]
		RectTransform _leftPanelRoot;

		[SerializeField]
		RectTransform _rightPanel;

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

			UpdatePanelTransform(true, _leftPanelRoot, _leftPanel);
			UpdatePanelTransform(false, _rightPanelRoot, _rightPanel);
		}

		void UpdatePanelTransform(bool left, RectTransform panelRoot, RectTransform panel)
		{
			// Calculate margin between the panel & planet
			var planetScale = _planetScaler.lossyScale.x;
			var canvasScale = _planetCanvas.transform.lossyScale.x;
			var marginsSize = planetScale / canvasScale / 2;

			// Calculate the angle that keeps the panel against the camera
			var h = marginsSize + panel.sizeDelta.x / 2;
			var p = new Vector2(_mainCameraTransform.position.x, _mainCameraTransform.position.z);
			var o = new Vector2(_planetScaler.position.x, _planetScaler.position.z);
			var op = (p - o).magnitude / canvasScale;
			var np = Mathf.Sqrt(op * op - h * h);
			var od = Mathf.Atan(np / h) * Mathf.Rad2Deg;

			// Apply rotation
			panelRoot.LookAt(_mainCameraTransform.position, Vector3.up);
			panelRoot.Rotate(0, 180, 0, Space.Self);
			panelRoot.Rotate(0, left ? od - 90 : 90 - od, 0, Space.Self);

			// Apply margin
			var sizeDelta = panelRoot.sizeDelta;
			sizeDelta.x = marginsSize;
			panelRoot.sizeDelta = sizeDelta;
		}
	}
}