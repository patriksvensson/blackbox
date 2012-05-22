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
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Filters
{
	[TestFixture]
	public class LevelRangeFilterTests
	{
		[Test]
		public void LevelRangeFilter_EntriesWithinRangeReturnsMatch()
		{
			LevelRangeFilter filter = new LevelRangeFilter { MinLevel = LogLevel.Information, MaxLevel = LogLevel.Debug, Action = LogFilterResult.Accept };
			Logger logger = new Logger(null, typeof(LevelRangeFilterTests));
			ILogEntry informationEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Information, "", logger, null);
			ILogEntry verboseEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Verbose, "", logger, null);
			ILogEntry debugEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Debug, "", logger, null);
			ILogEntry errorEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Error, "", logger, null);
			ILogEntry fatalEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Fatal, "", logger, null);
			ILogEntry warningEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Warning, "", logger, null);
			Assert.AreEqual(LogFilterResult.Accept, filter.Evaluate(informationEntry));
			Assert.AreEqual(LogFilterResult.Accept, filter.Evaluate(verboseEntry));
			Assert.AreEqual(LogFilterResult.Accept, filter.Evaluate(debugEntry));
			Assert.AreEqual(LogFilterResult.Neutral, filter.Evaluate(errorEntry));
			Assert.AreEqual(LogFilterResult.Neutral, filter.Evaluate(fatalEntry));
			Assert.AreEqual(LogFilterResult.Neutral, filter.Evaluate(warningEntry));
		}

		[Test]
		public void LevelRangeFilter_ThrowsIfMinLevelIsNotSetAtInitialization()
		{
			LevelRangeFilter filter = new LevelRangeFilter { MaxLevel = LogLevel.Debug, Action = LogFilterResult.Accept };
            Assert.That(() => filter.Initialize(null),
                Throws.Exception.TypeOf<BlackBoxException>()
                .With.Property("Message").EqualTo("The minimum log level has not been set."));
		}

		[Test]
		public void LevelRangeFilter_ThrowsIfMaxLevelIsNotSetAtInitialization()
		{
			LevelRangeFilter filter = new LevelRangeFilter { MinLevel = LogLevel.Debug, Action = LogFilterResult.Accept };
            Assert.That(() => filter.Initialize(null),
                Throws.Exception.TypeOf<BlackBoxException>()
                .With.Property("Message").EqualTo("The maximum log level has not been set."));
		}

		[Test]
		public void LevelRangeFilter_ThrowsIfMinimumLevelIsGreaterThanMaximumLevelAtInitialization()
		{
			LevelRangeFilter filter = new LevelRangeFilter { MinLevel = LogLevel.Debug, MaxLevel = LogLevel.Verbose, Action = LogFilterResult.Accept };
            Assert.That(() => filter.Initialize(null),
                Throws.Exception.TypeOf<BlackBoxException>()
                .With.Property("Message").EqualTo("The minimum log level must be less or equal to the maximum log level."));
		}
	}
}
