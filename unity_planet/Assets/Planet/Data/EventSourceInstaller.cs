using System;
using System.Collections.Generic;
using Planet.OfflineNewsSources;
using UniRx;
using Zenject;

namespace Planet.Data
{
	public class EventSourceInstaller : Installer<ICollection<IDisposable>, EventSourceInstaller>
	{
		[Inject]
		ICollection<IDisposable> _disposables;

		public override void InstallBindings()
		{
			var offlineSource = OfflineNewsSourceReader.FromResource();
			var source = new OfflineEventSource(offlineSource).AddTo(_disposables);
			Container.Bind<IEventSource>().FromInstance(source);
		}
	}
}