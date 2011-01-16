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

namespace BlackBox.UnitTests.Tests.Filters
{
    [TestFixture]
    public class ConditionFilterTests
    {
        [Test]
        public void LevelMatchFilter_EntriesWhichEvaluatesConditionToTrueReturnsMatch()
        {
            string condition = "message=='Hello World!'";
            ConditionFilter filter = new ConditionFilter { Condition = condition, Action = LogFilterResult.Filter };
            filter.Initialize(null /* We don't need the service locator here. */);
            Logger logger = new Logger(null, typeof(ConditionFilterTests));
            ILogEntry trueEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Information, "Hello World!", logger, null);
            ILogEntry falseEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Information, "Goodbye World!", logger, null);
            Assert.AreEqual(LogFilterResult.Filter, filter.Evaluate(trueEntry));
            Assert.AreEqual(LogFilterResult.Neutral, filter.Evaluate(falseEntry));
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BlackBoxException), ExpectedMessage = "The filter condition has not been set.")]
        public void LevelMatchFilter_ThrowsIfLevelIsNotSetAtInitialization()
        {
            ConditionFilter filter = new ConditionFilter();
            filter.Initialize(null);
        }
    }
}
