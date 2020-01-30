using Zenject;

namespace Planet.Data
{
	public class ErrorReceiverInstaller : Installer<ErrorReceiverInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<IErrorReceiver>().To<SilentErrorReceiver>().AsSingle();
		}
	}
}