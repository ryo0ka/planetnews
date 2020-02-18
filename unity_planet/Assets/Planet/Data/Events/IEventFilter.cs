using Planet.Models;
using Planet.Rider;
using Planet.Utils;

namespace Planet.Data.Events
{
	public interface IEventFilter
	{
		[FrequentlyCalledMethod]
		bool Test(IEvent e);
	}
}