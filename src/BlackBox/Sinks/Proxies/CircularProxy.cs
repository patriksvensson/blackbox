using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BlackBox
{
    /// <summary>
    /// A log sink proxy that distribute log entries in a round robin fashion.
    /// </summary>
    [LogSinkType("circular")]
    public sealed class CircularProxy : LogSinkProxy
    {
        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularProxy"/> class.
        /// </summary>
        public CircularProxy()
        {
            _index = -1;
        }

        /// <summary>
        /// Performs the writing of the specified entries to 
        /// the nested log sinks in a circular manner.
        /// </summary>
        /// <param name="entry">The entry.</param>
        protected override void WriteEntry(ILogEntry entry)
        {
            if (this.Sinks.Count == 0)
            {
                return;
            }

            int index = Interlocked.Increment(ref _index) % this.Sinks.Count;
            if (index >= 0 && index < this.Sinks.Count)
            {
                this.Sinks[index].Write(entry);
            } 
        }
    }
}
