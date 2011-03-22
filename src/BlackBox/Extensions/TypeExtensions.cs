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
using System.Reflection;

namespace BlackBox
{
	internal static class TypeExtensions
	{
		internal static bool IsNonAbstractClass(this Type type)
		{
			return type.IsClass && !type.IsAbstract;
		}

		internal static bool Inherits<T>(this Type type)
		{
			return typeof(T).IsAssignableFrom(type);
		}

		internal static bool Inherits(this Type type, Type inheritedType)
		{
			return inheritedType.IsAssignableFrom(type);
		}

		internal static bool InheritsGenericType(this Type type, Type genericType)
		{
			while (type != typeof(object))
			{
				var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
				if (genericType == cur)
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}

		internal static PropertyInfo[] GetPublicProperties(this Type type)
		{
			return type != null
				? type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				: new List<PropertyInfo>().ToArray();
		}

		internal static PropertyInfo[] GetPublicProperties(this Type type, params string[] ignoredProperties)
		{
			if (type != null)
			{
				return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(property => !ignoredProperties.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
					.ToArray();
			}
			else
			{
				return new List<PropertyInfo>().ToArray();
			}
		}

		internal static PropertyInfo[] GetAllProperties(this Type type)
		{
			HashSet<PropertyInfo> result = new HashSet<PropertyInfo>();

			// Get all public properties
			PropertyInfo[] properties = type.GetPublicProperties();
			foreach (PropertyInfo property in properties)
			{
				result.Add(property);
			}

			// Now get all private properties. 
			while (type != typeof(object) && type != null)
			{
				PropertyInfo[] privateProperties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
				foreach (PropertyInfo property in privateProperties)
				{
					result.Add(property);
				}
				type = type.BaseType;
			}

			return result.ToArray();
		}

		internal static bool HasParameterlessConstructor(this Type type)
		{
			ConstructorInfo[] constructors = type.GetConstructors();
			foreach (ConstructorInfo constructor in constructors)
			{
				if (constructor.GetParameters().Length == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
