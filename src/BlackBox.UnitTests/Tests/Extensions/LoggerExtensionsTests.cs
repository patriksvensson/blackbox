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

namespace BlackBox.UnitTests.Tests.Extensions
{
	[TestFixture]
	public class LoggerExtensionsTests
	{
		private ILogKernel _kernel;
		private TestSink _sink;

		#region Private Nested Test Classes
		private class TestSink : LogSink
		{
			public ILogEntry Entry { get; private set; }

			public TestSink()
			{
			}

			protected override void WriteEntry(ILogEntry entry)
			{
				this.Entry = entry;
			}
		}
		#endregion

		#region Test Initialization

		[TestFixtureSetUp]
		public void Initialize()
		{
			LogConfiguration configuration = new LogConfiguration();
			_sink = new TestSink();
			configuration.Sinks.Add(_sink);
			_kernel = new LogKernel(configuration);
		}

		#endregion

		[Test]
		public void LoggerExtensions_WriteFatalMessage()
		{
			_kernel.GetLogger().Fatal("Fatal");
			Assert.AreEqual(LogLevel.Fatal, _sink.Entry.Level);
			Assert.AreEqual("Fatal", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteFatalMessageWithArgument()
		{
			_kernel.GetLogger().Fatal("Fatal{0}", 1);
			Assert.AreEqual(LogLevel.Fatal, _sink.Entry.Level);
			Assert.AreEqual("Fatal1", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteErrorMessage()
		{
			_kernel.GetLogger().Error("Error");
			Assert.AreEqual(LogLevel.Error, _sink.Entry.Level);
			Assert.AreEqual("Error", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteErrorMessageWithArgument()
		{
			_kernel.GetLogger().Error("Error{0}", 1);
			Assert.AreEqual(LogLevel.Error, _sink.Entry.Level);
			Assert.AreEqual("Error1", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteWarningMessage()
		{
			_kernel.GetLogger().Warning("Warning");
			Assert.AreEqual(LogLevel.Warning, _sink.Entry.Level);
			Assert.AreEqual("Warning", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteWarningMessageWithArgument()
		{
			_kernel.GetLogger().Warning("Warning{0}", 1);
			Assert.AreEqual(LogLevel.Warning, _sink.Entry.Level);
			Assert.AreEqual("Warning1", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteInformationMessage()
		{
			_kernel.GetLogger().Information("Information");
			Assert.AreEqual(LogLevel.Information, _sink.Entry.Level);
			Assert.AreEqual("Information", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteInformationMessageWithArgument()
		{
			_kernel.GetLogger().Information("Information{0}", 1);
			Assert.AreEqual(LogLevel.Information, _sink.Entry.Level);
			Assert.AreEqual("Information1", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteVerboseMessage()
		{
			_kernel.GetLogger().Verbose("Verbose");
			Assert.AreEqual(LogLevel.Verbose, _sink.Entry.Level);
			Assert.AreEqual("Verbose", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteVerboseMessageWithArgument()
		{
			_kernel.GetLogger().Verbose("Verbose{0}", 1);
			Assert.AreEqual(LogLevel.Verbose, _sink.Entry.Level);
			Assert.AreEqual("Verbose1", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteDebugMessage()
		{
			_kernel.GetLogger().Debug("Debug");
			Assert.AreEqual(LogLevel.Debug, _sink.Entry.Level);
			Assert.AreEqual("Debug", _sink.Entry.Message);
		}

		[Test]
		public void LoggerExtensions_WriteDebugMessageWithArgument()
		{
			_kernel.GetLogger().Debug("Debug{0}", 1);
			Assert.AreEqual(LogLevel.Debug, _sink.Entry.Level);
			Assert.AreEqual("Debug1", _sink.Entry.Message);
		}
	}
}
