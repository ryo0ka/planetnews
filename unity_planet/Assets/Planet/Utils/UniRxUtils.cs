using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Planet.Utils
{
	public static class UniRxUtils
	{
		public static IObservable<bool> OnTriggerStayingAsObservable(this Component self, Collider other)
		{
			return Observable.Create<bool>(observer =>
			{
				var disposer = new CompositeDisposable();

				self.OnTriggerEnterAsObservable()
				    .Where(c => c == other)
				    .Select(_ => true)
				    .Subscribe(observer)
				    .AddTo(other)
				    .AddTo(disposer);

				self.OnTriggerExitAsObservable()
				    .Where(c => c == other)
				    .Select(_ => false)
				    .Subscribe(observer)
				    .AddTo(other)
				    .AddTo(disposer);

				return disposer;
			});
		}
	}
}