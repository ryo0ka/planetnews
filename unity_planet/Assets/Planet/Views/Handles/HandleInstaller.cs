using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Planet.Views.Handles
{
	/// <summary>
	/// Attach this script along with any IHandle behaviours
	/// so that controllers can find them via IHandleRepository.
	/// </summary>
	public sealed class HandleInstaller : MonoBehaviour
	{
		HandleRepository _repository;
		IEnumerable<IHandle> _localHandles;

		[Inject, UsedImplicitly]
		public void Inject(HandleRepository handleRepository)
		{
			_repository = handleRepository;
			_localHandles = GetComponents<IHandle>();
			_repository.AddHandles(_localHandles);
		}

		void OnDestroy()
		{
			_repository.RemoveHandles(_localHandles);
		}
	}
}