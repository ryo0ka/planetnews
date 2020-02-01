using System.Collections.Generic;
using System.Linq;
using NewsAPI.Models;
using Planet.Models;
using UnityEngine;

namespace Planet.Data
{
	public sealed class NewsApiEventFactory
	{
		readonly IReadOnlyDictionary<string, NewsApiSource> _sources;
		int _nextId;

		public NewsApiEventFactory(IEnumerable<NewsApiSource> sources)
		{
			_sources = sources.ToDictionary(s => s.Id);
			_nextId = 0;
		}

		public bool TryMakeEvent(NewsApiArticle article, string country, out NewsApiEvent ev)
		{
			// No source ID -> Probably not very important publishers
			var sourceId = article.Source.Id;
			if (sourceId == null)
			{
				ev = null;
				return false;
			}

			if (!_sources.TryGetValue(sourceId, out var source))
			{
				Debug.Log($"Source not found with ID: {sourceId}");
				ev = null;
				return false;
			}

			// No language specified -> Probably unintelligible to us
			var language = source.Language;
			if (language == null)
			{
				ev = null;
				return false;
			}

			ev = new NewsApiEvent(_nextId++, article, country, language);
			return true;
		}
	}
}