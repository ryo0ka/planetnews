using System;
using Newtonsoft.Json;

namespace Planet.OfflineNewsSources
{
	[Serializable]
	public sealed class OfflineNewsArticle
	{
		[JsonProperty("title")]
		public string Title { get; private set; }

		[JsonProperty("description")]
		public string Description { get; private set; }

		[JsonProperty("url")]
		public string Url { get; private set; }

		[JsonProperty("imageUrl")]
		public string ImageUrl { get; private set; }

		[JsonProperty("date")]
		public DateTime? PublishDate { get; private set; }

		[JsonProperty("language")]
		public string Language { get; private set; }

		public OfflineNewsArticle(string title, string description, string url, string imageUrl, DateTime? publishDate, string language)
		{
			Title = title;
			Description = description;
			Url = url;
			ImageUrl = imageUrl;
			PublishDate = publishDate;
			Language = language;
		}

		bool Equals(OfflineNewsArticle other)
		{
			return Title == other.Title && Description == other.Description && Url == other.Url && ImageUrl == other.ImageUrl && Nullable.Equals(PublishDate, other.PublishDate) && Language == other.Language;
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || obj is OfflineNewsArticle other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				// ReSharper disable NonReadonlyMemberInGetHashCode
				int hashCode = (Title != null ? Title.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Url != null ? Url.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (ImageUrl != null ? ImageUrl.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ PublishDate.GetHashCode();
				hashCode = (hashCode * 397) ^ (Language != null ? Language.GetHashCode() : 0);
				return hashCode;
				// ReSharper restore NonReadonlyMemberInGetHashCode
			}
		}

		public override string ToString()
		{
			return $"{nameof(Title)}: {Title}, {nameof(Description)}: {Description}, {nameof(Url)}: {Url}, {nameof(ImageUrl)}: {ImageUrl}, {nameof(PublishDate)}: {PublishDate}, {nameof(Language)}: {Language}";
		}
	}
}