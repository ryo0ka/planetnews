using System;
using Newtonsoft.Json;

namespace NewsAPI.Models
{
	public sealed class NewsApiArticle
	{
		[JsonProperty("source")]
		public NewsApiSource Source { get; private set; }

		[JsonProperty("author")]
		public string Author { get; private set; }

		[JsonProperty("title")]
		public string Title { get; private set; }

		[JsonProperty("description")]
		public string Description { get; private set; }

		[JsonProperty("url")]
		public string Url { get; private set; }

		[JsonProperty("urlToImage")]
		public string UrlToImage { get; private set; }

		[JsonProperty("publishedAt")]
		public DateTime? PublishedAt { get; private set; }
	}
}