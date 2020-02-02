using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Planet.Views
{
	public class LineCollectionView : MonoBehaviour
	{
		[SerializeField]
		Material _lineMaterial;

		[SerializeField]
		Gradient _lineColor;

		[SerializeField]
		float _fadeDuration;

		List<(Transform, Transform)> _connections;
		List<float> _lineColorTimes;

		void Awake()
		{
			_connections = new List<(Transform, Transform)>();
			_lineColorTimes = new List<float>();
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
			for (var i = 0; i < _lineColorTimes.Count; i++)
			{
				var time = _lineColorTimes[i];
				var fadeDeltaDuration = Time.deltaTime / _fadeDuration;
				var nextTime = time - fadeDeltaDuration;
				nextTime = Mathf.Max(0f, nextTime);
				_lineColorTimes[i] = nextTime;
			}
		}

		void OnMainCameraPostRender()
		{
			for (var i = 0; i < _connections.Count; i++)
			{
				var (point1, point2) = _connections[i];
				if (!point1 || !point2) continue;

				var position1 = point1.position;
				var position2 = point2.position;
				var colorTime = _lineColorTimes[i];
				var color = _lineColor.Evaluate(colorTime);
				RenderGlLine(position1, position2, _lineMaterial, color);
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

			// Expand color time list if necessary
			while (_lineColorTimes.Count < length)
			{
				_lineColorTimes.Add(0f);
			}
		}

		public void Connect(int index, Transform point1, Transform point2)
		{
			var (lastPoint1, lastPoint2) = _connections[index];
			if (lastPoint1 != point1 || lastPoint2 != point2)
			{
				_connections[index] = (point1, point2);
				_lineColorTimes[index] = 1f;
			}
		}

		public void Disconnect(int index)
		{
			_connections[index] = default;
		}

		static void RenderGlLine(Vector3 v1, Vector3 v2, Material mat, Color color)
		{
			mat.SetPass(0);
			GL.Begin(GL.LINES);
			GL.Color(color); // vertex color
			GL.Vertex3(v1.x, v1.y, v1.z);
			GL.Vertex3(v2.x, v2.y, v2.z);
			GL.End();
		}
	}
}