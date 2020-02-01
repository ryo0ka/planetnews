using Planet.Models;
using TMPro;
using UniRx.Async;
using UnityEngine;

namespace Planet.Views
{
	public class EventHeadlineView : MonoBehaviour
	{
		[SerializeField]
		TMP_Text _sourceText;

		[SerializeField]
		TMP_Text _titleText;

		[SerializeField]
		ImageView _thumbnailView;

		string _currentThumbnailUrl;

		public async UniTask Load(IEvent ev)
		{
			gameObject.SetActive(true);

			_sourceText.text = ev.Source;
			_titleText.text = ev.Title;

			if (ev.ThumbnailUrl is string url)
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