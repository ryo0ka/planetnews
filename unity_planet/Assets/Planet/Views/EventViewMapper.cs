using System.Collections.Generic;
using System.Linq;

namespace Planet.Views
{
	public sealed class EventViewMapper
	{
		readonly string[] _mappedCountries;
		readonly HashSet<string> _focusedCountries;
		readonly HashSet<string> _focusedCountriesNotMapped;

		public EventViewMapper(int eventViewCount)
		{
			_mappedCountries = new string[eventViewCount];
			_focusedCountries = new HashSet<string>();
			_focusedCountriesNotMapped = new HashSet<string>();
		}

		/// NOTE Contents can be null if the index is not mapped to any countries
		public IReadOnlyList<string> MappedCountries => _mappedCountries.ToArray();

		public void UpdateMapping(IEnumerable<string> focusedCountries)
		{
			_focusedCountries.Clear();
			_focusedCountries.UnionWith(focusedCountries);

			_focusedCountriesNotMapped.Clear();
			_focusedCountriesNotMapped.UnionWith(focusedCountries);

			for (var i = 0; i < _mappedCountries.Length; i++)
			{
				if (!_focusedCountriesNotMapped.Any()) return;

				var mappedCountry = _mappedCountries[i];
				if (_focusedCountriesNotMapped.Contains(mappedCountry))
				{
					_focusedCountriesNotMapped.Remove(mappedCountry);
				}
				else if (mappedCountry == null)
				{
					var focusedCountry = _focusedCountriesNotMapped.First();
					_focusedCountriesNotMapped.Remove(focusedCountry);
					_mappedCountries[i] = focusedCountry;
				}
			}

			for (var i = 0; i < _mappedCountries.Length; i++)
			{
				if (!_focusedCountriesNotMapped.Any()) return;

				var mappedCountry = _mappedCountries[i];
				if (!_focusedCountries.Contains(mappedCountry))
				{
					var focusedCountry = _focusedCountriesNotMapped.First();
					_focusedCountriesNotMapped.Remove(focusedCountry);
					_mappedCountries[i] = focusedCountry;
				}
			}
		}
	}
}