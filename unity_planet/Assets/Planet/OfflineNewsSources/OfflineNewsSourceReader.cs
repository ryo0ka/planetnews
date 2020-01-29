using Newtonsoft.Json;
using UnityEngine;

namespace Planet.OfflineNewsSources
{
	public static class OfflineNewsSourceReader
	{
		public static OfflineNewsSource FromResource()
		{
			var textAsset = Resources.Load<TextAsset>("OfflineNewsSource");
			return JsonConvert.DeserializeObject<OfflineNewsSource>(textAsset.text);
		}
	}
}