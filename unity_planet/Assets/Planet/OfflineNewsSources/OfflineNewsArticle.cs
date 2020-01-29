using System;
using Newtonsoft.Json;

namespace Planet.OfflineNewsSources
{
	[Serializable]
	public sealed class OfflineNewsArticle
	{
		[JsonProperty("title")]
		public string Title { get; }

		[JsonProperty("description")]
		public string Description { get; }

		[JsonProperty("url")]
		public string Url { get; }

		[JsonProperty("imageUrl")]
		public string ImageUrl { get; }

		[JsonProperty("date")]
		public DateTime? PublishDate { get; }

		public OfflineNewsArticle(string title, string description, string url, string imageUrl, DateTime? publishDate)
		{
			Title = title;
			Description = description;
			Url = url;
			ImageUrl = imageUrl;
			PublishDate = publishDate;
		}
		
		// generated below

		bool Equals(OfflineNewsArticle other)
		{
			return Title == other.Title && Description == other.Description && Url == other.Url && ImageUrl == other.ImageUrl && Nullable.Equals(PublishDate, other.PublishDate);
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || obj is OfflineNewsArticle other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (Title != null ? Title.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Url != null ? Url.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (ImageUrl != null ? ImageUrl.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ PublishDate.GetHashCode();
				return hashCode;
			}
		}

		public override string ToString()
		{
			return $"{nameof(Title)}: {Title}, {nameof(Description)}: {Description}, {nameof(Url)}: {Url}, {nameof(ImageUrl)}: {ImageUrl}, {nameof(PublishDate)}: {PublishDate}";
		}
	}
}