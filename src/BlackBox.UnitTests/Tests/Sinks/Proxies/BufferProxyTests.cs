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

using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Sinks.Proxies
{
	[TestFixture]
	public class BufferProxyTests
	{
		#region Nested Private Test Classes
		private class TestSink : LogSink
		{
			private int _messagesWritten;

			public int MessagesWritten
			{
				get { return _messagesWritten; }
			}

			protected override void WriteEntry(ILogEntry entry)
			{
				_messagesWritten++;
			}
		}
		#endregion

		[Test]
		public void BufferProxy_RemainingMessagesAreFlushedWhenBufferProxyIsDisposed()
		{
			LogConfiguration configuration = new LogConfiguration();
			BufferProxy proxy = new BufferProxy { BufferSize = 3 };
			TestSink sink = new TestSink();
			proxy.Sinks.Add(sink);
			configuration.Sinks.Add(proxy);
			LogKernel kernel = new LogKernel(configuration);
			ILogger logger = kernel.GetLogger();

			Assert.AreEqual(0, sink.MessagesWritten);
			logger.Write(LogLevel.Information, "Hello World!");
			logger.Write(LogLevel.Information, "Hello World Again!");
			Assert.AreEqual(0, sink.MessagesWritten);
			configuration.Dispose();
			Assert.AreEqual(2, sink.MessagesWritten);
		}
	}
}
