using System;
using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UniRx.Async;
using UnityEngine.TestTools;

namespace Planet.CachedImageDownloaders.Tests
{
	public class ImageDownloadersTests
	{
		[Test]
		public void CacheFilePathEquality()
		{
			var cachePathGenerator = new CachePathGenerator($"test-{Guid.NewGuid():N}");
			var imageUrl = "https://sample-videos.com/img/Sample-jpg-image-50kb.jpg";

			Assert.AreEqual(
				cachePathGenerator.GenerateCachePath(imageUrl),
				cachePathGenerator.GenerateCachePath(imageUrl),
				"Different cache file path returned for equivalent inputs");
		}

		[Test]
		public void CacheFileReadWrite()
		{
			var cachePathGenerator = new CachePathGenerator($"test-{Guid.NewGuid():N}");
			var imageUrl = "https://sample-videos.com/img/Sample-jpg-image-50kb.jpg";
			var cachePath = cachePathGenerator.GenerateCachePath(imageUrl);

			Directory.CreateDirectory(cachePathGenerator.DirPath);
			File.WriteAllBytes(cachePath, new byte[] {0});

			Assert.IsTrue(File.Exists(cachePath));
		}

		[UnityTest]
		public IEnumerator DownloadImageCaching()
		{
			yield return DoDownloadImageCaching().ToCoroutine();
		}

		async UniTask DoDownloadImageCaching()
		{
			var cachePathGenerator = new CachePathGenerator($"test-{Guid.NewGuid():N}");
			var imageDownloader = new CachedImageDownloader(cachePathGenerator);
			var imageUrl = "https://sample-videos.com/img/Sample-jpg-image-50kb.jpg";

			var texture = await imageDownloader.Download(imageUrl);
			var textureCached = await imageDownloader.Download(imageUrl);

			Assert.IsFalse(texture.GetPixels().SequenceEqual(textureCached.GetPixels()));
		}

		[UnityTest]
		public IEnumerator DownloadImageNoCaching()
		{
			yield return DoDownloadImageNoCaching().ToCoroutine();
		}

		async UniTask DoDownloadImageNoCaching()
		{
			var imageDownloader1 = new CachedImageDownloader(new CachePathGenerator($"test-{Guid.NewGuid():N}"));
			var imageDownloader2 = new CachedImageDownloader(new CachePathGenerator($"test-{Guid.NewGuid():N}"));
			var imageUrl = "https://sample-videos.com/img/Sample-jpg-image-50kb.jpg";

			var texture1 = await imageDownloader1.Download(imageUrl);
			var texture2 = await imageDownloader2.Download(imageUrl);

			Assert.IsTrue(texture1.GetPixels().SequenceEqual(texture2.GetPixels()));
		}
	}
}