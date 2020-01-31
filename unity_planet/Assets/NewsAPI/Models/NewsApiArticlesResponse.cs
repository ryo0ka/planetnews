using NewsAPI.Constants;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewsAPI.Models
{
	public sealed class NewsApiArticlesResponse: INewsApiResponse
	{
		[JsonProperty("status")]
		public NewsApiStatus Status { get; private set; }

		[JsonProperty("code")]
		public NewsApiErrorCode? Code { get; private set; }

		[JsonProperty("message")]
		public string Message { get; private set; }

		[JsonProperty("totalResults")]
		public int TotalResults { get; private set; }

		[JsonProperty("articles")]
		public IEnumerable<NewsApiArticle> Articles { get; private set; }

		[JsonConstructor]
		NewsApiArticlesResponse()
		{
		}

		public NewsApiArticlesResponse(NewsApiErrorCode code, string message)
		{
			Status = NewsApiStatus.Error;
			Code = code;
			Message = message;
		}
	}
}