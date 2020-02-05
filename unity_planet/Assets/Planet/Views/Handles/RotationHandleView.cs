using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Planet.Views.Handles
{
	public class RotationHandleView : MonoBehaviour
	{
		[SerializeField]
		MeshRenderer[] _renderers;

		[SerializeField]
		Material _sourceMaterial;

		[SerializeField]
		Material _planetMaterial;

		[SerializeField]
		float _duration;

		Material _material;
		readonly int SelectionNormalId = Shader.PropertyToID("_SelectionNormal");

		void Start()
		{
			_material = Instantiate(_sourceMaterial);
			foreach (var meshRenderer in _renderers)
			{
				meshRenderer.material = _material;
			}

			SetSelectionNormal(0f);
		}

		[Button, DisableInEditorMode]
		public void StartPing()
		{
			DOSelectionNormal(0.5f);
		}

		[Button, DisableInEditorMode]
		public void EndPing()
		{
			DOSelectionNormal(0f);
		}

		[Button, DisableInEditorMode]
		public void StartGrab()
		{
			DOSelectionNormal(1f);
		}

		// should be called every frame
		public void StayGrab(float deltaAngle)
		{
			//TODO Implement
		}

		[Button, DisableInEditorMode]
		public void EndGrab()
		{
			DOSelectionNormal(0f);
		}

		void SetSelectionNormal(float normal)
		{
			_material.SetFloat(SelectionNormalId, normal);
			_planetMaterial.SetFloat(SelectionNormalId, normal);
		}

		void DOSelectionNormal(float toNormal)
		{
			_material.DOFloat(toNormal, SelectionNormalId, _duration);
			_planetMaterial.DOFloat(toNormal, SelectionNormalId, _duration);
		}
	}
}