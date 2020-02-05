using UnityEngine;
using Zenject;

namespace Planet.Views
{
	public class OculusViewInstaller : MonoInstaller
	{
		[SerializeField]
		OculusTouchController _touchController;

		public override void InstallBindings()
		{
			Container.Bind(new[]
			{
				typeof(IRotationHandleController),
			}).FromInstance(_touchController);
		}
	}
}