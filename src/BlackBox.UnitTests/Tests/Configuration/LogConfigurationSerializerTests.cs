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
using System.Xml;

namespace BlackBox.UnitTests.Tests.Configuration
{
	[TestFixture]
	public class LogConfigurationSerializerTests
	{
		#region Private Helper Methods

		private V GetFirst<T, V>(IEnumerable<T> source)
			where V : class
		{
			return source.Where(x => x.GetType().Equals(typeof(V))).FirstOrDefault() as V;
		}

		private bool HasInstanceOf<T, V>(IEnumerable<T> source)
		{
			return source.Any(x => x.GetType().Equals(typeof(V)));
		}

		#endregion

		[Test]
		public void LogConfigurationSerializer_SerializeAssemblies()
		{
			// Create a configuration.
			LogConfiguration config = new LogConfiguration();
			config.Assemblies.Add(this.GetType().Assembly);
			config.Assemblies.Add(typeof(string).Assembly);
			config.Assemblies.Add(typeof(XmlReader).Assembly);
			LogConfiguration clonedConfig = config.Clone();

			// Make sure it's the same.
			Assert.AreEqual(3, clonedConfig.Assemblies.Count);
			Assert.IsTrue(clonedConfig.Assemblies.Any(x => x.FullName == this.GetType().Assembly.FullName));
			Assert.IsTrue(clonedConfig.Assemblies.Any(x => x.FullName == typeof(string).Assembly.FullName));
			Assert.IsTrue(clonedConfig.Assemblies.Any(x => x.FullName == typeof(XmlReader).Assembly.FullName));
		}

		[Test]
		public void LogConfigurationSerializer_SerializeFilters()
		{
			// Create a configuration.
			LogConfiguration config = new LogConfiguration();
			config.Filters.Add(new ConditionFilter { Condition = "has-exception==true && message!=''", Action = LogFilterResult.Filter });
			config.Filters.Add(new LevelMatchFilter { Level = LogLevel.Warning, Action = LogFilterResult.Accept });
			LogConfiguration clonedConfig = config.Clone();

			// Make sure it's the same.
			Assert.AreEqual(2, clonedConfig.Filters.Count);
			Assert.IsTrue(this.HasInstanceOf<LogFilter, ConditionFilter>(clonedConfig.Filters));
			Assert.IsTrue(this.HasInstanceOf<LogFilter, LevelMatchFilter>(clonedConfig.Filters));
			Assert.AreEqual("has-exception==true && message!=''", this.GetFirst<LogFilter, ConditionFilter>(clonedConfig.Filters).Condition);
			Assert.AreEqual(LogFilterResult.Filter, this.GetFirst<LogFilter, ConditionFilter>(clonedConfig.Filters).Action);
			Assert.AreEqual(LogLevel.Warning, this.GetFirst<LogFilter, LevelMatchFilter>(clonedConfig.Filters).Level);
			Assert.AreEqual(LogFilterResult.Accept, this.GetFirst<LogFilter, LevelMatchFilter>(clonedConfig.Filters).Action);
		}

		[Test]
		public void LogConfigurationSerializer_SerializeSinks()
		{
			// Create a configuration.
			LogConfiguration config = new LogConfiguration();
			config.Sinks.Add(new ConsoleSink { Format = "$(time(format='HH:mm:ss.fff')) $(message())" });
			config.Sinks.Add(new MessageQueueSink { Queue = @".\Private$\BlackBox", CreateIfNotExists = true });
			LogConfiguration clonedConfig = config.Clone();

			// Make sure it's the same.
			Assert.AreEqual(2, clonedConfig.Sinks.Count);
			Assert.IsTrue(this.HasInstanceOf<LogSink, ConsoleSink>(clonedConfig.Sinks));
			Assert.AreEqual("$(time(format='HH:mm:ss.fff')) $(message())", this.GetFirst<LogSink, ConsoleSink>(clonedConfig.Sinks).Format);
			Assert.IsTrue(this.HasInstanceOf<LogSink, MessageQueueSink>(clonedConfig.Sinks));
			Assert.AreEqual(@".\Private$\BlackBox", this.GetFirst<LogSink, MessageQueueSink>(clonedConfig.Sinks).Queue);
			Assert.AreEqual(true, this.GetFirst<LogSink, MessageQueueSink>(clonedConfig.Sinks).CreateIfNotExists);
		}

