using System;
using System.Collections.Generic;
using Planet.Models;

namespace Planet.Data
{
	public interface IEventSource
	{
		IEnumerable<IEventDetailed> this[string country] { get; }
		bool HasCountry(string country);
		IEnumerable<string> Countries { get; }
		IObservable<string> OnEventsAddedToCountry { get; }
	}
}