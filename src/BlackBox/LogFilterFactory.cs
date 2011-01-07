//
// Copyright 2011 Patrik Svensson
//
// This file is part of BlackBox.
//
// BlackBox is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BlackBox is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser Public License for more details.
//
// You should have received a copy of the GNU Lesser Public License
// along with BlackBox. If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;

namespace BlackBox
{
	internal sealed class LogFilterFactory
	{
		private readonly Dictionary<string, Type> _types;

		internal Dictionary<string, Type> Types
		{
			get { return _types; }
		}

		internal LogFilterFactory()
			: this(null)
		{
		}

		internal LogFilterFactory(IEnumerable<Assembly> assemblies)
		{
			// Create the key to string look-up table.
			_types = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

			// Make sure the BlackBox assembly is present in the collection.
			assemblies = assemblies ?? new List<Assembly>() { typeof(LogFilterFactory).Assembly };
			List<Assembly> assemblyList = new List<Assembly>(assemblies);
			if (!assemblyList.Contains(typeof(LogFilterFactory).Assembly))
			{
				assemblyList.Add(typeof(LogFilterFactory).Assembly);
			}

			// Find all filter types in the assembly that inherits from the base filter type.
			// The filter type also need to be decorated with the log filter type attribute.
			foreach (Assembly assembly in assemblyList)
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsNonAbstractClass() && type.Inherits(typeof(LogFilter)))
					{
						var attribute = type.GetSingleAttribute<LogFilterTypeAttribute>(false);
						if (attribute != null)
						{
							string identifier = attribute.Name;

							// No type name?
							if (string.IsNullOrEmpty(identifier))
							{
								string message = "Log filter of type '{0}' has an empty type name.";
								throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type.FullName));
							}

							// Already exist?
							if (_types.ContainsKey(identifier))
							{
								string message = "The log filter type name '{0}' is ambiguous.";
								throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, identifier));
							}

							// No parameterles constructor?
							if (!type.HasParameterlessConstructor())
							{
								string message = "The log filter type '{0}' has no parameterless constructor.";
								throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type.FullName));
							}

							// Add the type to the list.
							_types.Add(identifier, type);
						}
					}
				}
			}
		}

		internal LogFilter Build(string type)
		{
			return this.Build(type, null);
		}

		internal LogFilter Build(string type, IDictionary<string, string> arguments)
		{
			// Check if the log sink type name isn't registered.
			if (!_types.ContainsKey(type))
			{
				string message = "Log filter with type name '{0}' has not been registered.";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type));
			}

			// Instantiate the log filter.
			LogFilter filter = Activator.CreateInstance(_types[type], true) as LogFilter;
			if (filter == null)
			{
				string message = "Could not instantiate log filter of type '{0}' ({1}).";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, _types[type].FullName, type));
			}

			// Map all log filter arguments.
			if (arguments != null && arguments.Count > 0)
			{
				this.Map(filter, arguments);
			}

			// Return the log filter.
			return filter;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private void Map(LogFilter filter, IDictionary<string, string> arguments)
		{
			#region Guard Statements
			if (filter == null)
			{
				throw new ArgumentNullException("filter");
			}
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			#endregion

			// Get identifier to property mappings.
			var mappings = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
			foreach (PropertyInfo property in filter.GetType().GetAllProperties())
			{
				mappings.Add(property.Name, property);
			}

			// Iterate through all keys in the argument dictionary.
			foreach (KeyValuePair<string, string> argument in arguments)
			{
				// Got a property mapping?
				if (mappings.ContainsKey(argument.Key))
				{
					// Get the property's type.
					Type propertyType = mappings[argument.Key].PropertyType;

					// Get the type converter.
					var typeConverter = TypeDescriptor.GetConverter(propertyType);
					if (typeConverter != null)
					{
						object value = typeConverter.ConvertFromInvariantString(argument.Value);
						mappings[argument.Key].SetValue(filter, value, null);
					}
				}
			}
		}
	}
}
