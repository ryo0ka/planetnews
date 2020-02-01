using System.Collections.Generic;
using System.IO;
using NewsAPI.Models;
using Newtonsoft.Json;

namespace NewsAPI.OfflineCopies
{
	public sealed class NewsApiOfflineCopy
	{
		public const string ResourceFilePath = "Assets/NewsAPI/Resources";

		readonly string _dirPath;
		readonly string _name;

		public NewsApiOfflineCopy(string dirPath, string name)
		{
			_dirPath = dirPath;
			_name = name;
		}

		string SourceFilePath(string name) => Path.Combine(_dirPath, $"{name}.sources.json");
		string ArticleFilePath(string name) => Path.Combine(_dirPath, $"{name}.articles.json");

		public static NewsApiOfflineCopy FromResources(string fileName)
		{
			return new NewsApiOfflineCopy(ResourceFilePath, fileName);
		}

		public void WriteSources(IEnumerable<NewsApiSource> sources)
		{
			var json = JsonConvert.SerializeObject(sources, Formatting.Indented);
			var path = SourceFilePath(_name);
			Directory.CreateDirectory(_dirPath);
			File.WriteAllText(path, json);
		}

		public IEnumerable<NewsApiSource> ReadSources()
		{
			var path = SourceFilePath(_name);
			var json = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<IEnumerable<NewsApiSource>>(json);
		}

		public void WriteArticles(string postfix, IEnumerable<NewsApiArticle> articles)
		{
			var json = JsonConvert.SerializeObject(articles, Formatting.Indented);
			var path = ArticleFilePath($"{_name}_{postfix}");
			Directory.CreateDirectory(_dirPath);
			File.WriteAllText(path, json);
		}

		public IEnumerable<NewsApiArticle> ReadArticles(string postfix)
		{
			var path = ArticleFilePath($"{_name}_{postfix}");
			var json = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<IEnumerable<NewsApiArticle>>(json);
		}
	}
}