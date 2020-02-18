using Planet.Data;
using Planet.Data.Errors;
using Planet.Data.Events;
using Planet.Data.Images;
using Planet.Views.Handles;
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
			EventRepositoryInstaller.Install(Container, disposables);
			HandleRepositoryInstaller.Install(Container);
		}

		void Start()
		{
			new RuntimeGarbageCleaner().AddTo(this).Initialize();
		}
	}
}