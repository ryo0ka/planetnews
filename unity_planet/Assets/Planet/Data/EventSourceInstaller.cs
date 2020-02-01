using System;
using System.Collections.Generic;
using NewsAPI.OfflineCopies;
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
			var newsApiOfflineCopy = NewsApiOfflineCopy.FromResources("tmp");
			var source = new OfflineEventSource(newsApiOfflineCopy).AddTo(_disposables);
			Container.Bind<IEventSource>().FromInstance(source);
		}
	}
}