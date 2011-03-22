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

using System.Xml;

namespace BlackBox
{
	internal static class XmlNodeExtensions
	{
		internal static XmlElement Rename(this XmlNode node, string name)
		{
			XmlDocument doc = node.OwnerDocument;

			// Create the new element.
			XmlElement newElement = doc.CreateElement(name);
			while (node.HasChildNodes)
			{
				newElement.AppendChild(node.FirstChild);
			}

			// Add all attributes to the new node.
			XmlAttributeCollection attributes = node.Attributes;
			while (attributes.Count > 0)
			{
				newElement.Attributes.Append(attributes[0]);
			}

			// Insert the new element in the parent node.
			XmlNode parentNode = node.ParentNode;
			parentNode.ReplaceChild(newElement, node);

			// Return the new element.
			return newElement;
		}
	}
}
