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
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests
{
	[TestFixture]
	public class LogKernelTests
	{
		#region Nested Private Test Classes

		private class FakeSink : StringSink
		{
			private bool _initializeMethodWasCalled;

			public bool InitializeMethodWasCalled
			{
				get { return _initializeMethodWasCalled; }
			}

			protected internal override void Initialize(InitializationContext context)
			{
				_initializeMethodWasCalled = true;
			}
		}

		private class FakeProxySink : LogSinkProxy
		{
			private bool _initializeMethodWasCalled;

			public bool InitializeMethodWasCalled
			{
				get { return _initializeMethodWasCalled; }
			}

			protected internal override void Initialize(InitializationContext context)
			{
				_initializeMethodWasCalled = true;
			}

			protected override void WriteEntry(ILogEntry entry)
			{
				foreach (LogSink sink in this.Sinks)
				{
					sink.Write(entry);
				}
			}
		}

		private class FakeFilter : LogFilter
		{
			private bool _initializeMethodWasCalled;

			public bool InitializeMethodWasCalled
			{
				get { return _initializeMethodWasCalled; }
			}

			protected internal override void Initialize(InitializationContext context)
			{
				_initializeMethodWasCalled = true;
			}

			protected internal override LogFilterResult Evaluate(ILogEntry entry)
			{
				return LogFilterResult.Accept;
			}
		}

		#endregion

		[Test]
		public void LogKernel_NullLogConfigurationCannotBePassedToConstructor_Throws()
		{
            Assert.That(() => new LogKernel(null),
                Throws.Exception.TypeOf<ArgumentNullException>()
                .With.Property("Message").EqualTo("Log configuration cannot be null.\r\nParameter name: configuration")
                .And.Property("ParamName").EqualTo("configuration"));
		}

		[Test]
		public void LogKernel_LogSinksAreInitializedWhenCreatingKernel()
		{
			LogConfiguration configuration = new LogConfiguration();
			FakeSink sink = new FakeSink { Name = "fake" };
			configuration.Sinks.Add(sink);
			Assert.IsFalse(sink.IsInitialized);
			Assert.IsFalse(sink.InitializeMethodWasCalled);
			LogKernel kernel = new LogKernel(configuration);
			Assert.IsTrue(sink.IsInitialized);
			Assert.IsTrue(sink.InitializeMethodWasCalled);
		}

		[Test]
		public void LogKernel_NestedLogSinksInProxiesAreInitializedWhenCreatingKernel()
		{
			LogConfiguration configuration = new LogConfiguration();
			FakeProxySink proxy = new FakeProxySink();
			FakeSink sink = new FakeSink();
			proxy.Sinks.Add(sink);
			configuration.Sinks.Add(proxy);
			Assert.IsFalse(proxy.InitializeMethodWasCalled);
			Assert.IsFalse(sink.InitializeMethodWasCalled);
			LogKernel kernel = new LogKernel(configuration);
			Assert.IsTrue(proxy.InitializeMethodWasCalled);
			Assert.IsTrue(sink.InitializeMethodWasCalled);
		}

		[Test]
		public void LogKernel_GlobalFiltersAreInitializedWhenCreatingKernel()
		{
			LogConfiguration configuration = new LogConfiguration();
			FakeFilter filter = new FakeFilter { };
			configuration.Filters.Add(filter);
			Assert.IsFalse(filter.InitializeMethodWasCalled);
			LogKernel kernel = new LogKernel(configuration);
			Assert.IsTrue(filter.InitializeMethodWasCalled);
		}

		[Test]
		public void LogKernel_SinkFiltersAreInitializedWhenCreatingKernel()
		{
			LogConfiguration configuration = new LogConfiguration();
			FakeFilter filter = new FakeFilter { };
			FakeSink sink = new FakeSink { };
			sink.Filters.Add(filter);
			configuration.Sinks.Add(sink);
			Assert.IsFalse(filter.InitializeMethodWasCalled);
			LogKernel kernel = new LogKernel(configuration);
			Assert.IsTrue(filter.InitializeMethodWasCalled);
		}

		[Test]
		public void LogKernel_GetLogger()
		{
			LogConfiguration configuration = new LogConfiguration();
			FakeSink sink = new FakeSink { Name = "fake" };
			configuration.Sinks.Add(sink);
			LogKernel kernel = new LogKernel(configuration);
			ILogger logger = kernel.GetLogger();
			Assert.AreEqual(typeof(LogKernelTests), logger.Source);
			ILogger stringLogger = kernel.GetLogger(typeof(System.String));
			Assert.AreEqual(typeof(System.String), stringLogger.Source);
		}

		[Test]
		public void LogKernel_Write()
		{
			LogConfiguration configuration = new LogConfiguration();
			FakeSink sink = new FakeSink { Name = "fake" };
			configuration.Sinks.Add(sink);
			LogKernel kernel = new LogKernel(configuration);
			ILogger logger = kernel.GetLogger();
			kernel.Write(LogLevel.Information, logger, null, "Hello World");
			Assert.AreEqual("Hello World", sink.LastEntry);
		}
	}
}
