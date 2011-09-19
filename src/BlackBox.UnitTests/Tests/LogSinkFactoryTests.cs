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

using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests
{
	[TestFixture]
	public class LogSinkFactoryTests
	{
		[Test]
		public void LogSinkFactory_CreateFactoryWithoutAssemblies()
		{
			LogSinkFactory factory = new LogSinkFactory();
			Assert.IsNotNull(factory.Types);
			Assert.AreEqual(10, factory.Types.Count);

			// The standard sinks should be available.
			Assert.IsTrue(factory.Types.ContainsKey("funnel"));
			Assert.IsTrue(factory.Types.ContainsKey("buffer"));
			Assert.IsTrue(factory.Types.ContainsKey("console"));
			Assert.IsTrue(factory.Types.ContainsKey("debug"));
			Assert.IsTrue(factory.Types.ContainsKey("file"));
			Assert.IsTrue(factory.Types.ContainsKey("msmq"));
			Assert.IsTrue(factory.Types.ContainsKey("trace"));
			Assert.IsTrue(factory.Types.ContainsKey("eventlog"));
			Assert.IsTrue(factory.Types.ContainsKey("circular"));
			Assert.IsFalse(factory.Types.ContainsKey("true"));

			// But not the unit test sinks.
			Assert.IsFalse(factory.Types.ContainsKey("memory"));
			Assert.IsFalse(factory.Types.ContainsKey("string"));			
		}

		[Test]
		public void LogSinkFactory_CreateFactoryWithOnlyExternalAssemblies()
		{
			List<Assembly> assemblies = new List<Assembly> { this.GetType().Assembly };
			LogSinkFactory factory = new LogSinkFactory(assemblies);
			Assert.IsNotNull(factory.Types);
			Assert.AreEqual(12, factory.Types.Count);

			// The standard sinks should be available.
			Assert.IsTrue(factory.Types.ContainsKey("funnel"));
			Assert.IsTrue(factory.Types.ContainsKey("buffer"));
			Assert.IsTrue(factory.Types.ContainsKey("console"));
			Assert.IsTrue(factory.Types.ContainsKey("debug"));
			Assert.IsTrue(factory.Types.ContainsKey("file"));
			Assert.IsTrue(factory.Types.ContainsKey("msmq"));
			Assert.IsTrue(factory.Types.ContainsKey("trace"));
			Assert.IsTrue(factory.Types.ContainsKey("eventlog"));
			Assert.IsTrue(factory.Types.ContainsKey("circular"));
			Assert.IsTrue(factory.Types.ContainsKey("async"));

			// But not the unit test sinks.
			Assert.IsTrue(factory.Types.ContainsKey("memory"));
			Assert.IsTrue(factory.Types.ContainsKey("string"));			
		}

		[Test]
		public void LogSinkFactory_BuildSinkHostedInBlackBoxAssembly()
		{
			LogSinkFactory factory = new LogSinkFactory();
			var sink = factory.Build("Console", null, false);
			Assert.IsNotNull(sink);
			Assert.IsInstanceOf<ConsoleSink>(sink);
		}

		[Test]
		public void LogSinkFactory_BuildSinkHostedInExternalAssembly()
		{
			LogSinkFactory factory = new LogSinkFactory(new List<Assembly> { this.GetType().Assembly });
			var sink = factory.Build("Memory", null, false);
			Assert.IsNotNull(sink);
			Assert.IsInstanceOf<MemorySink>(sink);
		}

		[Test]
		public void LogSinkFactory_BuildSinkWithArguments()
		{
			LogSinkFactory factory = new LogSinkFactory();
			Dictionary<string, string> arguments = new Dictionary<string, string>
			{
				{ "Queue", @".\private$\BlackBox" }
			};

			var sink = factory.Build("MSMQ", arguments, false);
			Assert.IsNotNull(sink);
			Assert.IsInstanceOf<MessageQueueSink>(sink);
			Assert.AreEqual(@".\private$\BlackBox", ((MessageQueueSink)sink).Queue);
		}
	}
}
