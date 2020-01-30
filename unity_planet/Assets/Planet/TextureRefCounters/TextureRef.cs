using UnityEngine;

namespace Planet.TextureRefCounters
{
	public sealed class TextureRef
	{
		internal TextureRef(Texture2D texture)
		{
			Texture = texture;
			ReferencedCount = 0;
		}

		public Texture2D Texture { get; }

		internal int ReferencedCount { get; private set; }
		internal bool IsDestroyed => !Texture;

		public void Use() => ReferencedCount += 1;
		public void Unuse() => ReferencedCount -= 1;
	}
}