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
