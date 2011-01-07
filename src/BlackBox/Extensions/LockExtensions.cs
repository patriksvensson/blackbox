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
using System.Threading;

namespace BlackBox
{
    internal static class ReaderWriterLockSlimExtensions
    {
        private struct Disposable : IDisposable
        {
            private readonly Action _action;

            public Disposable(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }

        internal static IDisposable AcquireReadLock(this ReaderWriterLockSlim lockInstance)
        {
            lockInstance.EnterReadLock();
            return new Disposable(lockInstance.ExitReadLock);
        }

        internal static IDisposable AcquireUpgradableReadLock(this ReaderWriterLockSlim lockInstance)
        {
            lockInstance.EnterUpgradeableReadLock();
            return new Disposable(lockInstance.ExitUpgradeableReadLock);
        }

        internal static IDisposable AcquireWriteLock(this ReaderWriterLockSlim lockInstance)
        {
            lockInstance.EnterWriteLock();
            return new Disposable(lockInstance.ExitWriteLock);
        }
    }
}
