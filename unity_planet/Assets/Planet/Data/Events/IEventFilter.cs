using Planet.Models;

namespace Planet.Data.Events
{
	public interface IEventFilter
	{
		bool Test(IEvent e);
	}
}