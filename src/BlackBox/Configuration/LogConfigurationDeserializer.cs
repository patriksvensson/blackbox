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
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.ComponentModel;

namespace BlackBox
{
	internal sealed class LogConfigurationDeserializer
	{
		private XDocument _document;

		private LogConfigurationDeserializer(XDocument document)
		{
			_document = document;
		}

		internal static LogConfiguration Deserialize(XDocument document)
		{
			LogConfigurationDeserializer deserializer = new LogConfigurationDeserializer(document);
			return deserializer.Deserialize();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		private LogConfiguration Deserialize()
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

			// Parse the internal logger settings in the configuration file.
			XElement internalLoggerRoot = rootElement.Element("InternalLogger");
			if (internalLoggerRoot != null)
			{
				this.DeserializeInternalLoggerSettings(internalLoggerRoot, configuration);
			}

			// Parse the assemblies in the configuration file.
			AssemblyCollection assemblies = null;
			XElement assemblyRoot = rootElement.Element("Assemblies");
			if (assemblyRoot != null)
			{
				assemblies = this.DeserializeAssemblies(assemblyRoot);
				if (assemblies != null && assemblies.Count > 0)
				{
					configuration.Assemblies.AddRange(assemblies);
				}
			}

			// Parse all global filters in the configuration file.
			LogFilterFactory filterFactory = new LogFilterFactory(assemblies);
			XElement filterRoot = rootElement.Element("Filters");
			if (filterRoot != null)
			{
				LogFilterCollection filters = this.DeserializeFilters(filterRoot, filterFactory);
				if (filters != null && filters.Count > 0)
				{
					configuration.Filters.AddRange(filters);
				}
			}

			// Parse all sinks in the configuration file.
			XElement sinkRoot = rootElement.Element("Sinks");
			if (sinkRoot != null)
			{
				LogSinkFactory sinkFactory = new LogSinkFactory(assemblies);
				LogSinkCollection sinks = this.DeserializeSinks(sinkRoot, sinkFactory, filterFactory);
				if (sinks != null && sinks.Count > 0)
				{
					configuration.Sinks.AddRange(sinks);
				}
			}

			// Return the configuration.
			return configuration;
		}

		private void DeserializeInternalLoggerSettings(XElement internalLoggerRoot, LogConfiguration configuration)
		{
			// Parse any arguments.
			IDictionary<string, string> arguments = this.ParseArguments(internalLoggerRoot);
			if (arguments != null)
			{
				// Map the arguments to the logger.
				this.MapArguments(configuration.InternalLogger, arguments);
			}
		}

