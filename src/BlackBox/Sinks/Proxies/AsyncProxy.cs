using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Globalization;

namespace BlackBox
{
	/// <summary>
	/// A log sink proxy that distribute log entries asynchronous.
	/// </summary>
	[LogSinkType("async")]
	public class AsyncProxy : LogSinkProxy
	{
		private BlockingCollection<ILogEntry> _queue;
		private Thread _backgroundThread;
		private AutoResetEvent _stopEvent;
		private bool _disposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncProxy"/> class.
		/// </summary>
		public AsyncProxy()
		{
			_queue = new BlockingCollection<ILogEntry>(new ConcurrentQueue<ILogEntry>());
			_stopEvent = new AutoResetEvent(false);
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
					_disposed = true;

					if (!_queue.IsAddingCompleted)
					{
						// Mark the queue as not accepting more additions.
						_queue.CompleteAdding();

						// Wait for the thread to terminate.
						_stopEvent.WaitOne();
					}

					// Dispose the blocking collection.
					if (_queue != null)
					{
						_queue.Dispose();
						_queue = null;
					}

					// Got a stop event?
					if (_stopEvent != null)
					{
						_stopEvent.Dispose();
						_stopEvent = null;
					}
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Initializes the log sink proxy.
		/// </summary>
		/// <param name="context">The context.</param>
		protected internal override void Initialize(InitializationContext context)
		{
			// Start the background thread.
			_backgroundThread = new Thread(ThreadExecute);
			_backgroundThread.IsBackground = false;
			_backgroundThread.Start();

			// Call the base class implementation.
			base.Initialize(context);
		}

		/// <summary>
		/// Performs the writing of the specified entry to the log sink.
		/// </summary>
		/// <param name="entry">The entry.</param>
		protected override void WriteEntry(ILogEntry entry)
		{
			if (!_queue.IsAddingCompleted)
			{
				_queue.Add(entry);
			}
		}

		private void ThreadExecute()
		{
			// Reset the stop event.
			_stopEvent.Reset();

			while (!_queue.IsCompleted)
			{
				ILogEntry entry = null;
				if (_queue.TryTake(out entry, Timeout.Infinite))
				{
					// Write the entry to all sinks.
					foreach(LogSink sink in this.Sinks)
					{
						sink.Write(entry);
					}
				}
			}

			// Set the stop event.
			_stopEvent.Set();
		}
	}
}
