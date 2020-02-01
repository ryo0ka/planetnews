using Planet.Models;

namespace Planet.Data
{
	public sealed class ViewableEventFilter
	{
		public bool Filter(IEvent ev)
		{
			return ev.Language?.ToLower() == "en";
		}
	}
}