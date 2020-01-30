using Planet.CachedImageDownloaders;
using Zenject;

namespace Planet.Data
{
	public sealed class ImageLoaderInstaller : Installer<ImageLoaderInstaller>
	{
		public override void InstallBindings()
		{
			const string CacheDirName = "planet_cache";
			var cachePathGenerator = new CachePathGenerator(CacheDirName);
			var imageDownloader = new CachedImageDownloader(cachePathGenerator);
			var imageLoader = new CachedImageLoader(imageDownloader);
			Container.Bind<IImageLoader>().FromInstance(imageLoader);
		}
	}
}