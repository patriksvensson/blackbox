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
using System.Configuration;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace BlackBox
{
	/// <summary>
	/// Represents a log configuration.
	/// </summary>
	public sealed class LogConfiguration : IDisposable
	{
		private readonly LogSinkCollection _sinks;
		private readonly AssemblyCollection _assemblies;
		private readonly LogFilterCollection _filters;
		private bool _disposed;

		#region Properties

		/// <summary>
		/// Gets the log sinks.
		/// </summary>
		/// <value>The log sinks.</value>
		public LogSinkCollection Sinks
		{
			get { return _sinks; }
		}

		/// <summary>
		/// Gets the assemblies.
		/// </summary>
		/// <value>The assemblies.</value>
		public AssemblyCollection Assemblies
		{
			get { return _assemblies; }
		}

		/// <summary>
		/// Gets the log filters.
		/// </summary>
		/// <value>The log filters.</value>
		public LogFilterCollection Filters
		{
			get { return _filters; }
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="LogConfiguration"/> class.
		/// </summary>
		public LogConfiguration()
		{
			_sinks = new LogSinkCollection();
			_assemblies = new AssemblyCollection();
			_filters = new LogFilterCollection();
		}

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_sinks.Dispose();
				}
				_disposed = true;
			}
		}

		#endregion

		#region Log Configuration Loading

		/// <summary>
		/// Loads a log configuration the section 'BlackBox' in App.config or Web.config.
		/// </summary>
		/// <returns></returns>
		public static LogConfiguration FromConfigSection()
		{
			return ConfigurationManager.GetSection("BlackBox") as LogConfiguration;
		}

		/// <summary>
		/// Loads a log configuration from the specified section in App.config or Web.config.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <returns></returns>
		public static LogConfiguration FromConfigSection(string section)
		{
			return ConfigurationManager.GetSection(section) as LogConfiguration;
		}

		/// <summary>
		/// Loads a log configuration from XML in a <see cref="System.String"/>.
		/// </summary>
		/// <param name="xml">The XML.</param>
		/// <returns></returns>
		public static LogConfiguration FromXml(string xml)
		{
			if (string.IsNullOrEmpty(xml))
			{
				throw new ArgumentException("The BlackBox configuration XML cannot be empty.", "xml");
			}

			try
			{
				XDocument document = XDocument.Parse(xml);
				return LogConfigurationDeserializer.Deserialize(document);
			}
			catch (XmlException exception)
			{
				string message = string.Format("Could not parse configuration XML. {0}", exception.Message);
				throw new BlackBoxException(message, exception);
			}
		}

		/// <summary>
		/// Loads a log configuration from  a <see cref="System.Xml.Linq.XDocument"/>
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns></returns>
		public static LogConfiguration FromXml(XDocument document)
		{
			return LogConfigurationDeserializer.Deserialize(document);
		}

		/// <summary>
		/// Loads a log configuration from a <see cref="System.Xml.XmlReader"/>.
		/// </summary>
		/// <param name="reader">The XML reader.</param>
		/// <returns></returns>
		public static LogConfiguration FromXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}

			XDocument document = XDocument.Load(reader);
			return LogConfigurationDeserializer.Deserialize(document);
		}

		/// <summary>
		/// Loads a log configuration from a <see cref="System.IO.FileInfo"/>.
		/// </summary>
		/// <param name="file">The XML file.</param>
		/// <returns></returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		public static LogConfiguration FromXml(FileInfo file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}

			using (TextReader reader = new StreamReader(file.FullName))
			{
				try
				{
					XDocument document = XDocument.Load(reader);
					return LogConfigurationDeserializer.Deserialize(document);
				}
				catch (XmlException exception)
				{
					string message = string.Format("Could not parse configuration XML. {0}", exception.Message);
					throw new BlackBoxException(message, exception);
				}
			}
		}

		#endregion

        #region Log Configuration Saving

        /// <summary>
        /// Saves the log configuration to the specified <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}

            XDocument document = LogConfigurationSerializer.Serialize(this);
			XmlWriterSettings settings = new XmlWriterSettings { Indent = true, IndentChars = "\t" };
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                document.Save(writer);
            }
        }

        /// <summary>
        /// Saves the log configuration to the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void Save(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            XDocument document = LogConfigurationSerializer.Serialize(this);
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true, IndentChars = "\t" };
            using (XmlWriter writer = XmlWriter.Create(file.FullName, settings))
            {
                document.Save(writer);
            }
        }

        #endregion

        internal LogConfiguration Clone()
		{
			// Serialize the configuration.
			XDocument document = LogConfigurationSerializer.Serialize(this);
			if (document == null) 
			{
				string message = "Could not clone log configuration.";
				throw new BlackBoxException(message);
			}

			// Return a deserialized copy.
			return LogConfigurationDeserializer.Deserialize(document);
		}
	}
}
