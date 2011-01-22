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

namespace BlackBox.Editor
{
	public static class TypeFinder
	{
		public static IEnumerable<Type> FindLogSinks(IEnumerable<Assembly> assemblies)
		{
			// Create the key to string look-up table.
			var types = new List<Type>();

			// Make sure the BlackBox assembly is present in the collection.
			assemblies = assemblies ?? new List<Assembly>();
			List<Assembly> assemblyList = new List<Assembly>(assemblies);
			if (!assemblyList.Contains(typeof(LogSink).Assembly))
			{
				assemblyList.Add(typeof(LogSink).Assembly);
			}

			// Find all sink types in the assembly that inherits from the base sink type.
			// The sink type also need to be decorated with the log sink type attribute.
			foreach (Assembly assembly in assemblyList)
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsNonAbstractClass() && type.Inherits(typeof(LogSink)))
					{
						// No parameterles constructor?
						if (!type.HasParameterlessConstructor())
						{
							string message = "The log sink type '{0}' has no parameterless constructor.";
							throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type.FullName));
						}

						// Add the type to the list.
						types.Add(type);
					}
				}
			}

			// Return the types.
			return types;
		}

        public static IEnumerable<Type> FindFilters(IEnumerable<Assembly> assemblies)
        {
            // Create the key to string look-up table.
            var types = new List<Type>();

            // Make sure the BlackBox assembly is present in the collection.
            assemblies = assemblies ?? new List<Assembly>();
            List<Assembly> assemblyList = new List<Assembly>(assemblies);
            if (!assemblyList.Contains(typeof(LogSink).Assembly))
            {
                assemblyList.Add(typeof(LogSink).Assembly);
            }

            // Find all filter types in the assembly that inherits from the base filter type.
            // The filter type also need to be decorated with the filter type attribute.
            foreach (Assembly assembly in assemblyList)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsNonAbstractClass() && type.Inherits(typeof(LogFilter)))
                    {
                        // No parameterles constructor?
                        if (!type.HasParameterlessConstructor())
                        {
                            string message = "The filter type '{0}' has no parameterless constructor.";
                            throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type.FullName));
                        }

                        // Add the type to the list.
                        types.Add(type);
                    }
                }
            }

            // Return the types.
            return types;
        }
	}
}
