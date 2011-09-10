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

namespace BlackBox.UnitTests.Tests
{
	[TestFixture]
	public class InternalLoggerTests
	{
		[Test]
		public void InternalLogger_InternalLoggerIsDisabledByDefault()
		{
			LogConfiguration configuration = new LogConfiguration();
			Assert.IsFalse(configuration.InternalLogger.Enabled, "The internal logger should be enabled by default.");
		}

		[Test]
		public void InternalLogger_InternalLoggerWritesToTraceListeners()
		{
			LogConfiguration configuration = new LogConfiguration();
			configuration.InternalLogger.Enabled = true;

			// Redirect the output from the console to a string writer.
			using (StringTraceListenerScope scope = new StringTraceListenerScope())
			{
				// Write something to the internal log and make sure we intercepted it.
				configuration.InternalLogger.Write(LogLevel.Information, "Hello World");
				Assert.AreEqual(1, scope.Listener.Messages.Count);
				Assert.AreEqual("[BLACKBOX] Hello World", scope.Listener.Messages[0]);
			}
		}

		[Test]
		public void InternalLogger_InternalLoggerDoNotWriteToTraceListenersIfDisabled()
		{
			LogConfiguration configuration = new LogConfiguration();			

			// Redirect the output from the console to a string writer.
			using (StringTraceListenerScope scope = new StringTraceListenerScope())
			{
				// Write something to the internal log and make sure we did not intercept it.
				configuration.InternalLogger.Write(LogLevel.Information, "Hello World");
				Assert.AreEqual(0, scope.Listener.Messages.Count);
			}
		}

		[Test]
		public void InternalLogger_InternalLoggerRespectsThresholdLevel()
		{
			LogConfiguration configuration = new LogConfiguration();
			configuration.InternalLogger.Enabled = true;
			configuration.InternalLogger.Threshold = LogLevel.Information;

			// Redirect the output from the console to a string writer.
			using (StringTraceListenerScope scope = new StringTraceListenerScope())
			{
				configuration.InternalLogger.Write(LogLevel.Verbose, "1");
				Assert.AreEqual(0, scope.Listener.Messages.Count);
				configuration.InternalLogger.Write(LogLevel.Debug, "2");
				Assert.AreEqual(0, scope.Listener.Messages.Count);
				configuration.InternalLogger.Write(LogLevel.Information, "3");
				Assert.AreEqual(1, scope.Listener.Messages.Count);
				configuration.InternalLogger.Write(LogLevel.Warning, "4");
				Assert.AreEqual(2, scope.Listener.Messages.Count);
				configuration.InternalLogger.Write(LogLevel.Error, "5");
				Assert.AreEqual(3, scope.Listener.Messages.Count);
				configuration.InternalLogger.Write(LogLevel.Fatal, "6");
				Assert.AreEqual(4, scope.Listener.Messages.Count);
			}
		}
	}
}
