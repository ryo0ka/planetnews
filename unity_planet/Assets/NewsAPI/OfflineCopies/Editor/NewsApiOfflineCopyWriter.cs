using System.Collections.Generic;
using System.IO;
using NewsAPI.Models;
using Newtonsoft.Json;
using UnityEditor;

namespace NewsAPI.OfflineCopies.Editor
{
	public sealed class NewsApiOfflineCopyWriter
	{
		public const string ResourceFilePath = "Assets/NewsAPI/Resources";
		readonly string _name;

		public NewsApiOfflineCopyWriter(string name)
		{
			_name = name;
		}

		public void WriteSources(IEnumerable<NewsApiSource> sources)
		{
			var json = JsonConvert.SerializeObject(sources, Formatting.Indented);
			var name = NewsApiOfflineCopyFormat.SourceFileName(_name);
			var path = Path.Combine(ResourceFilePath, name);
			Directory.CreateDirectory(ResourceFilePath);
			File.WriteAllText(path, json);
			AssetDatabase.Refresh();
		}

		public void WriteArticles(string postfix, IEnumerable<NewsApiArticle> articles)
		{
			var json = JsonConvert.SerializeObject(articles, Formatting.Indented);
			var name = NewsApiOfflineCopyFormat.ArticleFileName(_name, postfix);
			var path = Path.Combine(ResourceFilePath, name);
			Directory.CreateDirectory(ResourceFilePath);
			File.WriteAllText(path, json);
			AssetDatabase.Refresh();
		}
	}
}