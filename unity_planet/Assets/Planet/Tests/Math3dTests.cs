using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Planet.Tests
{
	public class Math3dTests
	{
		[Test]
		public void InverseTransformPoint()
		{
			var foo = new GameObject("foo");
			var bar = new GameObject("bar");

			foo.transform.position = Vector3.one;
			foo.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

			var inv1 = foo.transform.InverseTransformPoint(bar.transform.position);
			var inv2 = Matrix4x4.TRS(foo.transform.position, foo.transform.rotation, foo.transform.lossyScale)
			                    .inverse
			                    .MultiplyPoint3x4(bar.transform.position);

			var comparer = new Vector3EqualityComparer(10e-6f);
			Assert.That(inv1, Is.EqualTo(inv2).Using(comparer));
		}

		[Test]
		public void InverseTransformPointLookRotation()
		{
			var comparer = new Vector3EqualityComparer(10e-6f);

			var foo = new GameObject("foo");
			var bar = new GameObject("bar");

			bar.transform.position = Vector3.right; // (1, 0, 0)
			foo.transform.rotation = Quaternion.LookRotation(bar.transform.position);

			Assert.That(bar.transform.position, Is.EqualTo(new Vector3(1, 0, 0)).Using(comparer));
			Assert.That(foo.transform.rotation * Vector3.forward, Is.EqualTo(bar.transform.position).Using(comparer));

			var inv1 = foo.transform.InverseTransformPoint(bar.transform.position);
			var inv2 = Matrix4x4.TRS(
				                    foo.transform.position,
				                    foo.transform.rotation,
				                    foo.transform.lossyScale)
			                    .inverse
			                    .MultiplyPoint3x4(bar.transform.position);

			Assert.That(inv1, Is.EqualTo(inv2).Using(comparer));
		}
	}
}