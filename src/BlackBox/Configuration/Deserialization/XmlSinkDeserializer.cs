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
	internal sealed class XmlSinkDeserializer
	{
		private readonly LogSinkFactory _factory;
		private readonly XmlFilterDeserializer _filterDeserializer;

		internal XmlSinkDeserializer(IEnumerable<Assembly> assemblies, XmlFilterDeserializer filterDeserializer)
		{
			_factory = new LogSinkFactory(assemblies);
			_filterDeserializer = filterDeserializer;
		}   
		
		internal LogSinkCollection Deserialize(XElement element)
		{
			XElement sinksElement = element.Element("Sinks");
			return this.DeserializeSinks(sinksElement);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		private LogSinkCollection DeserializeSinks(XElement element)
		{
			LogSinkCollection sinks = new LogSinkCollection();

			if (element != null)
			{
				// Deserialize all proxies.
				foreach (XElement proxyElement in element.Elements("Proxy"))
				{
					var proxy = this.DeserializeSink(proxyElement, true);
					if (proxy != null)
					{
						sinks.Add(proxy);
					}
				}

				// Deserialize all sinks.
				foreach (XElement sinkElement in element.Elements("Sink"))
				{
					var sink = this.DeserializeSink(sinkElement, false);
					if (sink != null)
					{
						sinks.Add(sink);
					}
				}
			}
			
			return sinks;
		}

		private LogSink DeserializeSink(XElement element, bool isProxy)
		{
			// Get the "flavor" of the sink. This is used to create meaningful messages.
			string sinkFlavor = isProxy ? "Log sink proxy" : "Log sink";

			// Get the log sink type.
			string type = element.GetAttributeValue("Type");
			if (type.IsNullOrEmpty())
			{                
				string message = "{0} type is missing.";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, sinkFlavor));
			}

			// Parse the arguments
			IDictionary<string, string> arguments = XmlSinkDeserializer.ParseArguments(element);

			// Build the log sink.
			LogSink sink = _factory.Build(type, arguments, isProxy);

			// Parse filters.
			XElement filterElement = element.Element("Filters");
			if (filterElement != null)
			{
				LogFilterCollection filters = _filterDeserializer.DeserializeFilters(filterElement);
				if (filters != null && filters.Count > 0)
				{
					sink.Filters.Clear();
					sink.Filters.AddRange(filters);
				}
			}  

			// Is this a proxy?
			if (isProxy)
			{
				LogSinkProxy proxy = sink as LogSinkProxy;
				if (proxy != null)
				{
					var sinks = this.DeserializeSinks(element);
					if (sinks != null && sinks.Count > 0)
					{
						proxy.Sinks.AddRange(sinks);
					}
				}
			}

			// Return the sink.
			return sink;
		}

		private static IDictionary<string, string> ParseArguments(XElement element)
		{
			var arguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			string[] ignoredAttributes = new string[] { "Type" };
			string[] ignoredElements = new string[] { "Filters", "Sink" };

			// Iterate through all attributes.
			foreach (var attribute in element.Attributes())
			{
				if (!ignoredAttributes.Contains(attribute.Name.LocalName, StringComparer.OrdinalIgnoreCase))
				{
					if (!arguments.ContainsKey(attribute.Name.LocalName))
					{
						arguments.Add(attribute.Name.LocalName, attribute.Value);
					}
				}
			}

			// Iterate through all child elements in the current element.
			foreach (var childElement in element.Elements())
			{
				if (!ignoredElements.Contains(childElement.Name.LocalName, StringComparer.OrdinalIgnoreCase))
				{
					if (!arguments.ContainsKey(childElement.Name.LocalName))
					{
						arguments.Add(childElement.Name.LocalName, childElement.Value);
					}
				}
			}

			return arguments;
		}
	}
}
