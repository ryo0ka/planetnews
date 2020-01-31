using Newtonsoft.Json;

namespace NewsAPI.Models
{
	public sealed class NewsApiSource
	{
		[JsonProperty("id")]
		public string Id { get; private set; }

		[JsonProperty("name")]
		public string Name { get; private set; }
	}
}