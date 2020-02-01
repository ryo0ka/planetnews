using System.Collections.Generic;
using System.IO;
using NewsAPI.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace NewsAPI.OfflineCopies
{
	public sealed class NewsApiOfflineCopy
	{
		readonly string _name;

		public NewsApiOfflineCopy(string name)
		{
			_name = name;
		}

		public IEnumerable<NewsApiSource> ReadSources()
		{
			var resourceName = NewsApiOfflineCopyFormat.SourceFileName(_name);
			resourceName = Path.GetFileNameWithoutExtension(resourceName);
			var resource = Resources.Load<TextAsset>(resourceName);
			return JsonConvert.DeserializeObject<IEnumerable<NewsApiSource>>(resource.text);
		}

		public IEnumerable<NewsApiArticle> ReadArticles(string postfix)
		{
			var resourceName = NewsApiOfflineCopyFormat.ArticleFileName(_name, postfix);
			resourceName = Path.GetFileNameWithoutExtension(resourceName);
			var resource = Resources.Load<TextAsset>(resourceName);
			return JsonConvert.DeserializeObject<IEnumerable<NewsApiArticle>>(resource.text);
		}
	}
}