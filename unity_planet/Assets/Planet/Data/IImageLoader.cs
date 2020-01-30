using Planet.TextureRefCounters;
using UniRx.Async;

namespace Planet.Data
{
	public interface IImageLoader
	{
		UniTask<TextureRef> LoadImage(string url);
	}
}