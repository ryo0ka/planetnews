using System.Collections.Generic;
using Planet.Utils;
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

		[SerializeField]
		AnimationCurve _curve;

		List<(Transform marker, Transform eventView)> _connections;
		List<LineViewState> _lineStates;
		Vector3[] _points;

		void Awake()
		{
			_connections = new List<(Transform, Transform)>();
			_lineStates = new List<LineViewState>();
			_points = new Vector3[3];
		}

		void Update()
		{
			foreach (var lineState in _lineStates)
			{
				lineState.Update();
			}

			for (var i = 0; i < _connections.Count; i++)
			{
				var (marker, eventView) = _connections[i];
				if (!marker || !eventView) continue;

				var position1 = marker.position;
				var position2 = eventView.position;
				var lineState = _lineStates[i];
				DrawLine(position1, position2, lineState);
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

		void DrawLine(Vector3 markerPosition, Vector3 panelPosition, LineViewState state)
		{
			var normalTime = Mathf.Clamp01(state.PastTime / _fadeDuration);
			normalTime = _curve.Evaluate(normalTime);

			var legVectorNormal = (markerPosition - _sphere.position).normalized;
			var legVector = (legVectorNormal * _legLength).Scaled(_sphere.lossyScale);
			var kneePosition = markerPosition + legVector;

			_points[0] = panelPosition;
			_points[1] = kneePosition;
			_points[2] = markerPosition;

			var color = _lineColor.Evaluate(normalTime);

			DrawLine(_lineMaterial, color, normalTime, _points);
		}

		static void DrawLine(Material mat, Color color, float normalLength, params Vector3[] points)
		{
			var totalLength = 0f;
			for (var i = 0; i < points.Length - 1; i++)
			{
				var p1 = points[i];
				var p2 = points[i + 1];

				var patialLength = (p2 - p1).magnitude;
				totalLength += patialLength;
			}

			var restLength = totalLength * normalLength;
			for (var i = 0; i < points.Length - 1; i++)
			{
				var p1 = points[i];
				var p2 = points[i + 1];

				var partialLength = (p2 - p1).magnitude;
				var targetLength = Mathf.Min(partialLength, restLength);
				restLength -= targetLength;

				var p2p = p1 + (p2 - p1).OfMagnitude(targetLength);

				DrawLine(mat, color, p1, p2p);
			}
		}

		static void DrawLine(Material mat, Color color, Vector3 p1, Vector3 p2)
		{
			// Editor scene view support
			Debug.DrawLine(p1, p2, color * mat.color);

			IMDraw.Line3D(p1, p2, color, 0f);
		}
	}
}