using System.Collections.Generic;
using NewsAPI.Constants;
using Newtonsoft.Json;

namespace NewsAPI.Models
{
	public sealed class NewsApiSourcesResponse: INewsApiResponse
	{
		[JsonProperty("status")]
		public NewsApiStatus Status { get; private set; }

		[JsonProperty("code")]
		public NewsApiErrorCode? Code { get; private set; }

		[JsonProperty("message")]
		public string Message { get; private set; }

		[JsonProperty("sources")]
		public IEnumerable<NewsApiSourceDetail> Sources { get; private set; }

		[JsonConstructor]
		NewsApiSourcesResponse()
		{
		}

		public NewsApiSourcesResponse(NewsApiErrorCode code, string message)
		{
			Status = NewsApiStatus.Error;
			Code = code;
			Message = message;
		}
	}
}