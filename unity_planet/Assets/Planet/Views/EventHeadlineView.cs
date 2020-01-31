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

		string _currentThumbnailUrl;

		public async UniTask Load(IEventHeadline eventHeadline)
		{
			gameObject.SetActive(true);

			_titleText.text = eventHeadline.Title;

			if (eventHeadline.ThumbnailUrl is string url)
			{
				if (url != _currentThumbnailUrl)
				{
					_currentThumbnailUrl = url;
					await _thumbnailView.LoadImage(url);
				}
			}
			else
			{
				_thumbnailView.LoadDefaultImage();
			}
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}