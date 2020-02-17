using UniRx.Async;
using UnityEngine;

namespace Planet.Data.Images
{
	public interface IImageLoader
	{
		UniTask<Texture2D> LoadImage(string url);
	}
}