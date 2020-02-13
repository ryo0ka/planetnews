using System;
using System.Collections.Generic;
using Planet.Models;

namespace Planet.Data
{
	public interface IEventStreamer
	{
		IEnumerable<string> Countries { get; }
		IObservable<IEvent> OnEventAdded { get; }

		/// <summary>
		/// Return all events in the country.
		/// Return an empty list if not found.
		/// </summary>
		IReadOnlyList<IEvent> GetEvents(string country);

		/// <summary>
		/// Start loading the resource.
		/// Don't do anything from the 2nd invocation.
		/// </summary>
		void StartStreaming();
	}
}