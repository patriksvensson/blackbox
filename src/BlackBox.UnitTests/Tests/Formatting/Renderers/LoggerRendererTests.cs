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
using BlackBox.Formatting;

namespace BlackBox.UnitTests.Tests.Formatting
{
	[TestFixture]
	public class LoggerRendererTests
	{
		#region Private Helper Methods
		private string Render(string format)
		{
			LogKernel kernel = new LogKernel(new LogConfiguration());
			ILogger logger = kernel.GetLogger();
			FormatPatternFactory factory = new FormatPatternFactory();
			FormatPattern pattern = factory.Create(format);
			ILogEntry entry = new LogEntry(DateTimeOffset.Now, LogLevel.Information, "The log message.", logger, null);
			return pattern.Render(entry);
		}
		#endregion

		[Test]
		public void LoggerRenderer_RenderName()
		{
			string format = "$(logger(fullname=false))";
			string expected = "LoggerRendererTests";
			Assert.AreEqual(expected, this.Render(format));
		}

		[Test]
		public void LoggerRenderer_RenderFullName()
		{
			string format = "$(logger(fullname=true))";
			string expected = "BlackBox.UnitTests.Tests.Formatting.LoggerRendererTests";
			Assert.AreEqual(expected, this.Render(format));
		}
	}
}
