using System;
using Newtonsoft.Json;

namespace NewsAPI.Models
{
	public sealed class NewsApiArticle
	{
		[JsonProperty("source")]
		public NewsApiSourceShort Source { get; private set; }

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

		[JsonConstructor]
		NewsApiArticle()
		{
		}

		public NewsApiArticle(NewsApiSourceShort source, string author, string title, string description, string url, string urlToImage, DateTime? publishedAt)
		{
			Source = source;
			Author = author;
			Title = title;
			Description = description;
			Url = url;
			UrlToImage = urlToImage;
			PublishedAt = publishedAt;
		}

		bool Equals(NewsApiArticle other)
		{
			return Equals(Source, other.Source) && Author == other.Author && Title == other.Title && Description == other.Description && Url == other.Url && UrlToImage == other.UrlToImage && Nullable.Equals(PublishedAt, other.PublishedAt);
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || obj is NewsApiArticle other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (Source != null ? Source.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Author != null ? Author.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Url != null ? Url.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (UrlToImage != null ? UrlToImage.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ PublishedAt.GetHashCode();
				return hashCode;
			}
		}

		public override string ToString()
		{
			return $"{nameof(Source)}: {Source}, {nameof(Author)}: {Author}, {nameof(Title)}: {Title}, {nameof(Description)}: {Description}, {nameof(Url)}: {Url}, {nameof(UrlToImage)}: {UrlToImage}, {nameof(PublishedAt)}: {PublishedAt}";
		}
	}
}