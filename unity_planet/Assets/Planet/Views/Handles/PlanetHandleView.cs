using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Planet.Views.Handles
{
	/// <summary>
	/// Describes focus/selection state for user.
	/// </summary>
	public class PlanetHandleView : MonoBehaviour
	{
		[SerializeField]
		MeshRenderer[] _renderers;

		[SerializeField]
		Material _sourceMaterial; // template for _material

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
		public void StartSelected()
		{
			DOSelectionNormal(0.5f);
		}

		[Button, DisableInEditorMode]
		public void EndSelected()
		{
			DOSelectionNormal(0f);
		}

		[Button, DisableInEditorMode]
		public void StartGrabbed()
		{
			DOSelectionNormal(1f);
		}

		// should be called every frame
		public void StayGrab(float deltaAngle)
		{
			//TODO Implement
		}

		[Button, DisableInEditorMode]
		public void EndGrabbed()
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