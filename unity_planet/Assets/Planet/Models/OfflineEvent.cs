using Planet.OfflineNewsSources;

namespace Planet.Models
{
	public sealed class OfflineEvent : IEventDetailed
	{
		readonly OfflineNewsArticle _article;

		public OfflineEvent(OfflineNewsArticle article)
		{
			_article = article;
		}

		public string Title => _article.Title;
		public string ThumbnailUrl => _article.ImageUrl;
		public string Description => _article.Description;
	}
}