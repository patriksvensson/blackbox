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
using System.Threading;

namespace BlackBox.UnitTests.Tests.Sinks.Proxies
{
	[TestFixture]
	public class AsyncProxyTests
	{
		#region Private Nested Test Classes
		private class TestSink : LogSink
		{
			private ManualResetEvent _stoppedEvent;
			private ILogEntry _entry;
			private int _writtenEntries;

			public ILogEntry Entry
			{
				get { return _entry; }
			}

			public int WrittenEntries
			{
				get { return _writtenEntries; }
			}

			public TestSink(ManualResetEvent stoppedEvent)
			{
				_stoppedEvent = stoppedEvent;
				_entry = null;
				_writtenEntries = 0;
			}

			protected override void WriteEntry(ILogEntry entry)
			{
				_entry = entry;
				_writtenEntries++;

				if (_stoppedEvent != null)
				{
					_stoppedEvent.Set();
				}
			}
		}
		#endregion

		[Test]
		public void AsyncProxy_LogEntriesAreWrittenToSinks()
		{
			LogConfiguration configuration = new LogConfiguration();
			AsyncProxy asyncProxy = new AsyncProxy();
			ManualResetEvent stoppedEvent = new ManualResetEvent(false);
			TestSink sink = new TestSink(stoppedEvent);
			asyncProxy.Sinks.Add(sink);
			configuration.Sinks.Add(asyncProxy);
			using (LogKernel kernel = new LogKernel(configuration))
			{
				ILogger logger = kernel.GetLogger();
				logger.Write(LogLevel.Information, "A message");

				// Wait for the entry to be written.
				if (!stoppedEvent.WaitOne(TimeSpan.FromSeconds(3)))
				{
					throw new TimeoutException("Timed out while waiting for entries to be written to sink.");
				}

				// Assert that the entry was written to the sink.
				Assert.IsNotNull(sink.Entry);
				Assert.AreEqual("A message", sink.Entry.Message);
				Assert.AreEqual(1, sink.WrittenEntries);
			}
		}
	}
}
