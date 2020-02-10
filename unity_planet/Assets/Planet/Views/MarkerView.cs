using DG.Tweening;
using UnityEngine;

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
		bool _isFocused, _isViewable;

		public void SetFocused(bool focused)
		{
			_isFocused = focused;
			UpdateView();
		}

		public void SetViewable(bool viewable)
		{
			_isViewable = viewable;
			UpdateView();
		}

		void UpdateView()
		{
			Color color;
			float scale;

			if (_isViewable)
			{
				color = _color.Evaluate(_isFocused ? 1f : 0f);
				scale = _isFocused ? 2f : 1f;
			}
			else
			{
				color = _color.Evaluate(0.5f);
				scale = 1f;
			}

			_renderer.material.DOColor(color, _colorId, _duration);
			transform.DOScale(scale, _duration).SetEase(Ease.OutBack);
		}
	}
}