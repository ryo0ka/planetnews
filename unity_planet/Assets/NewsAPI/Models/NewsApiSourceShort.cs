using Newtonsoft.Json;

namespace NewsAPI.Models
{
	public sealed class NewsApiSourceShort
	{
		[JsonProperty("id")]
		public string Id { get; private set; }

		[JsonProperty("name")]
		public string Name { get; private set; }

		[JsonConstructor]
		NewsApiSourceShort()
		{
		}

		public NewsApiSourceShort(string id, string name)
		{
			Id = id;
			Name = name;
		}

		bool Equals(NewsApiSourceShort other)
		{
			return Id == other.Id && Name == other.Name;
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || obj is NewsApiSourceShort other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Id != null ? Id.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
			}
		}

		public override string ToString()
		{
			return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
		}
	}
}