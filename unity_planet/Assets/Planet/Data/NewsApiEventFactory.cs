using System.Collections.Generic;
using System.Linq;
using NewsAPI.Models;
using Planet.Models;

namespace Planet.Data
{
	/// <summary>
	/// Converts NewsApi events to IEvent.
	/// </summary>
	public sealed class NewsApiEventFactory
	{
		readonly IReadOnlyDictionary<string, NewsApiSource> _sources;
		int _nextId;

		public NewsApiEventFactory(IEnumerable<NewsApiSource> sources)
		{
			_sources = sources.ToDictionary(s => s.Id);
			_nextId = 0;
		}

		public NewsApiEvent MakeEvent(NewsApiArticle article, string country)
		{
			var sourceId = article.Source.Id;
			_sources.TryGetValue(sourceId ?? "", out var source);
			var language = source?.Language;
			return new NewsApiEvent(_nextId++, article, country, language);
		}
	}
}