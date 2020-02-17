using Zenject;

namespace Planet.Data.Errors
{
	public class ErrorReceiverInstaller : Installer<ErrorReceiverInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<IErrorReceiver>().To<SilentErrorReceiver>().AsSingle();
		}
	}
}