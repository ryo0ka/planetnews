using Planet.Models;

namespace Planet.Data
{
	public sealed class EventThumbnailFilter : IEventFilter
	{
		public bool Test(IEvent e)
		{
			return e.ThumbnailUrl != null;
		}
	}
}