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
using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace BlackBox
{
	internal sealed class LogConfigurationSectionHandler : IConfigurationSectionHandler
	{
		#region IConfigurationSectionHandler Members

		public object Create(object parent, object configContext, XmlNode section)
		{
			// Make sure that we've got a valid configuration.
			if (section == null || string.IsNullOrEmpty(section.OuterXml))
			{
				string message = "The configuration section could not be found.";
				throw new BlackBoxException(message);
			}

			string rootNodeName = section.LocalName;
			if (!rootNodeName.Equals("blackbox", StringComparison.OrdinalIgnoreCase))
			{
				section = section.Rename("BlackBox");
			}

			// Deserialize the XML.
			XDocument document = XDocument.Parse(section.OuterXml);
			return LogConfigurationDeserializer.Deserialize(document);
		}

		#endregion
	}
}
