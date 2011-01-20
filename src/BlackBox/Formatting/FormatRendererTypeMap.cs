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

namespace BlackBox.Formatting
{
    internal sealed class FormatRendererTypeMap<TContext>
    {
        private readonly Dictionary<string, Type> _types;

        public FormatRendererTypeMap()
            : this(null)
        {
        }

        public FormatRendererTypeMap(IEnumerable<Assembly> assemblies)
        {
            // Create the key to string look-up table.
            _types = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

            // Make sure the BlackBox assembly is present in the collection.
            assemblies = assemblies ?? new List<Assembly>() { typeof(FormatRendererTypeMap<>).Assembly };
            List<Assembly> assemblyList = new List<Assembly>(assemblies);
            if (!assemblyList.Contains(typeof(FormatRendererTypeMap<>).Assembly))
            {
                assemblyList.Add(typeof(FormatRendererTypeMap<>).Assembly);
            }

            // Get the type for the base class renderer.
            Type rendererBaseType = typeof(FormatRenderer<>).MakeGenericType(typeof(TContext));

            // Find all types in the assembly that inherits from the base type.
            // If the type is decorated with the specified attribute, then add it to the internal look-up table.
            foreach (Assembly assembly in assemblyList)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    bool isGenericRenderer = false;
                    if (type.IsGenericType && type.ContainsGenericParameters && type.IsNonAbstractClass())
                    {
                        if (type.InheritsGenericType(typeof(FormatRenderer<>)))
                        {
                            isGenericRenderer = true;
                        }                       
                    }

                    bool isNonGenericRenderer = type.IsNonAbstractClass() && type.Inherits(rendererBaseType);
                    if (isNonGenericRenderer || isGenericRenderer)
                    {                        
                        var attribute = type.GetAttribute<FormatRendererTypeAttribute>(false);
                        if (attribute != null)
                        {
                            string identifier = attribute.Name;
                            if (!identifier.IsNullOrEmpty() && !_types.ContainsKey(identifier))
                            {
                                _types.Add(identifier, type);
                            }
                        }
                    }
                }
            }
        }

        internal Type GetRendererType(string name)
        {
            if (!_types.ContainsKey(name))
            {
                return null;
            }
            return _types[name];
        }
    }
}
