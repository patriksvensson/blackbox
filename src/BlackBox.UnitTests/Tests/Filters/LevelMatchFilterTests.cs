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
using System.Xml.Linq;

namespace BlackBox.UnitTests.Tests.Filters
{
    [TestFixture]
    public class LevelMatchFilterTests
    {
        [Test]
        public void LevelMatchFilter_EntriesWithSameLevelReturnsMatch()
        {
            LevelMatchFilter filter = new LevelMatchFilter { Level = LogLevel.Information };
            Logger logger = new Logger(null, typeof(LevelMatchFilterTests));
            ILogEntry informationEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Information, "", logger, null);
            ILogEntry errorEntry = new LogEntry(DateTimeOffset.Now, LogLevel.Error, "", logger, null);
            Assert.AreEqual(LogFilterResult.Filter, filter.Evaluate(informationEntry));
            Assert.AreEqual(LogFilterResult.Neutral, filter.Evaluate(errorEntry));
        }

        [Test]
        [ExpectedException(ExpectedException=typeof(BlackBoxException), ExpectedMessage="The log level has not been set.")]
        public void LevelMatchFilter_ThrowsIfLevelIsNotSetAtInitialization()
        { 
            LevelMatchFilter filter = new LevelMatchFilter();
            filter.Initialize(null);
        }
    }
}
