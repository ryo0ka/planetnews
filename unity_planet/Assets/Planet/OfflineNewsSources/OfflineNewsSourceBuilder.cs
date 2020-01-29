using System.Collections.Generic;
using System.Linq;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace Planet.OfflineNewsSources
{
	public sealed class OfflineNewsSourceBuilder
	{
		readonly Dictionary<string, IEnumerable<OfflineNewsArticle>> _content;

		public OfflineNewsSourceBuilder()
		{
			_content = new Dictionary<string, IEnumerable<OfflineNewsArticle>>();
		}

		public void Add(Countries country, IEnumerable<Article> articles)
		{
			_content[country.ToString()] = articles.Select(a => new OfflineNewsArticle(
				title: a.Title,
				description: a.Description,
				url: a.Url,
				imageUrl: a.UrlToImage,
				publishDate: a.PublishedAt));
		}

		public void Add(string country, IEnumerable<OfflineNewsArticle> articles)
		{
			_content[country] = articles;
		}

		public OfflineNewsSource Build()
		{
			return new OfflineNewsSource(_content);
		}
	}
}