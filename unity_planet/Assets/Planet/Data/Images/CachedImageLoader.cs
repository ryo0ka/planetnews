using Planet.CachedImageDownloaders;
using UniRx.Async;
using UnityEngine;

namespace Planet.Data.Images
{
	class CachedImageLoader : IImageLoader
	{
		readonly CachedImageDownloader _downloader;

		public CachedImageLoader(CachedImageDownloader downloader)
		{
			_downloader = downloader;
		}

		public UniTask<Texture2D> LoadImage(string url)
		{
			return _downloader.Download(url);
		}
	}
}