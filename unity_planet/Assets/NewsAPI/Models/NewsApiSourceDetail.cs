using Newtonsoft.Json;

namespace NewsAPI.Models
{
	public sealed class NewsApiSourceDetail
	{
		[JsonProperty("id")]
		public string Id { get; private set; }

		[JsonProperty("name")]
		public string Name { get; private set; }

		[JsonProperty("description")]
		public string Description { get; private set; }

		[JsonProperty("url")]
		public string Url { get; private set; }

		[JsonProperty("category")]
		public string Category { get; private set; }

		[JsonProperty("language")]
		public string Language { get; private set; }

		[JsonProperty("country")]
		public string Country { get; private set; }
	}
}