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

namespace BlackBox.UnitTests.Tests
{
	[TestFixture]
	public class LogKernelTests
	{
		#region Nested Private Test Classes
		private class FakeSink : StringSink
		{
			private bool _isInitialized;

			public bool IsInitialized
			{
				get { return _isInitialized; }
			}

			protected internal override void InitializeSink(IServiceLocator locator)
			{
				_isInitialized = true;
			}
		}
		#endregion

        [Test]
        [ExpectedException(ExpectedException = typeof(ArgumentNullException), UserMessage = "Log configuration cannot be null.")]
        public void LogKernel_NullLogConfigurationCannotBePassedToConstructor_Throws()
        {
            LogKernel kernel = new LogKernel(null);
        }

		[Test]
		public void LogKernel_LogSinksAreInitializedWhenCreatingKernel()
		{
			LogConfiguration configuration = new LogConfiguration();
			FakeSink sink = new FakeSink { Name = "fake" };
			configuration.Sinks.Add(sink);
			Assert.IsFalse(sink.IsInitialized);
			LogKernel kernel = new LogKernel(configuration);
			Assert.IsTrue(sink.IsInitialized);
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
