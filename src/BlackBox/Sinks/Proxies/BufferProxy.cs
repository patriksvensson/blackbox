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

namespace BlackBox
{
    /// <summary>
    /// A log sink proxy that buffers messages and write them
    /// at once to all the child log sinks when the buffer size threshold have been reached.
    /// </summary>
    [LogSinkType("buffer")]
    public sealed class BufferProxy : LogSinkProxy
    {
        private readonly object _syncLock;
        private readonly Queue<ILogEntry> _queue;
        private int _bufferSize;
        private bool _disposed;

        /// <summary>
        /// Gets or sets the size of the buffer.
        /// </summary>
        /// <value>The size of the buffer.</value>
        public int BufferSize
        {
          get { return _bufferSize; }
          set { _bufferSize = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferProxy"/> class.
        /// </summary>
        public BufferProxy()
        {
            _syncLock = new object();
            _queue = new Queue<ILogEntry>();
            _bufferSize = 5;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                    lock (_syncLock)
                    {
                        // Flush the queue.
                        this.ProcessQueue();
                    }
                    _disposed = true;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Performs the writing of the specified entry to the log sink.
        /// </summary>
        /// <param name="entry">The entry.</param>
        protected override void WriteEntry(ILogEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            lock (_syncLock)
            {
                _queue.Enqueue(entry);

                if (_queue.Count > this.BufferSize)
                {
                    this.ProcessQueue();
                }
            }
        }

        /// <summary>
        /// Performs the writing of the specified entries to the log sink.
        /// </summary>
        /// <param name="entries">The entries.</param>
        protected override void WriteEntries(ILogEntry[] entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException("entries");
            }

            lock (_syncLock)
            {
                foreach (ILogEntry entry in entries)
                {
                    _queue.Enqueue(entry);
                }

                if (_queue.Count >= this.BufferSize)
                {
                    this.ProcessQueue();
                }
            }
        }

        private void ProcessQueue()
        {
            // NOTE: This method is not thread safe on its own.
            if (_queue.Count > 0)
            {
                ILogEntry[] entries = new ILogEntry[_queue.Count];
                int index = 0;
                while (_queue.Count != 0)
                {
                    entries[index] = _queue.Dequeue();
                    index++;
                }

                foreach (LogSink sink in this.Sinks)
                {
                    sink.Write(entries);
                }
            }
        }
    }
}
