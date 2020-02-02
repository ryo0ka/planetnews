using Planet.Models;
using Zenject;

namespace Planet.Data
{
	public sealed class EventFilterInstaller : Installer<EventFilterInstaller>
	{
		public override void InstallBindings()
		{
			var filters = new EventFilter();

			filters.AddFilter(new FallbackFilterEntry<IEvent>(true, e =>
			{
				return e.Language?.ToLower() == "en";
			}));

			filters.AddFilter(new FallbackFilterEntry<IEvent>(false, e =>
			{
				return !string.IsNullOrEmpty(e.ThumbnailUrl);
			}));

			Container.Bind<IEventFilter>().FromInstance(filters);
		}

		class EventFilter : FallbackFilter<IEvent>, IEventFilter
		{
		}
	}
}