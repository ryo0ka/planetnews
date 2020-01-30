using System;
using UniRx;
using UnityEngine;

namespace Planet.Data
{
	public class RuntimeGarbageCleaner : IDisposable
	{
		readonly CompositeDisposable _disposable;

		public RuntimeGarbageCleaner()
		{
			_disposable = new CompositeDisposable();
		}

		public void Initialize()
		{
			Observable
				.Interval(TimeSpan.FromSeconds(5f))
				.Subscribe(_ => Resources.UnloadUnusedAssets())
				.AddTo(_disposable);
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}
}