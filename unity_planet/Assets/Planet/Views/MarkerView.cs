using UnityEngine;

namespace Planet.Views
{
	public class MarkerView : MonoBehaviour
	{
		[SerializeField]
		MarkerTransformer _transformer;

		[SerializeField]
		MeshRenderer _renderer;

		[SerializeField]
		Gradient _color;

		readonly int ColorId = Shader.PropertyToID("_Color");
		bool _isFocused, _isViewable;

		public Vector3 WorldPosition => _renderer.transform.position;

		public void SetPosition(float latitude, float longitude)
		{
			_transformer.SetPosition(latitude, longitude);
		}

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

			if (_isViewable)
			{
				color = _color.Evaluate(_isFocused ? 1f : 0f);
			}
			else
			{
				color = _color.Evaluate(0.5f);
			}

			_renderer.material.SetColor(ColorId, color);
		}
	}
}