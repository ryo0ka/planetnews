using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx.Async;
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
		public void StartHighlightingHalf()
		{
			DOSelectionNormal(0.5f);
		}

		[Button, DisableInEditorMode]
		public void EndHighlightingHalf()
		{
			DOSelectionNormal(0f);
		}

		[Button, DisableInEditorMode]
		public void StartHighlighting()
		{
			DOSelectionNormal(1f);
		}

		[Button, DisableInEditorMode]
		public void EndHighlighting()
		{
			DOSelectionNormal(0f);
		}

		[Button, DisableInEditorMode]
		public async void BlinkHighlight()
		{
			DOSelectionNormal(1f);
			await UniTask.Delay(TimeSpan.FromSeconds(_duration));
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