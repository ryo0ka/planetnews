namespace Planet.Models
{
	public interface IEvent
	{
		int Id { get; }
		string Source { get; }
		string Title { get; }
		string ThumbnailUrl { get; }
		string Language { get; }
		string Country { get; }
	}
}