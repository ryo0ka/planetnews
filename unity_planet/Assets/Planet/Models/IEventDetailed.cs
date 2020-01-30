namespace Planet.Models
{
	public interface IEventDetailed : IEventHeadline
	{
		string Description { get; }
	}
}