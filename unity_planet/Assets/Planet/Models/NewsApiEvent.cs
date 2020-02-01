using NewsAPI.Models;

namespace Planet.Models
{
	public sealed class NewsApiEvent : IEventHeadline
	{
		readonly NewsApiArticle _article;
		readonly int _id;
		readonly string _language;

		public NewsApiEvent(int id, NewsApiArticle article, string language)
		{
			_id = id;
			_article = article;
			_language = language;
		}

		public int Id => _id;
		public string Title => _article.Title;
		public string ThumbnailUrl => _article.UrlToImage;
		public string Language => _language;
	}
}