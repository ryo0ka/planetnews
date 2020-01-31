using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace NewsAPI
{
	/// <summary>
	/// Use this to get results from NewsAPI.org.
	/// </summary>
	public class NewsApiClient
	{
		const string BASE_URL = "https://newsapi.org/v2/";
		readonly HttpClient HttpClient;

		/// <summary>
		/// Use this to get results from NewsAPI.org.
		/// </summary>
		/// <param name="apiKey">Your News API key. You can create one for free at https://newsapi.org.</param>
		public NewsApiClient(string apiKey)
		{
			HttpClient = new HttpClient(new HttpClientHandler {AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate});
			HttpClient.DefaultRequestHeaders.Add("user-agent", "News-API-csharp/0.1");
			HttpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
		}

		public bool LogJsonResponses { get; set; }

		/// <summary>
		/// Query the /v2/top-headlines endpoint for live top news headlines.
		/// </summary>
		/// <param name="request">The params and filters for the request.</param>
		/// <returns></returns>
		public async Task<NewsApiArticlesResponse> GetTopHeadlines(NewsApiTopHeadlinesRequest request)
		{
			// build the querystring
			var queryParams = new List<string>();

			// q
			if (!string.IsNullOrWhiteSpace(request.Q))
			{
				queryParams.Add("q=" + request.Q);
			}

			// sources
			if (request.Sources.Count > 0)
			{
				queryParams.Add("sources=" + string.Join(",", request.Sources));
			}

			if (request.Category.HasValue)
			{
				queryParams.Add("category=" + request.Category.Value.ToString().ToLowerInvariant());
			}

			if (request.Language.HasValue)
			{
				queryParams.Add("language=" + request.Language.Value.ToString().ToLowerInvariant());
			}

			if (request.Country.HasValue)
			{
				queryParams.Add("country=" + request.Country.Value.ToString().ToLowerInvariant());
			}

			// page
			if (request.Page > 1)
			{
				queryParams.Add("page=" + request.Page);
			}

			// page size
			if (request.PageSize > 0)
			{
				queryParams.Add("pageSize=" + request.PageSize);
			}

			// join them together
			var querystring = string.Join("&", queryParams.ToArray());

			return await MakeNewsRequest("top-headlines", querystring);
		}

		/// <summary>
		/// Query the /v2/everything endpoint for recent articles all over the web.
		/// </summary>
		/// <param name="request">The params and filters for the request.</param>
		/// <returns></returns>
		public async Task<NewsApiArticlesResponse> GetEverything(NewsApiEverythingRequest request)
		{
			// build the querystring
			var queryParams = new List<string>();

			// q
			if (!string.IsNullOrWhiteSpace(request.Q))
			{
				queryParams.Add("q=" + request.Q);
			}

			// sources
			if (request.Sources.Count > 0)
			{
				queryParams.Add("sources=" + string.Join(",", request.Sources));
			}

			// domains
			if (request.Domains.Count > 0)
			{
				queryParams.Add("domains=" + string.Join(",", request.Sources));
			}

			// from
			if (request.From.HasValue)
			{
				queryParams.Add("from=" + $"{request.From.Value:s}");
			}

			// to
			if (request.To.HasValue)
			{
				queryParams.Add("to=" + $"{request.To.Value:s}");
			}

			// language
			if (request.Language.HasValue)
			{
				queryParams.Add("language=" + request.Language.Value.ToString().ToLowerInvariant());
			}

			// sortBy
			if (request.SortBy.HasValue)
			{
				queryParams.Add("sortBy=" + request.SortBy.Value.ToString());
			}

			// page
			if (request.Page > 1)
			{
				queryParams.Add("page=" + request.Page);
			}

			// page size
			if (request.PageSize > 0)
			{
				queryParams.Add("pageSize=" + request.PageSize);
			}

			// join them together
			var querystring = string.Join("&", queryParams.ToArray());

			return await MakeNewsRequest("everything", querystring);
		}

		async Task<NewsApiArticlesResponse> MakeNewsRequest(string endpoint, string querystring)
		{
			var (json, error) = await MakeRequest(endpoint, querystring);
			if (error != null)
			{
				return new NewsApiArticlesResponse(error.Code, error.Message);
			}

			return JsonConvert.DeserializeObject<NewsApiArticlesResponse>(json);
		}

		public async Task<NewsApiSourcesResponse> GetSources()
		{
			var (json, error) = await MakeRequest("sources");
			if (error != null)
			{
				return new NewsApiSourcesResponse(error.Code, error.Message);
			}

			return JsonConvert.DeserializeObject<NewsApiSourcesResponse>(json);
		}

		async Task<(string, Error)> MakeRequest(string endpoint, string querystring = null)
		{
			// make the http request
			var query = string.IsNullOrEmpty(querystring) ? "" : $"?{querystring}";
			var httpRequest = new HttpRequestMessage(HttpMethod.Get, BASE_URL + endpoint + query);
			var httpResponse = await HttpClient.SendAsync(httpRequest);

			var json = await httpResponse.Content?.ReadAsStringAsync();

			if (LogJsonResponses)
			{
				Debug.Log(json);
			}

			if (string.IsNullOrWhiteSpace(json))
			{
				var error = new Error
				{
					Code = NewsApiErrorCode.UnexpectedError,
					Message = "The API returned an empty response. Are you connected to the internet?"
				};

				Debug.LogWarning($"Failed fetching: {error}");

				return (json, error);
			}

			return (json, null);
		}

		class Error
		{
			public NewsApiErrorCode Code { get; set; }
			public string Message { get; set; }

			public override string ToString()
			{
				return $"{nameof(Code)}: {Code}, {nameof(Message)}: {Message}";
			}
		}
	}
}