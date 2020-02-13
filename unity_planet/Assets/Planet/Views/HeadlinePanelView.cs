using DG.Tweening;
using Planet.Models;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Planet.Views
{
	public class HeadlinePanelView : MonoBehaviour
	{
		[SerializeField]
		Text _sourceText;

		[SerializeField]
		Text _titleText;

		[SerializeField]
		ImageView _thumbnailView;

		[SerializeField]
		float _duration;

		string _currentThumbnailUrl;
		readonly int _fadeId = Shader.PropertyToID("_Fade");

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
					RunUnfadeEffect();
				}
			}
			else
			{
				_thumbnailView.LoadDefaultImage();
				RunUnfadeEffect();
			}
		}

		void RunUnfadeEffect()
		{
			if (!_thumbnailView) return;

			_thumbnailView.ImageMaterial.SetFloat(_fadeId, 1f);
			_thumbnailView.ImageMaterial.DOFloat(0f, _fadeId, _duration).SetEase(Ease.OutSine);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}