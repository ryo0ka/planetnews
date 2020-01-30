using Planet.CountryCodeToGps;
using Zenject;

namespace Planet.Data
{
	public class CountryGpsDictionaryInstaller: Installer<CountryGpsDictionaryInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInstance(CountryGpsFactory.FromResource());
		}
	}
}