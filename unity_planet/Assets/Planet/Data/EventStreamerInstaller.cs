using System;
using System.Collections.Generic;
using NewsAPI.Constants;
using NewsAPI.OfflineCopies;
using UniRx;
using Zenject;

namespace Planet.Data
{
	public class EventStreamerInstaller : Installer<ICollection<IDisposable>, EventStreamerInstaller>
	{
		[Inject]
		ICollection<IDisposable> _disposables;

		public override void InstallBindings()
		{
			var io = NewsApiOfflineCopy.FromResources("tmp");
			var factory = new NewsApiEventFactory(io.ReadSources());
			var source = new NewsApiEventStreamer(factory).AddTo(_disposables);
			var countries = (NewsApiCountry[]) Enum.GetValues(typeof(NewsApiCountry));
			foreach (var country in countries)
			{
				var countryStr = country.ToString();
				var countryArticles = io.ReadArticles(countryStr);
				source.SetArticles(countryStr, countryArticles);
			}

			Container.Bind<IEventStreamer>().FromInstance(source);
		}
	}
}