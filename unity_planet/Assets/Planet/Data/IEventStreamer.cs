using System;
using System.Collections.Generic;
using Planet.Models;

namespace Planet.Data
{
	public interface IEventStreamer
	{
		IEnumerable<string> Countries { get; }
		IObservable<IEvent> OnEventAdded { get; }

		bool TryGetEvents(string country, out IEnumerable<IEvent> events);

		/// <summary>
		/// Start loading the resource.
		/// Don't do anything from the 2nd invocation.
		/// </summary>
		void StartStreaming();
	}
}