using System.Collections.Generic;
using Planet.Utils;
using UnityEngine;

namespace Planet.Views.Handles
{
	public sealed class HandleRepository : IHandleRepository
	{
		readonly HashSet<IHandle> _handles;
		readonly RaycastHit[] _hits;

		public HandleRepository()
		{
			_handles = new HashSet<IHandle>();
			_hits = new RaycastHit[10];
		}

		public void AddHandles(IEnumerable<IHandle> handles)
		{
			_handles.AddRange(handles);
		}

		public void RemoveHandles(IEnumerable<IHandle> handles)
		{
			_handles.RemoveRange(handles);
		}

		public bool Test(Ray ray, out IHandle handle)
		{
			var hitCount = Physics.RaycastNonAlloc(ray, _hits);
			for (int i = 0; i < hitCount; i++)
			{
				var hit = _hits[i];
				if (Test(hit.collider, out handle))
				{
					return true;
				}
			}

			handle = null;
			return false;
		}

		public bool Test(Collider c, out IHandle handle)
		{
			foreach (var h in _handles)
			{
				if (h.Test(c))
				{
					handle = h;
					return true;
				}
			}

			handle = null;
			return false;
		}
	}
}