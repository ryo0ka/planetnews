using UniRx;
using Zenject;

namespace Planet.Data
{
	public class MonoDataInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			var disposables = new CompositeDisposable().AddTo(this);

			ImageLoaderInstaller.Install(Container);
			ErrorReceiverInstaller.Install(Container);
			CountryGpsDictionaryInstaller.Install(Container);
			EventSourceInstaller.Install(Container, disposables);
		}
	}
}