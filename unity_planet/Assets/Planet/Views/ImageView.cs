using System;
using Planet.CachedImageDownloaders;
using Planet.Data;
using Planet.TextureRefCounters;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Planet.Views
{
	public class ImageView : MonoBehaviour
	{
		[SerializeField]
		RawImage _image;

		[SerializeField]
		AspectRatioFitter _imageFitter;

		[SerializeField]
		Texture _defaultTexture;

		[SerializeField]
		Texture _loadingTexture;

		IImageLoader _imageLoader;
		IErrorReceiver _errorReceiver;

		string _currentUrl;
		TextureRef _currentTextureRef;

		[Inject]
		public void Inject(IImageLoader imageLoader)
		{
			_imageLoader = imageLoader;
		}

		[Inject]
		public void Inject(IErrorReceiver errorReceiver)
		{
			_errorReceiver = errorReceiver;
		}

		public async UniTask LoadImage(string url)
		{
			// release the last texture
			_currentTextureRef?.Unuse();
			_currentTextureRef = null;

			_currentUrl = url;
			SetTexture(_loadingTexture);

			try
			{
				var textureRef = await _imageLoader.LoadImage(url);

				// skipped
				if (_currentUrl != url)
				{
					Debug.Log($"Skipped loading image: {url}");
					return;
				}

				_currentTextureRef = textureRef;
				_currentTextureRef.Use();
				SetTexture(_currentTextureRef.Texture);
			}
			catch (CachedImageDownloaderException e) when (e.Code == 404)
			{
				Debug.Log($"Image not found (404): {e.Url}");
			}
			catch (Exception e)
			{
				_errorReceiver.Receive(e, "Failed loading image", "Check the Internet connection");
				SetTexture(_defaultTexture);
			}
		}

		public void LoadDefaultImage()
		{
			_currentUrl = null;

			// release the last texture
			_currentTextureRef?.Unuse();
			_currentTextureRef = null;

			SetTexture(_defaultTexture);
		}

		void SetTexture(Texture texture)
		{
			_image.texture = texture;

			if (texture != null)
			{
				_imageFitter.aspectRatio = (float) texture.width / texture.height;
			}
		}
	}
}