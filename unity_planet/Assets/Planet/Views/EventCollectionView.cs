using Sirenix.OdinInspector;
using UnityEngine;

namespace Planet.Views
{
	public class EventCollectionView : MonoBehaviour
	{
		[SerializeField, DisableInPlayMode]
		EventHeadlineView[] _eventViews;

		public int Length => _eventViews.Length;
		public EventHeadlineView this[int i] => _eventViews[i];
	}
}