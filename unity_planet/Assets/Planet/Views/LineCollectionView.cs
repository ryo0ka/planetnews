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
		Gradient _lineColor;

		[SerializeField]
		float _fadeDuration;

		[SerializeField]
		float _legLength;

		[SerializeField]
		AnimationCurve _curve;

		OrderedSet<(Transform marker, Transform panel)> _lines;
		List<LineViewState> _lineStates;
		HashSet<(Transform marker, Transform panel)> _newLines;
		List<int> _remover;
		Vector3[] _points;

		void Awake()
		{
			_lines = new OrderedSet<(Transform, Transform)>();
			_lineStates = new List<LineViewState>();
			_newLines = new HashSet<(Transform, Transform)>();
			_remover = new List<int>();
			_points = new Vector3[3];
		}

		void Update()
		{
			for (var i = 0; i < _lines.Count; i++)
			{
				var (p1, p2) = _lines[i];
				var lineState = _lineStates[i];
				lineState.Update();
				DrawLine(p1.position, p2.position, lineState);
			}
		}

		public void PrepareUpdateConnections()
		{
			_newLines.Clear();
		}

		public void Connect(Transform marker, Transform panel)
		{
			_newLines.Add((marker, panel));
		}

		public void UpdateConnections()
		{
			// add new connections
			foreach (var mp in _newLines)
			{
				if (!_lines.Contains(mp))
				{
					_lines.Add(mp);
					_lineStates.Add(new LineViewState());
				}
				else
				{
					//Debug.Log($"existing: {mp.Item1.name} {mp.Item2.name}");
				}
			}

			// find obsolete connections
			_remover.Clear();
			for (var i = 0; i < _lines.Count; i++)
			{
				var mp = _lines[i];
				if (!_newLines.Contains(mp))
				{
					_remover.Add(i);
				}
			}

			// remove obsolete connections
			_remover.Sort();
			for (var i = _remover.Count - 1; i >= 0; i--)
			{
				var removedIndex = _remover[i];
				_lines.RemoveAt(removedIndex);
				_lineStates.RemoveAt(removedIndex);
			}
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

			UnityUtils.DrawLine(color, normalTime, _points);
			Debug.DrawLine(markerPosition, panelPosition, Color.cyan);
		}
	}
}