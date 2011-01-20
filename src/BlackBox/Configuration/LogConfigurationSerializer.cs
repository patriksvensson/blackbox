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
using System.Xml;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;
using System.Xml.Linq;

namespace BlackBox
{
	internal sealed class LogConfigurationSerializer
	{
		private readonly LogConfiguration _configuration;

		private LogConfigurationSerializer(LogConfiguration configuration)
		{
			_configuration = configuration;
		}

		internal static XDocument Serialize(LogConfiguration configuration)
		{
			LogConfigurationSerializer serializer = new LogConfigurationSerializer(configuration);
			return serializer.Serialize();
		}

		private XDocument Serialize()
		{
			StringBuilder builder = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

			// Create the XML writer.
			using (XmlWriter writer = XmlWriter.Create(builder, settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("BlackBox");

				if (_configuration.Assemblies.Count > 0)
				{
					this.SerializeAssemblies(writer, _configuration.Assemblies);
				}

				if (_configuration.Filters.Count > 0)
				{
					writer.WriteStartElement("Filters");
					this.SerializeFilters(writer, _configuration.Filters);
					writer.WriteEndElement();
				}

				if (_configuration.Sinks.Count > 0)
				{
					writer.WriteStartElement("Sinks");
					this.SerializeSinks(writer, _configuration.Sinks);
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}

			// Return the XML as an XDocument.
			return XDocument.Parse(builder.ToString());
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private void SerializeAssemblies(XmlWriter writer, AssemblyCollection assemblies)
		{
			if (assemblies.Count > 0)
			{
				writer.WriteStartElement("Assemblies");
				foreach(Assembly assembly in assemblies)
				{
					writer.WriteStartElement("Assembly");
					writer.WriteAttributeString("Name", assembly.FullName);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}

		private void SerializeFilters(XmlWriter writer, LogFilterCollection filters)
		{
			if (filters.Count > 0)
			{
				foreach (LogFilter filter in filters)
				{
					LogFilterTypeAttribute attribute = filter.GetFilterType();
					if (attribute == null)
					{
						string message = "Cannot serialize type '{0}' since it's missing a log filter type attribute.";
						throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, filter.GetType().FullName));
					}

					writer.WriteStartElement("Filter");
					writer.WriteAttributeString("Type", attribute.Name);
					this.SerializeProperties(writer, filter);
					writer.WriteEndElement();
				}
			}
		}

		private void SerializeSinks(XmlWriter writer, LogSinkCollection sinks)
		{
			if (sinks.Count > 0)
			{
				foreach (LogSink sink in sinks)
				{
					LogSinkProxy proxy = sink as LogSinkProxy;

					// Make sure we got a log sink attribute.
					LogSinkTypeAttribute attribute = sink.GetLogSinkType();
					if (attribute == null)
					{
						string message = "Cannot serialize type '{0}' since it's missing a log sink type attribute.";
						throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, sink.GetType().FullName));
					}

					// Write the element.
					writer.WriteStartElement(proxy != null ? "Proxy" : "Sink");
					writer.WriteAttributeString("Type", attribute.Name);

					// Write the name if the sink has one.
					if (sink.Name.IsNotNullOrEmpty())
					{
						writer.WriteAttributeString("Name", sink.Name);
					}

					// Serialize properties.
					this.SerializeProperties(writer, sink);

					// Serialize filters.
					this.SerializeFilters(writer, sink.Filters);

					if (proxy != null)
					{
						// Serialize sinks.
						this.SerializeSinks(writer, proxy.Sinks);
					}

					// Write the end of the element.
					writer.WriteEndElement();
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private void SerializeProperties(XmlWriter writer, object obj)
		{
			PropertyInfo[] properties = obj.GetType().GetPublicProperties();
			foreach (PropertyInfo property in properties)
			{
				// Skip properties not meant for serialization.
				if (property.HasAttribute<SkipSerializationAttribute>(true))
				{
					continue;
				}

				// Properties must have both getter and setters.
				if (!property.CanRead || !property.CanWrite)
				{
					continue;
				}

				// Get the type converter for the property type.
				TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
				if (converter != null && converter.CanConvertTo(typeof(string)))
				{
					writer.WriteStartElement(property.Name);
					writer.WriteString(converter.ConvertToInvariantString(property.GetValue(obj, null)));
					writer.WriteEndElement();
				}	
			}
		}
	}
}
