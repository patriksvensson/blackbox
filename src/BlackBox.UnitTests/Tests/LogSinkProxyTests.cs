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
    public class LogSinkProxyTests
    {
        #region Nested Private Test Classes
        private class TestSink : LogSink
        {
            private bool _disposed;

            public bool IsDisposed
            {
                get { return _disposed; }
            }

            protected override void Dispose(bool disposing)
            {
                _disposed = true;
            }

            protected override void WriteEntry(ILogEntry entry)
            {
            }
        }
        #endregion

        [Test]
        public void LogSinkProxy_ChildSinksAreProperlyDisposedWhenProxyIsDisposed()
        {
            FunnelProxy proxy = new FunnelProxy();
            TestSink sink = new TestSink();
            proxy.Sinks.Add(sink);
            Assert.IsFalse(sink.IsDisposed);
            proxy.Dispose();
            Assert.IsTrue(sink.IsDisposed);
        }
    }
}
