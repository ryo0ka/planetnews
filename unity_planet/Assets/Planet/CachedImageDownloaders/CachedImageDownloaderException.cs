using System;

namespace Planet.CachedImageDownloaders
{
	public class CachedImageDownloaderException : Exception
	{
		public string Url { get; }
		public long Code { get; }
		public string Message { get; }

		public CachedImageDownloaderException(string url, long code, string message)
			: base($"{message} ({code}) {url}")
		{
			Url = url;
			Code = code;
			Message = message;
		}
	}
}