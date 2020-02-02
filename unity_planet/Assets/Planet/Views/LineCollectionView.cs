using System.Collections.Generic;
using Planet.Utils;
using UniRx;
using UnityEngine;

namespace Planet.Views
{
	public class LineCollectionView : MonoBehaviour
	{
		[SerializeField]
		Transform _sphere;

		[SerializeField]
		Material _lineMaterial;

		[SerializeField]
		Gradient _lineColor;

		[SerializeField]
		float _fadeDuration;

		[SerializeField]
		float _legLength;

		List<(Transform marker, Transform eventView)> _connections;
		List<LineViewState> _lineStates;

		void Awake()
		{
			_connections = new List<(Transform, Transform)>();
			_lineStates = new List<LineViewState>();
		}

		void Start()
		{
			var mainCamera = Camera.main;
			mainCamera
				.OnPostRenderAsObservable()
				.Subscribe(_ => OnMainCameraPostRender())
				.AddTo(this);
		}

		void Update()
		{
			foreach (var lineState in _lineStates)
			{
				lineState.Update();
			}
		}

		void OnMainCameraPostRender()
		{
			for (var i = 0; i < _connections.Count; i++)
			{
				var (marker, eventView) = _connections[i];
				if (!marker || !eventView) continue;

				var position1 = marker.position;
				var position2 = eventView.position;
				var lineState = _lineStates[i];
				RenderLine(position1, position2, lineState);
			}
		}

		public void SetLength(int length)
		{
			var originalLength = _connections.Count;
			if (originalLength >= length)
			{
				// Trim end
				_connections.RemoveRange(length, originalLength - length);
			}
			else
			{
				// Add until the length
				while (_connections.Count < length)
				{
					_connections.Add(default);
				}
			}

			// Expand state list if necessary
			while (_lineStates.Count < length)
			{
				_lineStates.Add(new LineViewState());
			}
		}

		public void Connect(int index, Transform marker, Transform eventView)
		{
			var (lastMarker, lastEventView) = _connections[index];
			if (lastMarker != marker || lastEventView != eventView)
			{
				_connections[index] = (marker, eventView);
				_lineStates[index].Initialize();
			}
		}

		public void Disconnect(int index)
		{
			_connections[index] = default;
		}

		void RenderLine(Vector3 markerPosition, Vector3 eventViewPosition, LineViewState lineState)
		{
			var colorTime = lineState.PastTime / _fadeDuration;
			var color = _lineColor.Evaluate(Mathf.Clamp01(colorTime));

			var legVectorNormal = (markerPosition - _sphere.position).normalized;
			var legVector = (legVectorNormal * _legLength).MultipliedBy(_sphere.lossyScale);
			var kneePosition = markerPosition + legVector;
		
			DrawGlLine(markerPosition, kneePosition, _lineMaterial, color);
			DrawGlLine(kneePosition, eventViewPosition, _lineMaterial, color);
		}

		static void DrawGlLine(Vector3 p1, Vector3 p2, Material mat, Color color)
		{
			mat.SetPass(0);
			GL.Begin(GL.LINES);
			GL.Color(color); // vertex color
			GL.Vertex3(p1.x, p1.y, p1.z);
			GL.Vertex3(p2.x, p2.y, p2.z);
			GL.End();
		}
	}
}