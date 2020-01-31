using System;
using UniRx;
using UnityEngine;

namespace Planet.Views
{
	[RequireComponent(typeof(Camera))]
	public class CameraPostRenderObserver : MonoBehaviour
	{
		Subject<Unit> _onPostRender;

		public IObservable<Unit> ObservePostRender => _onPostRender;

		void Awake()
		{
			_onPostRender = new Subject<Unit>().AddTo(this);
		}

		void OnPostRender()
		{
			_onPostRender.OnNext(Unit.Default);
		}
	}
}