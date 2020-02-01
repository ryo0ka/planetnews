using System.Collections.Generic;
using Planet.Models;

namespace Planet.Data
{
	public interface IEventStreamer
	{
		IEnumerable<string> Countries { get; }
		IEnumerable<IEvent> GetEvents(string country);

		/// <summary>
		/// Start loading the resource.
		/// Don't do anything from the 2nd invocation.
		/// </summary>
		void StartStreaming();
	}
}