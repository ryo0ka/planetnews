using System.Collections.Generic;
using Planet.Models;

namespace Planet.Data
{
	public interface IEventFilter
	{
		IEnumerable<IEvent> Filter(IEnumerable<IEvent> events);
	}
}