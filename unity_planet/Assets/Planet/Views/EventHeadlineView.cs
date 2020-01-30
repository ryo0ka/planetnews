using Planet.Models;
using TMPro;
using UniRx.Async;
using UnityEngine;

namespace Planet.Views
{
	public class EventHeadlineView : MonoBehaviour
	{
		[SerializeField]
		TMP_Text _titleText;

		[SerializeField]
		ImageView _thumbnailView;

		public async UniTask Load(IEventHeadline eventHeadline)
		{
			_titleText.text = eventHeadline.Title;

			if (eventHeadline.ThumbnailUrl is string url)
			{
				await _thumbnailView.LoadImage(url);
			}
			else
			{
				_thumbnailView.LoadDefaultImage();
			}
		}
	}
}