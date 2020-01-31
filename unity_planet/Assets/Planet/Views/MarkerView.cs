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

		public Vector3 WorldPosition => _renderer.transform.position;

		public void SetPosition(float latitude, float longitude)
		{
			_transformer.SetPosition(latitude, longitude);
		}

		public void SetFocused(bool focused)
		{
			var color = _color.Evaluate(focused ? 1 : 0);
			_renderer.material.SetColor(ColorId, color);
		}
	}
}