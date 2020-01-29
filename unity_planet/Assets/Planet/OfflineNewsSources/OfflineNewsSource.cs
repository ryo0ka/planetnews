using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Planet.OfflineNewsSources
{
	[Serializable]
	public sealed class OfflineNewsSource
	{
		[JsonProperty("content")]
		Dictionary<string, OfflineNewsArticle[]> Content { get; }

		public OfflineNewsSource(IReadOnlyDictionary<string, IEnumerable<OfflineNewsArticle>> content)
		{
			Content = new Dictionary<string, OfflineNewsArticle[]>();
			foreach (var pair in content)
			{
				Content[pair.Key] = pair.Value.ToArray();
			}
		}

		[JsonConstructor]
		OfflineNewsSource()
		{
			Content = new Dictionary<string, OfflineNewsArticle[]>();
		}

		public IEnumerable<OfflineNewsArticle> this[string country] => Content[country];
		public bool HasCountry(string country) => Content.ContainsKey(country);
		public IEnumerable<string> Countries => Content.Keys;
	}
}