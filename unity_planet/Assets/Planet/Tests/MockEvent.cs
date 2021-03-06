using System;
using Planet.Models;

namespace Planet.Tests
{
	public sealed class MockEvent : IEvent
	{
		public MockEvent(int id = 0, string source = "", string title = "", string thumbnailUrl = "", string language = "", string country = "", DateTime? publishedDate = null)
		{
			Id = id;
			Source = source;
			Title = title;
			ThumbnailUrl = thumbnailUrl;
			Language = language;
			Country = country;
			PublishedDate = publishedDate;
		}

		public int Id { get; }
		public string Source { get; }
		public string Title { get; }
		public string ThumbnailUrl { get; }
		public string Language { get; }
		public string Country { get; }
		public DateTime? PublishedDate { get; }
	}
}