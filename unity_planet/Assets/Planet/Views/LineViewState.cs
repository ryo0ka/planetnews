using UnityEngine;

namespace Planet.Views
{
	public sealed class LineViewState
	{
		public float PastTime { get; private set; }

		public void Initialize()
		{
			PastTime = 0f;
		}

		public void Update()
		{
			PastTime += Time.deltaTime;
		}
	}
}