		#region Assembly Deserialization

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		internal AssemblyCollection DeserializeAssemblies(XElement assembliesElement)
		{
			AssemblyCollection assemblies = new AssemblyCollection();
			if (assembliesElement != null)
			{
				foreach (XElement assemblyElement in assembliesElement.Elements("Assembly"))
				{
					string assemblyString = assemblyElement.GetAttributeValue("Name");
					if (!string.IsNullOrEmpty(assemblyString))
					{
						try
						{
							Assembly assembly = Assembly.Load(assemblyString);
							if (assembly != null)
							{
								assemblies.Add(assembly);
							}
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
			return assemblies;
		}

		#endregion

		#region Filter Deserialization

		internal LogFilterCollection DeserializeFilters(XElement filterRoot, LogFilterFactory filterFactory)
		{
			// Create the collection.
			LogFilterCollection collection = new LogFilterCollection();

			// Load all filters.
			foreach (XElement filterElement in filterRoot.Elements("Filter"))
			{
				LogFilter filter = this.DeserializeFilter(filterElement, filterFactory);
				if (filter != null)
				{
					collection.Add(filter);
				}
			}

			// Return the collection.
			return collection;
		}

		private LogFilter DeserializeFilter(XElement element, LogFilterFactory filterFactory)
		{
			// Get the log sink type.
			string type = element.GetAttributeValue("Type");
			if (type.IsNullOrEmpty())
			{
				string message = "Filter type is missing.";
				throw new BlackBoxException(message);
			}

			// Parse the arguments.
			IDictionary<string, string> arguments = this.ParseArguments(element,
				new string[] { "Type" }, new string[] { });

			// Build the filter.
			var filter = filterFactory.Build(type, arguments);
			if (filter == null)
			{
				string message = "Could not build log filter '{0}'.";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, type));
			}

			// Return the filter.
			return filter;
		}

		#endregion

		#region Sink Deserialization

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		private LogSinkCollection DeserializeSinks(XElement sinkRoot, LogSinkFactory sinkFactory, LogFilterFactory filterFactory)
		{
			LogSinkCollection sinks = new LogSinkCollection();

			if (sinkRoot != null)
			{
				// Deserialize all proxies.
				foreach (XElement proxyElement in sinkRoot.Elements("Proxy"))
				{
					var proxy = this.DeserializeSink(proxyElement, sinkFactory, filterFactory, true);
					if (proxy != null)
					{
						sinks.Add(proxy);
					}
				}

				// Deserialize all sinks.
				foreach (XElement sinkElement in sinkRoot.Elements("Sink"))
				{
					var sink = this.DeserializeSink(sinkElement, sinkFactory, filterFactory, false);
					if (sink != null)
					{
						sinks.Add(sink);
					}
				}
			}

			return sinks;
		}

		private LogSink DeserializeSink(XElement sinkElement, LogSinkFactory sinkFactory, LogFilterFactory filterFactory, bool isProxy)
		{
			// Get the "flavor" of the sink. This is used to create meaningful messages.
			string sinkFlavor = isProxy ? "Log sink proxy" : "Log sink";

			// Get the log sink type.
			string type = sinkElement.GetAttributeValue("Type");
			if (type.IsNullOrEmpty())
			{
				string message = "{0} type is missing.";
				throw new BlackBoxException(string.Format(CultureInfo.InvariantCulture, message, sinkFlavor));
			}

			// Parse the arguments
			IDictionary<string, string> arguments = this.ParseArguments(sinkElement,
				new string[] { "Type" }, new string[] { "Filters", "Sink" });

			// Build the log sink.
			LogSink sink = sinkFactory.Build(type, arguments, isProxy);

			// Parse filters.
			LogFilterCollection filters = this.DeserializeFilters(sinkElement, filterFactory);
			if (filters != null && filters.Count > 0)
			{
				sink.Filters.Clear();
				sink.Filters.AddRange(filters);
			}

			// Is this a proxy?
			if (isProxy)
			{
				LogSinkProxy proxy = sink as LogSinkProxy;
				if (proxy != null)
				{
					var sinks = this.DeserializeSinks(sinkElement, sinkFactory, filterFactory);
					if (sinks != null && sinks.Count > 0)
					{
						proxy.Sinks.AddRange(sinks);
					}
				}
			}

			// Return the sink.
			return sink;
		}

		#endregion

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private IDictionary<string, string> ParseArguments(XElement element, string[] ignoredAttributes = null, string[] ignoredElements = null)
		{
			ignoredAttributes = ignoredAttributes ?? new string[0];
			ignoredElements = ignoredElements ?? new string[0];

			var arguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private void MapArguments<T>(T obj, IDictionary<string, string> arguments)
			where T : class
		{
			#region Guard Statements
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			#endregion

			// Get identifier to property mappings.
			var mappings = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
			foreach (PropertyInfo property in obj.GetType().GetAllProperties())
			{
				mappings.Add(property.Name, property);
			}

			// Iterate through all keys in the argument dictionary.
			foreach (KeyValuePair<string, string> argument in arguments)
			{
				// Got a property mapping?
				if (mappings.ContainsKey(argument.Key))
				{
					// Get the property's type.
					Type propertyType = mappings[argument.Key].PropertyType;

					// Get the type converter.
					var typeConverter = TypeDescriptor.GetConverter(propertyType);
					if (typeConverter != null && typeConverter.CanConvertFrom(typeof(string)))
					{
						object value = typeConverter.ConvertFromInvariantString(argument.Value);
						mappings[argument.Key].SetValue(obj, value, null);
					}
				}
			}
		}
	}
}
