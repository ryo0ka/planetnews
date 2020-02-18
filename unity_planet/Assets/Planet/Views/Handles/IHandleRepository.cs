using UnityEngine;

namespace Planet.Views.Handles
{
	public interface IHandleRepository
	{
		bool Test(Ray ray, out IHandle handle);
		bool Test(Collider collider, out IHandle handle);
	}
}