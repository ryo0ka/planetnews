using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Planet.Utils
{
	public static class UniRxUtils
	{
		public static IObservable<bool> OnTriggerInOutAsObservable(this Component self, Func<Collider, bool> other)
		{
			return Observable.Create<bool>(observer =>
			{
				var disposer = new CompositeDisposable();

				self.OnTriggerEnterAsObservable()
				    .Where(c => other(c))
				    .Select(_ => true)
				    .Subscribe(observer)
				    .AddTo(disposer);

				self.OnTriggerExitAsObservable()
				    .Where(c => other(c))
				    .Select(_ => false)
				    .Subscribe(observer)
				    .AddTo(disposer);

				return disposer;
			});
		}
	}
}