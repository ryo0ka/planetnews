using Newtonsoft.Json;

namespace NewsAPI.Models
{
	public sealed class NewsApiSource
	{
		[JsonProperty("id")]
		public string Id { get; private set; }

		[JsonProperty("name")]
		public string Name { get; private set; }

		[JsonProperty("description")]
		public string Description { get; private set; }

		[JsonProperty("url")]
		public string Url { get; private set; }

		[JsonProperty("category")]
		public string Category { get; private set; }

		[JsonProperty("language")]
		public string Language { get; private set; }

		[JsonProperty("country")]
		public string Country { get; private set; }

		[JsonConstructor]
		NewsApiSource()
		{
		}

		public NewsApiSource(string id, string name, string description, string url, string category, string language, string country)
		{
			Id = id;
			Name = name;
			Description = description;
			Url = url;
			Category = category;
			Language = language;
			Country = country;
		}

		bool Equals(NewsApiSource other)
		{
			return Id == other.Id && Name == other.Name && Description == other.Description && Url == other.Url && Category == other.Category && Language == other.Language && Country == other.Country;
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || obj is NewsApiSource other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				// ReSharper disable NonReadonlyMemberInGetHashCode
				int hashCode = (Id != null ? Id.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Url != null ? Url.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Category != null ? Category.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Language != null ? Language.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Country != null ? Country.GetHashCode() : 0);
				return hashCode;
				// ReSharper restore NonReadonlyMemberInGetHashCode
			}
		}

		public override string ToString()
		{
			return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}, {nameof(Url)}: {Url}, {nameof(Category)}: {Category}, {nameof(Language)}: {Language}, {nameof(Country)}: {Country}";
		}
	}
}