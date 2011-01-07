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
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;

namespace BlackBox
{
	internal sealed class XmlAssemblyDeserializer
	{
		internal AssemblyCollection Deserialize(XElement element)
		{
			AssemblyCollection assemblies = new AssemblyCollection();
			XElement assembliesElement = element.Element("Assemblies");
			if (assembliesElement != null)
			{
				foreach (XElement assemblyElement in assembliesElement.Elements("Assembly"))
				{
					string assemblyString = assemblyElement.GetAttributeValue("Name");
					if (!string.IsNullOrEmpty(assemblyString))
					{
						Assembly assembly = this.LoadAssembly(assemblyString);
						if (assembly != null)
						{
							assemblies.Add(assembly);
						}
					}
				}
			}
			return assemblies;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private Assembly LoadAssembly(string assemblyString)
		{
			try
			{
				// Load the assembly into the current application domain.
				return Assembly.Load(assemblyString);
			}
			catch (BadImageFormatException exception)
			{
				// Assembly have a bad image format.
				string message = "Could not load assembly '{0}' due to bad image format.";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, assemblyString), exception);
			}
		}
	}
}
