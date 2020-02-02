using System;
using Planet.CachedImageDownloaders;
using Planet.Data;
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

		public Material ImageMaterial => _image.materialForRendering;

		void Awake()
		{
			// Clone the material so we can control each individual panel
			_image.material = new Material(_image.material);
		}

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
			SetTexture(_loadingTexture);
			_currentUrl = url;

			try
			{
				var texture = await _imageLoader.LoadImage(url);

				// cancel if the scene has been unloaded
				if (!this || !_image || !_imageFitter) return;

				// skipped
				if (_currentUrl != url)
				{
					Debug.Log($"Skipped loading image: {url}");
					return;
				}

				SetTexture(texture);
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