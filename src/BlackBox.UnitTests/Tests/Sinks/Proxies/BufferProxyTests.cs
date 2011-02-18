using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
