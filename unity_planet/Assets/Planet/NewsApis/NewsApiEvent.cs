using System;
using NewsAPI.Models;
using Planet.Models;

namespace Planet.NewsApis
{
	public sealed class NewsApiEvent : IEvent
	{
		readonly NewsApiArticle _article;
		readonly int _id;
		readonly string _language;
		readonly string _country;

		public NewsApiEvent(int id, NewsApiArticle article, string country, string language)
		{
			_id = id;
			_article = article;
			_country = country;
			_language = language;
		}

		public int Id => _id;
		public string Source => _article.Source.Name;
		public string Title => _article.Title;
		public string ThumbnailUrl => _article.UrlToImage;
		public string Language => _language;
		public string Country => _country;
		public DateTime? PublishedDate => _article.PublishedAt;

		public override string ToString()
		{
			return $"{nameof(_article)}: {_article}, {nameof(_id)}: {_id}, {nameof(_language)}: {_language}, {nameof(_country)}: {_country}, {nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(ThumbnailUrl)}: {ThumbnailUrl}, {nameof(Language)}: {Language}, {nameof(Country)}: {Country}";
		}
	}
}