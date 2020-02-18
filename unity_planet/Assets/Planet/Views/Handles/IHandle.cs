using Planet.Rider;
using Planet.Utils;
using UnityEngine;

namespace Planet.Views.Handles
{
	public interface IHandle
	{
		[FrequentlyCalledMethod]
		bool Test(Collider c);
	}
}