using System;
using Planet.Models;

namespace Planet.Data.Events
{
	public interface IEventSource
	{
		/// <summary>
		/// Start loading the resource.
		/// Don't do anything from the 2nd invocation.
		/// </summary>
		void StartStreaming();

		IObservable<IEvent> OnEventAdded { get; }
	}
}