using DG.Tweening;
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
		readonly int _fadeId = Shader.PropertyToID("_Fade");

		float Fade
		{
			get => _thumbnailView.ImageMaterial.GetFloat(_fadeId);
			set => _thumbnailView.ImageMaterial.SetFloat(_fadeId, value);
		}

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
			_thumbnailView.ImageMaterial.SetFloat(_fadeId, 1f);
			_thumbnailView.ImageMaterial.DOFloat(0f, _fadeId, 0.25f).SetEase(Ease.OutSine);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}