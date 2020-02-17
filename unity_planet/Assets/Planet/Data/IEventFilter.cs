using Planet.Models;

namespace Planet.Data
{
	public interface IEventFilter
	{
		bool Test(IEvent e);
	}
}