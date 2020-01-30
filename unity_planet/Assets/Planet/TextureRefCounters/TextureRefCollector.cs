using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Planet.TextureRefCounters
{
	public sealed class TextureRefCollector
	{
		readonly Dictionary<string, TextureRef> _textures;

		public TextureRefCollector()
		{
			_textures = new Dictionary<string, TextureRef>();
		}

		public TextureRef MakeNewTextureRef(string url, Texture2D texture)
		{
			if (_textures.ContainsKey(url))
			{
				throw new Exception($"Duplicate texture entry. Potentially leaking: {url}");
			}

			var textureRef = new TextureRef(texture);
			_textures.Add(url, textureRef);
			return textureRef;
		}

		public bool TryGetTextureRef(string url, out TextureRef texture)
		{
			return _textures.TryGetValue(url, out texture) && !texture.IsDestroyed;
		}

		public void CollectUnusedTextures()
		{
			var unusedTextureUrls = new List<string>();
			foreach (var pair in _textures)
			{
				var url = pair.Key;
				var texture = pair.Value;

				if (texture.IsDestroyed)
				{
					Debug.LogError($"Texture destroyed outside collector: {url}");
					unusedTextureUrls.Add(url);
				}

				if (texture.ReferencedCount < 0)
				{
					Debug.LogError($"Negative texture referenced count: {url}");
					unusedTextureUrls.Add(url);
					Object.DestroyImmediate(texture.Texture);
				}

				if (texture.ReferencedCount == 0)
				{
					unusedTextureUrls.Add(url);
					Object.DestroyImmediate(texture.Texture);
				}
			}

			foreach (var unusedTextureUrl in unusedTextureUrls)
			{
				_textures.Remove(unusedTextureUrl);
			}
		}
	}
}