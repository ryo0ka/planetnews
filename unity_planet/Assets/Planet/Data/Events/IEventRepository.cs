using System;
using System.Collections.Generic;
using Planet.Models;
using Planet.Rider;
using Planet.Utils;

namespace Planet.Data.Events
{
	public interface IEventRepository
	{
		/// <summary>
		/// Return all events from which `GetEvents()` can pull at least one event.
		/// Should be free of memory allocation on invocation.
		/// </summary>
		[FrequentlyCalledMethod]
		IEnumerable<string> Countries { get; }

		/// <summary>
		/// Observe events added to this repository after `StartStreaming()`.
		/// </summary>
		IObservable<IEvent> OnEventAdded { get; }

		/// <summary>
		/// Return all events issued in the country.
		/// Return an empty list if no events are found.
		/// The last element is the latest event.
		/// Should be free of memory allocation.
		/// </summary>
		[FrequentlyCalledMethod]
		IReadOnlyList<IEvent> GetEvents(string country);

		/// <summary>
		/// Start loading the resource.
		/// Don't do anything from the 2nd invocation.
		/// </summary>
		void StartStreaming();
	}
}