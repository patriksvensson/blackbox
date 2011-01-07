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
	public sealed class LogConfiguration : IDisposable
	{
		private readonly LogSinkCollection _sinks;
		private readonly AssemblyCollection _assemblies;
		private readonly LogFilterCollection _filters;
		private bool _disposed;

		#region Properties

		public LogSinkCollection Sinks
		{
			get { return _sinks; }
		}

		public AssemblyCollection Assemblies
		{
			get { return _assemblies; }
		}

		public LogFilterCollection Filters
		{
			get { return _filters; }
		}

		#endregion

		public LogConfiguration()
		{
			_sinks = new LogSinkCollection();
			_assemblies = new AssemblyCollection();
			_filters = new LogFilterCollection();
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

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

		public static LogConfiguration FromConfigSection()
		{
			return ConfigurationManager.GetSection("BlackBox") as LogConfiguration;
		}

		public static LogConfiguration FromConfigSection(string section)
		{
			return ConfigurationManager.GetSection(section) as LogConfiguration;
		}

		public static LogConfiguration FromXml(string xml)
		{
			if (string.IsNullOrEmpty(xml))
			{
				throw new ArgumentException("The BlackBox configuration XML cannot be empty.", "xml");
			}

			XDocument document = XDocument.Parse(xml);
			XmlConfigurationDeserializer deserializer = new XmlConfigurationDeserializer(document);
			return deserializer.Deserialize();
		}

		public static LogConfiguration FromXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}

			XDocument document = XDocument.Load(reader);
			XmlConfigurationDeserializer deserializer = new XmlConfigurationDeserializer(document);
			return deserializer.Deserialize();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		public static LogConfiguration FromXml(FileInfo file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}

			using (TextReader reader = new StreamReader(file.FullName))
			{
				XDocument document = XDocument.Load(reader);
				XmlConfigurationDeserializer deserializer = new XmlConfigurationDeserializer(document);
				return deserializer.Deserialize();
			}
		}
	}
}
