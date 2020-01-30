using Planet.CachedImageDownloaders;
using Planet.TextureRefCounters;
using Zenject;

namespace Planet.Data
{
	public sealed class ImageLoaderInstaller : Installer<TextureRefCollector, ImageLoaderInstaller>
	{
		[Inject]
		TextureRefCollector _textureRefCollector;

		public override void InstallBindings()
		{
			const string CacheDirName = "planet_cache";
			var cachePathGenerator = new CachePathGenerator(CacheDirName);
			var imageDownloader = new CachedImageDownloader(cachePathGenerator);
			var imageLoader = new CachedImageLoader(imageDownloader, _textureRefCollector);
			Container.Bind<IImageLoader>().FromInstance(imageLoader);
		}
	}
}