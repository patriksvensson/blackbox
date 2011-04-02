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
using System.Globalization;
using System.Reflection;

namespace BlackBox.Conditions
{
    internal sealed class ConditionFactory
    {
		private readonly Dictionary<string, Type> _types;
        private readonly Dictionary<Type, MethodExpressionAttribute> _attributes;

		internal Dictionary<string, Type> Types
		{
			get { return _types; }
		}

		internal ConditionFactory()
			: this(null)
		{
		}

        internal ConditionFactory(IEnumerable<Assembly> assemblies)
		{
			_types = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            _attributes = new Dictionary<Type, MethodExpressionAttribute>();

			// Make sure the BlackBox assembly is present in the collection.
			assemblies = assemblies ?? new List<Assembly>() { typeof(LogSinkFactory).Assembly };
			List<Assembly> assemblyList = new List<Assembly>(assemblies);
			if (!assemblyList.Contains(typeof(LogSinkFactory).Assembly))
			{
				assemblyList.Add(typeof(LogSinkFactory).Assembly);
			}

			foreach (Assembly assembly in assemblyList)
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsNonAbstractClass() && type.Inherits(typeof(MethodExpression)))
					{
                        var attribute = type.GetAttribute<MethodExpressionAttribute>(false);
						if (attribute != null)
						{
							string identifier = attribute.Name;

							// No type name?
							if (string.IsNullOrEmpty(identifier))
							{
								string message = "Method expression of type '{0}' has an empty type name.";
								throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type.FullName));
							}

							// Already exist?
							if (_types.ContainsKey(identifier))
							{
                                string message = "The method expression type name '{0}' is ambiguous.";
								throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, identifier));
							}

							// Add the type to the list.
							_types.Add(identifier, type);
                            _attributes.Add(type, attribute);
						}
					}
				}
			}
        }

        internal ConditionExpression ParseCondition(string condition)
        {
            ConditionTokenBuffer buffer = ConditionTokenizer.Tokenize(condition);
            ConditionParser parser = new ConditionParser(this, buffer);
            return parser.ParseExpression();
        }

		internal MethodExpression BuildMethodExpression(string type, ConditionExpression[] arguments)
		{
			// Check if the method expression type name isn't registered.
			if (!_types.ContainsKey(type))
			{
				string message = "The method expression with type name '{0}' has not been registered.";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type));
			}

            // Get the method epxression attribute.
            MethodExpressionAttribute attribute = _types[type].GetAttribute<MethodExpressionAttribute>(false);
            if (attribute == null)
            {
                string message = "Could not resolve method expression attribute for type '{0}'.";
                throw new ConditionException(string.Format(CultureInfo.InvariantCulture, message, _types[type].FullName));
            }

            // Validate the number of attributes.
            if (arguments.Length != attribute.ArgumentCount && attribute.ArgumentCount != -1)
            {
                string message = "Method expression for type '{0}' expects {1} argument(s) but got {2}.";
                throw new ConditionException(string.Format(
                    CultureInfo.InvariantCulture, message, _types[type].FullName, attribute.ArgumentCount, arguments.Length));
            }

			// Instantiate the method expression.
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            CultureInfo culture = CultureInfo.InvariantCulture;
            object[] parameters = new object[] { arguments };
            MethodExpression expression = Activator.CreateInstance(_types[type], flags, null, parameters, culture) as MethodExpression;
			if (expression == null)
			{
				string message = "Could not instantiate method expression of type '{0}' ({1}).";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, _types[type].FullName, type));
			}

			// Return the sink.
			return expression;
		}
    }
}
