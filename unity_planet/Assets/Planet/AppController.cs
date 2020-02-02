using Planet.Data;
using UniRx;
using Zenject;

namespace Planet
{
	public sealed class AppController : MonoInstaller
	{
		public override void InstallBindings()
		{
			var disposables = new CompositeDisposable().AddTo(this);

			ImageLoaderInstaller.Install(Container);
			ErrorReceiverInstaller.Install(Container);
			CountryGpsDictionaryInstaller.Install(Container);
			EventStreamerInstaller.Install(Container, disposables);
			EventFilterInstaller.Install(Container);
		}

		void Start()
		{
			new RuntimeGarbageCleaner().AddTo(this).Initialize();
		}
	}
}