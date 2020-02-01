using NewsAPI.Constants;

namespace NewsAPI.Models
{
	public interface INewsApiResponse
	{
		NewsApiStatus Status { get; }
		NewsApiErrorCode? Code { get; }
		string Message { get; }
	}
}