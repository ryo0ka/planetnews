using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools.Constraints;
using Is = NUnit.Framework.Is;

namespace Planet.Tests
{
	public sealed class LangTests
	{
		[Test]
		public void DictionaryEnumerableAllocation()
		{
			var d = new Dictionary<string, string>();
			d["a"] = "";
			Assert.That(() =>
			{
				foreach (var pair in d)
				{
				}
			}, Is.Not.AllocatingGCMemory());
		}

		[Test]
		public void SetEnumerableAllocation()
		{
			var d = new HashSet<string>();
			d.Add("a");
			d.Add("b");
			Assert.That(() =>
			{
				foreach (var n in d)
				{
				}
			}, Is.Not.AllocatingGCMemory());
		}

		[Test]
		public void ListEnumerableAllocation()
		{
			var d = new List<string>();
			d.Add("a");
			d.Add("b");
			Assert.That(() =>
			{
				foreach (var n in d)
				{
				}
			}, Is.Not.AllocatingGCMemory());
		}
	}
}