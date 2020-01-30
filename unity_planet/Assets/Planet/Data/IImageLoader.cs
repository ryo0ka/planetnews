using UniRx.Async;
using UnityEngine;

namespace Planet.Data
{
	public interface IImageLoader
	{
		UniTask<Texture2D> LoadImage(string url);
	}
}