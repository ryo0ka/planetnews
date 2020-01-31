using UniRx;
using Zenject;

namespace Planet.Data
{
	public sealed class AppController : MonoInstaller
	{
		public override void InstallBindings()
		{
			var disposables = new CompositeDisposable().AddTo(this);

			ImageLoaderInstaller.Install(Container);
			ErrorReceiverInstaller.Install(Container);
			CountryGpsDictionaryInstaller.Install(Container);
			EventSourceInstaller.Install(Container, disposables);
		}

		void Start()
		{
			new RuntimeGarbageCleaner().AddTo(this).Initialize();
		}
	}
}