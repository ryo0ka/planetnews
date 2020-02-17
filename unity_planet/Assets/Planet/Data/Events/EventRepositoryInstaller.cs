using System;
using System.Collections.Generic;
using NewsAPI.Constants;
using NewsAPI.OfflineCopies;
using Planet.NewsApis;
using UniRx;
using Zenject;

namespace Planet.Data.Events
{
	public class EventRepositoryInstaller : Installer<ICollection<IDisposable>, EventRepositoryInstaller>
	{
		[Inject]
		ICollection<IDisposable> _disposables;

		public override void InstallBindings()
		{
			var io = new NewsApiOfflineCopy("tmp");
			var factory = new NewsApiEventFactory(io.ReadSources());
			var source = new NewsApiEventSource(factory).AddTo(_disposables);
			
			// Load offline news into source
			var countries = (NewsApiCountry[]) Enum.GetValues(typeof(NewsApiCountry));
			foreach (var country in countries)
			{
				var countryStr = country.ToString();
				var countryArticles = io.ReadArticles(countryStr);
				source.SetArticles(countryStr, countryArticles);
			}
			
			var filter = new EventThumbnailFilter();
			var repository = new FilteredEventRepository(source, filter);

			Container.Bind<IEventRepository>().FromInstance(repository);
		}
	}
}