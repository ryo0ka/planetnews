using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Planet.Views
{
	public class RotationHandleView : MonoBehaviour
	{
		[SerializeField]
		MeshRenderer _renderer;

		[SerializeField]
		float _duration;

		readonly int AlphaNormalId = Shader.PropertyToID("_AlphaNormal");

		void Start()
		{
			_renderer.material.SetFloat(AlphaNormalId, 0f);
		}

		[Button, DisableInEditorMode]
		public void StartPing()
		{
			_renderer.material.DOFloat(.5f, AlphaNormalId, _duration);
		}

		[Button, DisableInEditorMode]
		public void EndPing()
		{
			_renderer.material.DOFloat(0f, AlphaNormalId, _duration);
		}

		[Button, DisableInEditorMode]
		public void StartGrab()
		{
			_renderer.material.DOFloat(1f, AlphaNormalId, _duration);
		}

		// should be called every frame
		public void StayGrab(float deltaAngle)
		{
			//TODO Implement
		}

		[Button, DisableInEditorMode]
		public void EndGrab()
		{
			_renderer.material.DOFloat(0f, AlphaNormalId, _duration);
		}
	}
}