		[Test]
		public void LogConfigurationSerializer_SerializeSinkWithFilter()
		{
			// Create a configuration.
			LogConfiguration config = new LogConfiguration();
			ConsoleSink sink = new ConsoleSink { Format = "$(time(format='HH:mm:ss.fff')) $(message())" };
			sink.Filters.Add(new LevelMatchFilter { Level = LogLevel.Warning, Action = LogFilterResult.Accept });
			config.Sinks.Add(sink);
			LogConfiguration clonedConfig = config.Clone();

			// Make sure it's the same.
			Assert.AreEqual(1, clonedConfig.Sinks.Count);
			Assert.IsTrue(this.HasInstanceOf<LogSink, ConsoleSink>(clonedConfig.Sinks));
			ConsoleSink consoleSink = this.GetFirst<LogSink, ConsoleSink>(clonedConfig.Sinks);
			Assert.AreEqual("$(time(format='HH:mm:ss.fff')) $(message())", consoleSink.Format);
			Assert.IsTrue(this.HasInstanceOf<LogFilter, LevelMatchFilter>(consoleSink.Filters));
			Assert.AreEqual(LogLevel.Warning, this.GetFirst<LogFilter, LevelMatchFilter>(consoleSink.Filters).Level);
			Assert.AreEqual(LogFilterResult.Accept, this.GetFirst<LogFilter, LevelMatchFilter>(consoleSink.Filters).Action);
		}

		[Test]
		public void LogConfigurationSerializer_SerializeProxyWithSink()
		{
			// Create a configuration.
			LogConfiguration config = new LogConfiguration();
			FunnelProxy proxy = new FunnelProxy();
			proxy.Sinks.Add(new FileSink() { Name = "file1", FileName = "$(level()).log" });
			config.Sinks.Add(proxy);
			LogConfiguration clonedConfig = config.Clone();

			// Make sure it's the same.
			Assert.AreEqual(1, clonedConfig.Sinks.Count);
			Assert.IsTrue(this.HasInstanceOf<LogSink, FunnelProxy>(clonedConfig.Sinks));
			FunnelProxy funnel = this.GetFirst<LogSink, FunnelProxy>(clonedConfig.Sinks);
			Assert.AreEqual(1, funnel.Sinks.Count);
			Assert.IsTrue(this.HasInstanceOf<LogSink, FileSink>(funnel.Sinks));
			Assert.AreEqual("file1", this.GetFirst<LogSink, FileSink>(funnel.Sinks).Name);
			Assert.AreEqual("$(level()).log", this.GetFirst<LogSink, FileSink>(funnel.Sinks).FileName);
		}

		[Test]
		public void LogConfigurationSerializer_SerializeProxyWithFilter()
		{
			// Create a configuration.
			LogConfiguration config = new LogConfiguration();
			FunnelProxy proxy = new FunnelProxy();
			proxy.Filters.Add(new LevelMatchFilter { Level = LogLevel.Warning, Action = LogFilterResult.Accept });
			config.Sinks.Add(proxy);
			LogConfiguration clonedConfig = config.Clone();

			// Make sure it's the same.
			Assert.AreEqual(1, clonedConfig.Sinks.Count);
			Assert.IsTrue(this.HasInstanceOf<LogSink, FunnelProxy>(clonedConfig.Sinks));
			FunnelProxy funnel = this.GetFirst<LogSink, FunnelProxy>(clonedConfig.Sinks);
			Assert.IsTrue(this.HasInstanceOf<LogFilter, LevelMatchFilter>(funnel.Filters));
			Assert.AreEqual(LogLevel.Warning, this.GetFirst<LogFilter, LevelMatchFilter>(funnel.Filters).Level);
			Assert.AreEqual(LogFilterResult.Accept, this.GetFirst<LogFilter, LevelMatchFilter>(funnel.Filters).Action);
		}
	}
}
