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
	public class LogSinkTests
	{
		[Test]
		[ExpectedException(ExpectedException = typeof(BlackBoxException), ExpectedMessage = "Cannot change the name of the sink after initialization.")]
		public void LogKernel_LogSinkNamesCanNotChangeAfterKernelInstantiation()
		{
			LogConfiguration configuration = new LogConfiguration();
			StringSink sink = new StringSink { Name = "fake" };
			sink.Name = "fake2";
			sink.Name = "fake3";
			configuration.Sinks.Add(sink);
			Assert.AreEqual("fake3", sink.Name);
			LogKernel kernel = new LogKernel(configuration);
			sink.Name = "fake4";
		}
	}
}
