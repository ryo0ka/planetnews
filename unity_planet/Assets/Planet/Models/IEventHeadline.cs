namespace Planet.Models
{
	public interface IEventHeadline
	{
		int Id { get; }
		string Title { get; }
		string ThumbnailUrl { get; }
		string Language { get; }
	}
}