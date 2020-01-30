using Planet.TextureRefCounters;
using UniRx;
using Zenject;

namespace Planet.Data
{
	public class MonoDataInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			var disposables = new CompositeDisposable().AddTo(this);
			var textureRefCollector = new TextureRefCollector();

			ImageLoaderInstaller.Install(Container, textureRefCollector);
			ErrorReceiverInstaller.Install(Container);
			CountryGpsDictionaryInstaller.Install(Container);
			EventSourceInstaller.Install(Container, disposables);

			// Doing this explicily becasue it matters...
			new TextureRefCollectionController(textureRefCollector, disposables).Initialize();
		}
	}
}