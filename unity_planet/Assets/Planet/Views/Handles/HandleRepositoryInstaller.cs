using Zenject;

namespace Planet.Views.Handles
{
	public sealed class HandleRepositoryInstaller : Installer<HandleRepositoryInstaller>
	{
		public override void InstallBindings()
		{
			var repository = new HandleRepository();
			Container.Bind(new[]
			         {
				         typeof(HandleRepository),
				         typeof(IHandleRepository),
			         })
			         .FromInstance(repository);
		}
	}
}