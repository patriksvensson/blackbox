using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Sinks.Proxies
{
    [TestFixture]
    public class CircularProxyTests
    {
        [Test]
        public void CircularProxy_CircularProxyWithoutSinksWontFail()
        {
            LogConfiguration configuration = new LogConfiguration();
            CircularProxy proxy = new CircularProxy();
            LogKernel kernel = new LogKernel(configuration);
            configuration.Sinks.Add(proxy);
            ILogger logger = kernel.GetLogger();
            logger.Write(LogLevel.Information, "1");
        }

        [Test]
        public void CircularProxy_CircularProxyWorksAsExpected()
        {
            LogConfiguration configuration = new LogConfiguration();
            CircularProxy proxy = new CircularProxy();
            proxy.Sinks.Add(new MemorySink());
            proxy.Sinks.Add(new MemorySink());
            proxy.Sinks.Add(new MemorySink());
            configuration.Sinks.Add(proxy);
            LogKernel kernel = new LogKernel(configuration);
            ILogger logger = kernel.GetLogger();

            logger.Write(LogLevel.Information, "1");
            Assert.AreEqual(1, ((MemorySink)proxy.Sinks[0]).Queue.Count);
            Assert.AreEqual(0, ((MemorySink)proxy.Sinks[1]).Queue.Count);
            Assert.AreEqual(0, ((MemorySink)proxy.Sinks[2]).Queue.Count);
            logger.Write(LogLevel.Information, "2");
            Assert.AreEqual(1, ((MemorySink)proxy.Sinks[0]).Queue.Count);
            Assert.AreEqual(1, ((MemorySink)proxy.Sinks[1]).Queue.Count);
            Assert.AreEqual(0, ((MemorySink)proxy.Sinks[2]).Queue.Count);
            logger.Write(LogLevel.Information, "3");
            Assert.AreEqual(1, ((MemorySink)proxy.Sinks[0]).Queue.Count);
            Assert.AreEqual(1, ((MemorySink)proxy.Sinks[1]).Queue.Count);
            Assert.AreEqual(1, ((MemorySink)proxy.Sinks[2]).Queue.Count);
            logger.Write(LogLevel.Information, "4");
            Assert.AreEqual(2, ((MemorySink)proxy.Sinks[0]).Queue.Count);
            Assert.AreEqual(1, ((MemorySink)proxy.Sinks[1]).Queue.Count);
            Assert.AreEqual(1, ((MemorySink)proxy.Sinks[2]).Queue.Count);
            logger.Write(LogLevel.Information, "5");
            Assert.AreEqual(2, ((MemorySink)proxy.Sinks[0]).Queue.Count);
            Assert.AreEqual(2, ((MemorySink)proxy.Sinks[1]).Queue.Count);
            Assert.AreEqual(1, ((MemorySink)proxy.Sinks[2]).Queue.Count);
            logger.Write(LogLevel.Information, "6");
            Assert.AreEqual(2, ((MemorySink)proxy.Sinks[0]).Queue.Count);
            Assert.AreEqual(2, ((MemorySink)proxy.Sinks[1]).Queue.Count);
            Assert.AreEqual(2, ((MemorySink)proxy.Sinks[2]).Queue.Count);
        }
    }
}
