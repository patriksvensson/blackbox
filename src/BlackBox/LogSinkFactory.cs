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
using System.ComponentModel;
using System.Globalization;

namespace BlackBox
{
	internal sealed class LogSinkFactory
	{
		private readonly Dictionary<string, Type> _types;

		internal Dictionary<string, Type> Types
		{
			get { return _types; }
		}

		internal LogSinkFactory()
			: this(null)
		{
		}

		internal LogSinkFactory(IEnumerable<Assembly> assemblies)
		{
			// Create the key to string look-up table.
			_types = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

			// Make sure the BlackBox assembly is present in the collection.
			assemblies = assemblies ?? new List<Assembly>() { typeof(LogSinkFactory).Assembly };
			List<Assembly> assemblyList = new List<Assembly>(assemblies);
			if (!assemblyList.Contains(typeof(LogSinkFactory).Assembly))
			{
				assemblyList.Add(typeof(LogSinkFactory).Assembly);
			}

			// Find all sink types in the assembly that inherits from the base sink type.
			// The sink type also need to be decorated with the log sink type attribute.
			foreach (Assembly assembly in assemblyList)
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsNonAbstractClass() && type.Inherits(typeof(LogSink)))
					{
						var attribute = type.GetSingleAttribute<LogSinkTypeAttribute>(false);
						if (attribute != null)
						{
							string identifier = attribute.Name;

							// No type name?
							if (string.IsNullOrEmpty(identifier))
							{
								string message = "Log sink of type '{0}' has an empty type name.";
								throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type.FullName));
							}

							// Already exist?
							if (_types.ContainsKey(identifier))
							{
								string message = "The log sink type name '{0}' is ambiguous.";
								throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, identifier));
							}

							// No parameterles constructor?
							if (!type.HasParameterlessConstructor())
							{
								string message = "The log sink type '{0}' has no parameterless constructor.";
								throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type.FullName));
							}

							// Add the type to the list.
							_types.Add(identifier, type);
						}
					}
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
		internal LogSink Build(string type, IDictionary<string, string> arguments, bool isProxy)
		{
			// Get the "flavor" of the sink. This is used to create meaningful messages.
			string sinkFlavor = isProxy ? "Log sink proxy" : "Log sink";

			// Check if the log sink type name isn't registered.
			if (!_types.ContainsKey(type))
			{                
				string message = "{0} with type name '{1}' has not been registered.";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, sinkFlavor, type));
			}

			// Instantiate the log sink.
			LogSink sink = Activator.CreateInstance(_types[type], true) as LogSink;
			if (sink == null)
			{
				string message = "Could not instantiate {0} of type '{1}' ({2}).";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, 
					sinkFlavor.ToLowerInvariant(), _types[type].FullName, type));
			}

			// Map all log sink arguments.
			if (arguments != null && arguments.Count > 0)
			{
				this.Map(sink, arguments);
			}

			// Return the sink.
			return sink;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private void Map(LogSink sink, IDictionary<string, string> arguments)
		{
			#region Guard Statements
			if (sink == null)
			{
				throw new ArgumentNullException("sink");
			}
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			#endregion

			// Get identifier to property mappings.
			var mappings = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
			foreach (PropertyInfo property in sink.GetType().GetAllProperties())
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
						mappings[argument.Key].SetValue(sink, value, null);
					}
				}
			}
		}
	}
}
