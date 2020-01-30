using Planet.CachedImageDownloaders;
using Planet.TextureRefCounters;
using UniRx.Async;

namespace Planet.Data
{
	class CachedImageLoader : IImageLoader
	{
		readonly CachedImageDownloader _downloader;
		readonly TextureRefCollector _collector;

		public CachedImageLoader(
			CachedImageDownloader downloader,
			TextureRefCollector collector)
		{
			_downloader = downloader;
			_collector = collector;
		}

		public async UniTask<TextureRef> LoadImage(string url)
		{
			if (_collector.TryGetTextureRef(url, out var textureRef))
			{
				return textureRef;
			}

			var texture = await _downloader.Download(url);
			return _collector.MakeNewTextureRef(url, texture);
		}
	}
}