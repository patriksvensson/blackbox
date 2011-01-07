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
using NUnit.Framework;
using System.Xml.Linq;

namespace BlackBox.UnitTests.Tests.Configuration
{
	[TestFixture]
	public class XmlConfigurationDeserializerTests
	{
		[Test]
		public void XmlConfigurationDeserializer_ParseEmpty()
		{
			string xml = "<BlackBox></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration.Assemblies);
			Assert.IsNotNull(configuration.Sinks);
			Assert.IsNotNull(configuration.Filters);
		}

		[Test]
		public void XmlConfigurationDeserializer_LoadAssembly()
		{
			string assemblyName = "System.Runtime.Caching, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			string xml = "<BlackBox><Assemblies><Assembly Name=\"" + assemblyName + "\" /></Assemblies></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.AreEqual(1, configuration.Assemblies.Count);
			Assert.AreEqual(assemblyName, configuration.Assemblies[0].FullName);
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseSink()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""Console"" /></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(ConsoleSink), configuration.Sinks[0]);
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(BlackBoxException), ExpectedMessage = "Log sink with type name 'Unknown' has not been registered.")]
		public void XmlConfigurationDeserializer_ParseMissingSink()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""Unknown"" /></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseProxy()
		{
			string xml = @"<BlackBox><Sinks>
				<Proxy Type=""Funnel""><Sink Type=""Console"" /><Sink Type=""Debug"" /></Proxy>
				</Sinks></BlackBox>";

			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(FunnelProxy), configuration.Sinks[0]);
			Assert.AreEqual(2, ((FunnelProxy)configuration.Sinks[0]).Sinks.Count);
			Assert.IsInstanceOf(typeof(ConsoleSink), ((FunnelProxy)configuration.Sinks[0]).Sinks[0]);
			Assert.IsInstanceOf(typeof(DebugSink), ((FunnelProxy)configuration.Sinks[0]).Sinks[1]);
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(BlackBoxException), ExpectedMessage = "Log sink proxy with type name 'Unknown' has not been registered.")]
		public void XmlConfigurationDeserializer_ParseMissingProxy()
		{
			string xml = @"<BlackBox><Sinks><Proxy Type=""Unknown"" /></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseSinkWithElementProperty()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""MSMQ""><Queue>$\Shared</Queue></Sink></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(MessageQueueSink), configuration.Sinks[0]);
			Assert.AreEqual(@"$\Shared", ((MessageQueueSink)configuration.Sinks[0]).Queue);
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseSinkWithAttributeProperty()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""MSMQ"" Queue=""$\Shared"" /></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(MessageQueueSink), configuration.Sinks[0]);
			Assert.AreEqual(@"$\Shared", ((MessageQueueSink)configuration.Sinks[0]).Queue);
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseFilter()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch"" /></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
		}

		[Test]
		[ExpectedException(ExpectedException=typeof(BlackBoxException), ExpectedMessage="Log filter with type name 'Unknown' has not been registered.")]
		public void XmlConfigurationDeserializer_ParseMissingFilter()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""Unknown"" /></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseFiltersWithPropertiesAsAttributes()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch"" Threshold=""Fatal"" /></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
			Assert.AreEqual(LogLevel.Fatal, ((LevelMatchFilter)configuration.Filters[0]).Level);
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseFiltersWithPropertiesAsAttributes_PropertyDoNotExist()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch"" SomeProperty=""Fatal"" /></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseFiltersWithPropertiesAsChildElements()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch""><Threshold>Fatal</Threshold></Filter></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
			Assert.AreEqual(LogLevel.Fatal, ((LevelMatchFilter)configuration.Filters[0]).Level);
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseFiltersWithPropertiesAsChildElements_PropertyDoNotExist()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch""><SomeProperty>Fatal</SomeProperty></Filter></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
		}

		[Test]
		public void XmlConfigurationDeserializer_ParseSinkWithFilter()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""Console""><Filters><Filter Type=""LevelMatch"" /></Filters></Sink></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = new XmlConfigurationDeserializer(document).Deserialize();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(ConsoleSink), configuration.Sinks[0]);
			Assert.AreEqual(1, configuration.Sinks[0].Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Sinks[0].Filters[0]);
		}
	}
}
