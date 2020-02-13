using Sirenix.OdinInspector;
using UnityEngine;

namespace Planet.Views
{
	public class HeadlinePanelCollectionView : MonoBehaviour
	{
		[SerializeField, DisableInPlayMode]
		HeadlinePanelView[] _panels;

		public int Count => _panels.Length;
		public HeadlinePanelView this[int i] => _panels[i];
	}
}