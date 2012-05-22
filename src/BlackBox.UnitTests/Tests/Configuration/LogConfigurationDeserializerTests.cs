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

using System.Xml.Linq;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Configuration
{
	[TestFixture]
	public class LogConfigurationDeserializerTests
	{
		[Test]
		public void LogConfigurationDeserializer_ParseEmpty()
		{
			string xml = "<BlackBox></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration.Assemblies);
			Assert.IsNotNull(configuration.Sinks);
			Assert.IsNotNull(configuration.Filters);
		}

		[Test]
		public void LogConfigurationDeserializer_LoadAssembly()
		{
			string assemblyName = "System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
			string xml = "<BlackBox><Assemblies><Assembly Name=\"" + assemblyName + "\" /></Assemblies></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.AreEqual(1, configuration.Assemblies.Count);
			Assert.AreEqual(assemblyName, configuration.Assemblies[0].FullName);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseSink()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""Console"" /></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(ConsoleSink), configuration.Sinks[0]);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseMissingSink()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""Unknown"" /></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
            Assert.That(() => LogConfigurationDeserializer.Deserialize(document),
                Throws.Exception.TypeOf<BlackBoxException>()
                .With.Property("Message").EqualTo("Log sink with type name 'Unknown' has not been registered."));
		}

		[Test]
		public void LogConfigurationDeserializer_ParseProxy()
		{
			string xml = @"<BlackBox><Sinks>
				<Proxy Type=""Funnel""><Sink Type=""Console"" /><Sink Type=""Debug"" /></Proxy>
				</Sinks></BlackBox>";

			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(FunnelProxy), configuration.Sinks[0]);
			Assert.AreEqual(2, ((FunnelProxy)configuration.Sinks[0]).Sinks.Count);
			Assert.IsInstanceOf(typeof(ConsoleSink), ((FunnelProxy)configuration.Sinks[0]).Sinks[0]);
			Assert.IsInstanceOf(typeof(DebugSink), ((FunnelProxy)configuration.Sinks[0]).Sinks[1]);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseMissingProxy()
		{
			string xml = @"<BlackBox><Sinks><Proxy Type=""Unknown"" /></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
            Assert.That(() => LogConfigurationDeserializer.Deserialize(document),
                Throws.Exception.TypeOf<BlackBoxException>()
                .With.Property("Message").EqualTo("Log sink proxy with type name 'Unknown' has not been registered."));
		}

		[Test]
		public void LogConfigurationDeserializer_ParseSinkWithElementProperty()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""MSMQ""><Queue>$\Shared</Queue></Sink></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(MessageQueueSink), configuration.Sinks[0]);
			Assert.AreEqual(@"$\Shared", ((MessageQueueSink)configuration.Sinks[0]).Queue);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseSinkWithAttributeProperty()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""MSMQ"" Queue=""$\Shared"" /></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(MessageQueueSink), configuration.Sinks[0]);
			Assert.AreEqual(@"$\Shared", ((MessageQueueSink)configuration.Sinks[0]).Queue);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseFilter()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch"" /></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseMissingFilter()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""Unknown"" /></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
            Assert.That(() => LogConfigurationDeserializer.Deserialize(document),
                Throws.Exception.TypeOf<BlackBoxException>()
                .With.Property("Message").EqualTo("Log filter with type name 'Unknown' has not been registered."));
		}

		[Test]
		public void LogConfigurationDeserializer_ParseFiltersWithPropertiesAsAttributes()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch"" Level=""Error"" /></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
			Assert.AreEqual(LogLevel.Error, ((LevelMatchFilter)configuration.Filters[0]).Level);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseFiltersWithPropertiesAsAttributes_PropertyDoNotExist()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch"" SomeProperty=""Fatal"" /></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
		}

		[Test]
		public void LogConfigurationDeserializer_LogFilterResultCanBeParsed()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch"" Action=""Accept"" /></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.AreEqual(LogFilterResult.Accept, ((LevelMatchFilter)configuration.Filters[0]).Action);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseFiltersWithPropertiesAsChildElements()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch""><Level>Error</Level></Filter></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
			Assert.AreEqual(LogLevel.Error, ((LevelMatchFilter)configuration.Filters[0]).Level);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseFiltersWithPropertiesAsChildElements_PropertyDoNotExist()
		{
			string xml = @"<BlackBox><Filters><Filter Type=""LevelMatch""><SomeProperty>Fatal</SomeProperty></Filter></Filters></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Filters[0]);
		}

		[Test]
		public void LogConfigurationDeserializer_ParseSinkWithFilter()
		{
			string xml = @"<BlackBox><Sinks><Sink Type=""Console""><Filter Type=""LevelMatch"" /></Sink></Sinks></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.IsInstanceOf(typeof(ConsoleSink), configuration.Sinks[0]);
			Assert.AreEqual(1, configuration.Sinks[0].Filters.Count);
			Assert.IsInstanceOf(typeof(LevelMatchFilter), configuration.Sinks[0].Filters[0]);
		}

		[Test]
		public void LogConfigurationSerializer_InternalLoggerSettingsCanBeDeserialized()
		{
			string xml = @"<BlackBox><InternalLogger><Enabled>True</Enabled><Threshold>Warning</Threshold></InternalLogger></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsTrue(configuration.InternalLogger.Enabled);
			Assert.AreEqual(LogLevel.Warning, configuration.InternalLogger.Threshold);
		}

		[Test]
		public void LogConfigurationSerializer_InternalLoggerSettingsAsAttributesCanBeDeserialized()
		{
			string xml = @"<BlackBox><InternalLogger Enabled=""True"" Threshold=""Warning""></InternalLogger></BlackBox>";
			XDocument document = XDocument.Parse(xml);
			var configuration = LogConfigurationDeserializer.Deserialize(document);
			Assert.IsTrue(configuration.InternalLogger.Enabled);
			Assert.AreEqual(LogLevel.Warning, configuration.InternalLogger.Threshold);
		}
	}
}
