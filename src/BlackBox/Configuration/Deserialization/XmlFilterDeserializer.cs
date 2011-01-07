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
using System.Xml.Linq;
using System.Globalization;

namespace BlackBox
{
    internal sealed class XmlFilterDeserializer
    {
        private readonly LogFilterFactory _factory;

        internal XmlFilterDeserializer(IEnumerable<Assembly> assemblies)
        {
            _factory = new LogFilterFactory(assemblies);
        }

        internal LogFilterCollection Deserialize(XElement element)
        {
            XElement sinksElement = element.Element("Filters");
            return this.DeserializeFilters(sinksElement);
        }

        internal LogFilterCollection DeserializeFilters(XElement element)
        {
            // Create the collection.
            LogFilterCollection collection = new LogFilterCollection();

            // Got a element?
            if (element != null)
            {
                // Load all filters.
                foreach (XElement filterElement in element.Elements("Filter"))
                {
                    LogFilter filter = this.ParseFilter(filterElement);
                    if (filter != null)
                    {
                        collection.Add(filter);
                    }
                }
            }

            // Return the collection.
            return collection;
        }

        private LogFilter ParseFilter(XElement element)
        {
            // Get the log sink type.
            string type = element.GetAttributeValue("Type");
            if (type.IsNullOrEmpty())
            {
                string message = "Filter type is missing.";
                throw new BlackBoxException(message);
            }

            // Parse the arguments.
            IDictionary<string, string> arguments = XmlFilterDeserializer.ParseArguments(element);

            // Build the filter.
            var filter = _factory.Build(type, arguments);
            if (filter == null)
            {
                string message = "Could not build log filter '{0}'.";
                throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type));
            }

            // Return the filter.
            return filter;
        }

        private static IDictionary<string, string> ParseArguments(XElement filterElement)
        {
            var arguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string[] ignoredAttributes = new string[] { "Type" };

            // Iterate through all attributes.
            foreach (var attribute in filterElement.Attributes())
            {
                // Not part of the ignored attributes?
                if (!ignoredAttributes.Contains(attribute.Name.LocalName, StringComparer.OrdinalIgnoreCase))
                {
                    if (!arguments.ContainsKey(attribute.Name.LocalName))
                    {
                        arguments.Add(attribute.Name.LocalName, attribute.Value);
                    }
                }
            }

            // Iterate through all elements in the current element.
            foreach (var element in filterElement.Elements())
            {
                if (!arguments.ContainsKey(element.Name.LocalName))
                {
                    arguments.Add(element.Name.LocalName, element.Value);
                }
            }

            return arguments;
        }
    }
}
