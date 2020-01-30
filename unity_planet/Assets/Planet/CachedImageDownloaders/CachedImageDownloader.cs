using System.IO;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;

namespace Planet.CachedImageDownloaders
{
	public sealed class CachedImageDownloader
	{
		readonly CachePathGenerator _cachePathGenerator;

		public CachedImageDownloader(CachePathGenerator cachePathGenerator)
		{
			_cachePathGenerator = cachePathGenerator;
		}

		public async UniTask<Texture2D> Download(string url)
		{
			try
			{
				await UniTask.SwitchToThreadPool();

				// just making sure
				Directory.CreateDirectory(_cachePathGenerator.DirPath);

				var cacheFilePath = _cachePathGenerator.GenerateCachePath(url);
				if (File.Exists(cacheFilePath))
				{
					await UniTask.SwitchToMainThread();

					var qualifiedCacheFilePath = $"file://{cacheFilePath}";
					return await DoDownload(qualifiedCacheFilePath);
				}

				await UniTask.SwitchToMainThread();

				var texture = await DoDownload(url);
				var textureData = texture.EncodeToJPG(80);

				await UniTask.SwitchToThreadPool();

				File.WriteAllBytes(cacheFilePath, textureData);

				await UniTask.SwitchToMainThread();

				return texture;
			}
			finally
			{
				await UniTask.SwitchToMainThread();
			}
		}

		async UniTask<Texture2D> DoDownload(string url)
		{
			using (var request = UnityWebRequestTexture.GetTexture(url))
			{
				var response = await request.SendWebRequest();

				if (response.isHttpError ||
				    response.isNetworkError ||
				    response.error != null)
				{
					throw new CachedImageDownloaderException(url, response.responseCode, response.error);
				}

				return ((DownloadHandlerTexture) response.downloadHandler).texture;
			}
		}
	}
}