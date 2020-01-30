using UniRx;
using Zenject;

namespace Planet.Data
{
	public class DataInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			ImageLoaderInstaller.Install(Container);
			ErrorReceiverInstaller.Install(Container);
			CountryGpsDictionaryInstaller.Install(Container);
			EventSourceInstaller.Install(Container, new CompositeDisposable().AddTo(this));
		}
	}
}