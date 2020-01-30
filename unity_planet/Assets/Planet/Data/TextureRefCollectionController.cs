using System;
using System.Collections.Generic;
using Planet.TextureRefCounters;
using UniRx;

namespace Planet.Data
{
	public class TextureRefCollectionController
	{
		readonly TextureRefCollector _collector;
		readonly ICollection<IDisposable> _disposables;

		public TextureRefCollectionController(
			TextureRefCollector collector,
			ICollection<IDisposable> disposables)
		{
			_collector = collector;
			_disposables = disposables;
		}

		public void Initialize()
		{
			Observable
				.Interval(TimeSpan.FromSeconds(5f))
				.Subscribe(_ => _collector.CollectUnusedTextures())
				.AddTo(_disposables);
		}
	}
}