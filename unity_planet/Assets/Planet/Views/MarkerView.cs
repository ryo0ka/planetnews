using DG.Tweening;
using UnityEngine;
using UnityEngine.Profiling;

namespace Planet.Views
{
	public class MarkerView : MonoBehaviour
	{
		[SerializeField]
		MeshRenderer _renderer;

		[SerializeField]
		Gradient _color;

		[SerializeField]
		float _duration;

		readonly int _colorId = Shader.PropertyToID("_Color");
		bool? _isHighlighted, _isViewable;

		public void SetHighlighted(bool highlighted)
		{
			if (highlighted == _isHighlighted) return;
			_isHighlighted = highlighted;
			UpdateView();
		}

		public void SetViewable(bool viewable)
		{
			if (viewable == _isViewable) return;
			_isViewable = viewable;
			UpdateView();
		}

		void UpdateView()
		{
			Profiler.BeginSample("MarkerView.UpdateView()");

			Color color;
			float scale;

			if (_isViewable ?? false)
			{
				var highlighted = _isHighlighted ?? false;
				color = _color.Evaluate(highlighted ? 1f : 0f);
				scale = highlighted ? 2f : 1f;
			}
			else
			{
				color = _color.Evaluate(0.5f);
				scale = 1f;
			}

			_renderer.material.DOColor(color, _colorId, _duration);
			transform.DOScale(scale, _duration).SetEase(Ease.OutBack);

			Profiler.EndSample();
		}
	}
}