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
using System.Globalization;

namespace BlackBox
{
    internal sealed class XmlConfigurationDeserializer
    {
        private readonly XDocument _document;

        internal XmlConfigurationDeserializer(XDocument document)
        {
            _document = document;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "BlackBox"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        internal LogConfiguration Deserialize()
        {
            // Create the log configuration.
            LogConfiguration configuration = new LogConfiguration();

            // Parse the main element.
            XElement rootElement = _document.Element("BlackBox");
            if (rootElement == null)
            {
                string message = "Could not find start element 'BlackBox' in configuration.";
                throw new BlackBoxException(message);
            }

            // Parse the assemblies in the configuration file.
            XmlAssemblyDeserializer assemblyDeserializer = new XmlAssemblyDeserializer();
            var assemblies = assemblyDeserializer.Deserialize(rootElement);
            if (assemblies != null && assemblies.Count > 0)
            {
                configuration.Assemblies.AddRange(assemblies);
            }

            // Parse all global filters in the configuration file.
            XmlFilterDeserializer filterDeserializer = new XmlFilterDeserializer(assemblies);
            LogFilterCollection filters = filterDeserializer.Deserialize(rootElement);
            if (filters != null && filters.Count > 0)
            {
                configuration.Filters.AddRange(filters);
            }

            // Parse all sinks in the configuration file.
            XmlSinkDeserializer sinkDeserializer = new XmlSinkDeserializer(assemblies, filterDeserializer);
            LogSinkCollection sinks = sinkDeserializer.Deserialize(rootElement);
            if (sinks != null && sinks.Count > 0)
            {
                configuration.Sinks.AddRange(sinks);
            }

            // Return the log configuration.
            return configuration;
        }
    }
}
