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
	public class LogSinkTests
	{
		#region Nested Private Test Classes
		private class TestSink : LogSink
		{
			private bool _disposed;

			public bool IsDisposed
			{
				get { return _disposed; }
			}

			protected override void Dispose(bool disposing)
			{
				_disposed = true;
			}

			protected override void WriteEntry(ILogEntry entry)
			{
				this.InternalLogger.Write(LogLevel.Information, entry.Message);
			}
		}

		private class ThrowSink : LogSink
		{
			private Func<Exception> _exceptionFactory;

			public ThrowSink(Func<Exception> exceptionFactory)
			{
				_exceptionFactory = exceptionFactory;
			}

			public ThrowSink(string name, Func<Exception> exceptionFactory)
			{
				this.Name = name;
				_exceptionFactory = exceptionFactory;				
			}

			protected override void WriteEntry(ILogEntry entry)
			{
				throw _exceptionFactory();
			}
		}
		#endregion

		[Test]
		[ExpectedException(ExpectedException = typeof(BlackBoxException), ExpectedMessage = "Cannot change the name of the sink after initialization.")]
		public void LogSink_LogSinkNamesCanNotChangeAfterKernelInstantiation()
		{
			LogConfiguration configuration = new LogConfiguration();
			StringSink sink = new StringSink { Name = "fake" };
			sink.Name = "fake2";
			sink.Name = "fake3";
			configuration.Sinks.Add(sink);
			Assert.AreEqual("fake3", sink.Name);
			LogKernel kernel = new LogKernel(configuration);
			sink.Name = "fake4";
		}

		[Test]
		public void LogSink_LogSinkHasInternalLogger()
		{
			LogConfiguration configuration = new LogConfiguration();
			StringSink sink = new StringSink { Name = "fake" };
			configuration.Sinks.Add(sink);
			LogKernel kernel = new LogKernel(configuration);
			Assert.IsNotNull(sink.InternalLogger);
		}

		[Test]
		public void LogSink_InternalLoggerIsRemovedFromSinkWhenDisposed()
		{
			LogConfiguration configuration = new LogConfiguration();
			StringSink sink = new StringSink { Name = "fake" };
			configuration.Sinks.Add(sink);
			LogKernel kernel = new LogKernel(configuration);
			Assert.IsNotNull(sink.InternalLogger);
			kernel.Dispose();
			Assert.IsNull(sink.InternalLogger);
		}

		public void LogSink_LogSinkCanWriteToInternalLogger()
		{
			LogConfiguration configuration = new LogConfiguration();
			configuration.InternalLogger.Enabled = true;
			configuration.Sinks.Add(new TestSink());
			LogKernel kernel = new LogKernel(configuration);
			ILogger logger = kernel.GetLogger();

			using (StringTraceListenerScope scope = new StringTraceListenerScope())
			{
				Assert.AreEqual(0, scope.Listener.Messages.Count);
				logger.Write(LogLevel.Information, "TestMessage");
				Assert.AreEqual(1, scope.Listener.Messages.Count);
				Assert.AreEqual("TestMessage", scope.Listener.Messages[0]);
			}
		}

		[Test]
		public void LogSink_UnhandledExceptionsInLogSinkAreHandledAndWrittenToInternalLog_LogSinkGotName()
		{			
			LogConfiguration configuration = new LogConfiguration();
			configuration.InternalLogger.Enabled = true;
			configuration.Sinks.Add(new ThrowSink("throwsink1", () => new InvalidOperationException("Hello World!")));
			LogKernel kernel = new LogKernel(configuration);
			ILogger logger = kernel.GetLogger();

			string expected = "[BLACKBOX] The sink 'throwsink1' (BlackBox.UnitTests.Tests.LogSinkTests+ThrowSink) threw an exception. Hello World!";
			using (StringTraceListenerScope scope = new StringTraceListenerScope())
			{
				logger.Write(LogLevel.Information, "TestMessage");				
				Assert.AreEqual(expected, scope.Listener.Messages[0]);
			}
		}

		[Test]
		public void LogSinkProxy_UnhandledExceptionsInLogSinkProxyAreHandledAndWrittenToInternalLogWhenWritingMoreThanOneMessage_LogSinkGotName()
		{
			// This test is using the buffer proxy to buffer up messages and
			// indirectly use the LogSink.Write(ILogEntry[]) overload on the sink.			

			LogConfiguration configuration = new LogConfiguration();
			configuration.InternalLogger.Enabled = true;
			BufferProxy funnelProxy = new BufferProxy() { Name = "funnel1", BufferSize = 0 };
			funnelProxy.Sinks.Add(new ThrowSink("throwsink1", () => new InvalidOperationException("Hello World!")));
			configuration.Sinks.Add(funnelProxy);
			LogKernel kernel = new LogKernel(configuration);
			ILogger logger = kernel.GetLogger();

			string expected = "[BLACKBOX] The sink 'throwsink1' (BlackBox.UnitTests.Tests.LogSinkTests+ThrowSink) threw an exception. Hello World!";
			using (StringTraceListenerScope scope = new StringTraceListenerScope())
			{
				logger.Write(LogLevel.Information, "TestMessage");				
				Assert.AreEqual(expected, scope.Listener.Messages[0]);
			}
		}

		[Test]
		public void LogSink_UnhandledExceptionsInLogSinkAreHandledAndWrittenToInternalLog_LogSinkGotNoName()
		{
			LogConfiguration configuration = new LogConfiguration();
			configuration.InternalLogger.Enabled = true;
			configuration.Sinks.Add(new ThrowSink(() => new InvalidOperationException("Hello World!")));
			LogKernel kernel = new LogKernel(configuration);
			ILogger logger = kernel.GetLogger();

			string expected = "[BLACKBOX] An unnamed sink of type 'BlackBox.UnitTests.Tests.LogSinkTests+ThrowSink' threw an exception. Hello World!";
			using (StringTraceListenerScope scope = new StringTraceListenerScope())
			{
				logger.Write(LogLevel.Information, "TestMessage");				
				Assert.AreEqual(expected, scope.Listener.Messages[0]);
			}
		}

		[Test]
		public void LogSinkProxy_UnhandledExceptionsInLogSinkProxyAreHandledAndWrittenToInternalLogWhenWritingMoreThanOneMessage_LogSinkGotNoName()
		{
			// This test is using the buffer proxy to buffer up messages and
			// indirectly use the LogSink.Write(ILogEntry[]) overload on the sink.			

			LogConfiguration configuration = new LogConfiguration();
			configuration.InternalLogger.Enabled = true;
			BufferProxy funnelProxy = new BufferProxy() { BufferSize = 0 };
			funnelProxy.Sinks.Add(new ThrowSink(() => new InvalidOperationException("Hello World!")));
			configuration.Sinks.Add(funnelProxy);
			LogKernel kernel = new LogKernel(configuration);
			ILogger logger = kernel.GetLogger();

			string expected = "[BLACKBOX] An unnamed sink of type 'BlackBox.UnitTests.Tests.LogSinkTests+ThrowSink' threw an exception. Hello World!";
			using (StringTraceListenerScope scope = new StringTraceListenerScope())
			{
				logger.Write(LogLevel.Information, "TestMessage");
				Assert.AreEqual(expected, scope.Listener.Messages[0]);
			}
		}
	}
}
