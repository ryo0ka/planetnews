using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UniRx.Async;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Planet.TextureRefCounters.Tests
{
	public class TextureRefCountersTests
	{
		[Test]
		public void MakeNewTextureRef()
		{
			var collector = new TextureRefCollector();
			var url = "pootis";
			var texture = new Texture2D(2, 2);

			var texref = collector.MakeNewTextureRef(url, texture);
			Assert.IsNotNull(texref);
			Assert.IsTrue(collector.TryGetTextureRef(url, out var texrefStored));
			Assert.AreEqual(texref, texrefStored);
		}

		[Test]
		public void MakeNewTextureRefDuplicated()
		{
			var collector = new TextureRefCollector();
			var url = "pootis";
			var texture = new Texture2D(2, 2);

			collector.MakeNewTextureRef(url, texture);

			try
			{
				collector.MakeNewTextureRef(url, texture);
				Assert.Fail("Should throw when a duplicate url is entered to the collector");
			}
			catch (Exception e)
			{
				Assert.IsTrue(e.Message.StartsWith("Duplicate") &&
				              e.Message.EndsWith(url));
			}
		}

		[Test]
		public void LeaveTextureUnused()
		{
			var collector = new TextureRefCollector();
			var url = "pootis";
			var texture = new Texture2D(2, 2);

			collector.MakeNewTextureRef(url, texture);
			collector.CollectUnusedTextures();
			Assert.IsFalse(collector.TryGetTextureRef(url, out _));
			Assert.IsFalse(texture);
		}

		[Test]
		public void UseTexture()
		{
			var collector = new TextureRefCollector();
			var url = "pootis";
			var texture = new Texture2D(2, 2);

			var texref = collector.MakeNewTextureRef(url, texture);
			texref.Use();
			collector.CollectUnusedTextures();
			Assert.IsTrue(collector.TryGetTextureRef(url, out _));
			Assert.IsTrue(texture);
		}

		[Test]
		public void UnuseTexture()
		{
			var collector = new TextureRefCollector();
			var url = "pootis";
			var texture = new Texture2D(2, 2);

			var texref = collector.MakeNewTextureRef(url, texture);
			texref.Use();
			texref.Unuse();
			collector.CollectUnusedTextures();
			Assert.IsFalse(collector.TryGetTextureRef(url, out _));
			Assert.IsFalse(texture);
		}

		[Test]
		public void UnuseTextureTooManyTimes()
		{
			var collector = new TextureRefCollector();
			var url = "pootis";
			var texture = new Texture2D(2, 2);

			var texref = collector.MakeNewTextureRef(url, texture);
			texref.Unuse();
			collector.CollectUnusedTextures();
			LogAssert.Expect(LogType.Error, new Regex("^Negative.+" + url));
			Assert.IsFalse(collector.TryGetTextureRef(url, out _));
			Assert.IsFalse(texture);
		}

		[Test]
		public void DestroyImmediateTextureInvalidly()
		{
			var collector = new TextureRefCollector();
			var url = "pootis";
			var texture = new Texture2D(2, 2);

			var texref = collector.MakeNewTextureRef(url, texture);
			Object.DestroyImmediate(texref.Texture);
			Assert.IsFalse(texture);
			collector.CollectUnusedTextures();
			LogAssert.Expect(LogType.Error, new Regex("^Texture destroyed.+" + url));
			Assert.IsFalse(collector.TryGetTextureRef(url, out _));
		}

		[UnityTest]
		public IEnumerator DestroyTextureInvalidly()
		{
			yield return DoDestroyTextureInvalidly().ToCoroutine();
		}

		// The collector WILL FAIL to detect destroyed textures
		// in case the texture was destroyed using `Object.Destroy()`, which
		// won't destroy the texture in that frame (only "queued" to be destroyed).
		async UniTask DoDestroyTextureInvalidly()
		{
			var collector = new TextureRefCollector();
			var url = "pootis";
			var texture = new Texture2D(2, 2);

			var texref = collector.MakeNewTextureRef(url, texture);
			texref.Use();
			Object.Destroy(texref.Texture);
			Assert.IsTrue(texture); // NOT destroyed here yet (1st frame)
			collector.CollectUnusedTextures();
			Assert.IsTrue(collector.TryGetTextureRef(url, out _)); // hence survives collection

			await UniTask.Yield();

			Assert.IsFalse(texture); // Finally destroyed here (2nd frame)

			// ReSharper disable once HeuristicUnreachableCode
			collector.CollectUnusedTextures();
			LogAssert.Expect(LogType.Error, new Regex("^Texture destroyed.+" + url));
			Assert.IsFalse(collector.TryGetTextureRef(url, out _));
		}
	}
}