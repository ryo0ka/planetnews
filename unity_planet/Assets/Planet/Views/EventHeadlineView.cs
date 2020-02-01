using Planet.Models;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Planet.Views
{
	public class EventHeadlineView : MonoBehaviour
	{
		[SerializeField]
		Text _sourceText;

		[SerializeField]
		Text _titleText;

		[SerializeField]
		ImageView _thumbnailView;

		string _currentThumbnailUrl;

		public async UniTask Load(IEvent ev)
		{
			gameObject.SetActive(true);

			var date = ev.PublishedDate == null ? "" : $"{ev.PublishedDate:MM/dd/yyyy}";
			_sourceText.text = $"{ev.Source} {date}";
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