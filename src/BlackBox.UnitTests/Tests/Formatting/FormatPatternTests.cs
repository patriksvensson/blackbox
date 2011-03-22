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
using BlackBox.Formatting;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Formatting
{
	[TestFixture]
	public class FormatPatternTests
	{
		[Test]
		public void FormatPattern_RenderLiteralPattern()
		{
			LogKernel kernel = new LogKernel(new LogConfiguration());
			ILogger logger = kernel.GetLogger();
			FormatPatternFactory factory = new FormatPatternFactory();
			FormatPattern pattern = factory.Create("Hello World!");
			ILogEntry entry = new LogEntry(DateTimeOffset.Now, LogLevel.Information, "The log message.", logger, null);
			string rendered = pattern.Render(entry);
			Assert.IsNotNullOrEmpty(rendered);
			Assert.AreEqual("Hello World!", rendered);
		}

		[Test]
		public void FormatPattern_RenderTimeRenderer()
		{
			LogKernel kernel = new LogKernel(new LogConfiguration());
			ILogger logger = kernel.GetLogger();
			FormatPatternFactory factory = new FormatPatternFactory();
			FormatPattern pattern = factory.Create("$(time(format='HH:mm:ss'))");
			DateTimeOffset currentTime = DateTimeOffset.Now;
			ILogEntry entry = new LogEntry(currentTime, LogLevel.Information, "The log message.", logger, null);
			string rendered = pattern.Render(entry);
			Assert.IsNotNullOrEmpty(rendered);
			Assert.AreEqual(currentTime.ToString("HH:mm:ss"), rendered);
		}

		[Test]
		public void FormatPattern_RenderEscapedRenderer()
		{
			LogKernel kernel = new LogKernel(new LogConfiguration());
			ILogger logger = kernel.GetLogger();
			FormatPatternFactory factory = new FormatPatternFactory();
			FormatPattern pattern = factory.Create("$$(time(format='HH:mm:ss'))");
			ILogEntry entry = new LogEntry(DateTime.Now, LogLevel.Information, "The log message.", logger, null);
			Assert.AreEqual("$(time(format='HH:mm:ss'))", pattern.Render(entry));
		}

		[Test]
		public void FormatPatternFactory_BuildPatternWithTransformer()
		{
			LogKernel kernel = new LogKernel(new LogConfiguration());
			ILogger logger = kernel.GetLogger();
			FormatPatternFactory factory = new FormatPatternFactory();
			FormatPattern pattern = factory.Create("$(uppercase(message()))");
			ILogEntry entry = new LogEntry(DateTimeOffset.Now, LogLevel.Information, "The log message.", logger, null);
			string rendered = pattern.Render(entry);
			Assert.IsNotNullOrEmpty(rendered);
			Assert.AreEqual("THE LOG MESSAGE.", rendered);
		}

		[Test]
		public void FormatPatternFactory_BuildPatternWithNestedTransformers()
		{
			LogKernel kernel = new LogKernel(new LogConfiguration());
			ILogger logger = kernel.GetLogger();
			FormatPatternFactory factory = new FormatPatternFactory();
			FormatPattern pattern = factory.Create("$(rot13(uppercase(message())))");
			ILogEntry entry = new LogEntry(DateTimeOffset.Now, LogLevel.Information, "Hello World!", logger, null);
			string rendered = pattern.Render(entry);
			Assert.IsNotNullOrEmpty(rendered);
			Assert.AreEqual("URYYB JBEYQ!", rendered);
		}

		[Test]
		public void FormatPatternFactory_BuildComplexPattern()
		{
			LogKernel kernel = new LogKernel(new LogConfiguration());
			ILogger logger = kernel.GetLogger();
			FormatPatternFactory factory = new FormatPatternFactory();
			FormatPattern pattern = factory.Create("[$(time(format='HH:mm:ss'))] $(uppercase(rot13(message())))");
			DateTimeOffset currentTime = DateTimeOffset.Now;
			ILogEntry entry = new LogEntry(currentTime, LogLevel.Information, "Hello World!", logger, null);
			string rendered = pattern.Render(entry);
			string expected = string.Format("[{0:HH:mm:ss}] URYYB JBEYQ!", currentTime);
			Assert.IsNotNullOrEmpty(rendered);
			Assert.AreEqual(expected, rendered);
		}
	}
}
