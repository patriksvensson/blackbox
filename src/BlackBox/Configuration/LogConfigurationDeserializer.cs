using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;
using System.Reflection;

namespace BlackBox
{
    internal sealed class LogConfigurationDeserializer
    {
        private XDocument _document;

        internal LogConfigurationDeserializer(XDocument document)
        {
            _document = document;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
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
            XElement filterRoot = sinkElement.Element("Filters");
            if (filterRoot != null)
            {
                LogFilterCollection filters = this.DeserializeFilters(filterRoot, filterFactory);
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
        private IDictionary<string, string> ParseArguments(XElement element, string[] ignoredAttributes, string[] ignoredElements)
        {
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
    }
}
