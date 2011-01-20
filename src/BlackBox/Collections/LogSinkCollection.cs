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
using System.Collections.ObjectModel;

namespace BlackBox
{
    /// <summary>
    /// Represents a strongly typed list of log sinks that can be accessed by index.
    /// </summary>
	public sealed class LogSinkCollection : IEnumerable<LogSink>, IDisposable
	{
		private readonly List<LogSink> _sinks;
		private bool _disposed;

        /// <summary>
        /// Gets the number of log sinks contained in the collection.
        /// </summary>
        /// <value>The count.</value>
		public int Count
		{
			get { return _sinks.Count; }
		}

        /// <summary>
        /// Gets the <see cref="BlackBox.LogSink"/> at the specified index.
        /// </summary>
        /// <value></value>
		internal LogSink this[int index]
		{
			get { return _sinks[index]; }
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSinkCollection"/> class.
        /// </summary>
		public LogSinkCollection()
		{
			_sinks = new List<LogSink>();
		}

		#region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					foreach (LogSink sink in _sinks)
					{
						sink.Dispose();
					}
					this.Clear();
				}
				_disposed = true;
			}
		}

		#endregion

		#region IEnumerable<LogSink> Members

        /// <summary>
        /// Gets the enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
		public IEnumerator<LogSink> GetEnumerator()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			return _sinks.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Initialization

		internal void Initialize(IServiceLocator locator)
		{
			// Iterate through all sinks in the collection.
			foreach (LogSink sink in _sinks)
			{
				// Perform the initialization.
				sink.PerformInitialization(locator);
			}
		}

		#endregion

		/// <summary>
        /// Adds the specified log sink to the collection.
        /// </summary>
        /// <param name="sink">The sink.</param>
		public void Add(LogSink sink)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			_sinks.Add(sink);
		}

        /// <summary>
        /// Adds the specified log sinks to the current collection.
        /// </summary>
        /// <param name="sinks">The sinks.</param>
		public void AddRange(IEnumerable<LogSink> sinks)
		{
			if (sinks != null)
			{
				foreach (LogSink sink in sinks)
				{
					_sinks.Add(sink);
				}
			}
		}

        /// <summary>
        /// Removes the specified log sink from the collection.
        /// </summary>
        /// <param name="sink">The sink.</param>
        /// <returns></returns>
		public bool Remove(LogSink sink)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			return _sinks.Remove(sink);
		}

        /// <summary>
        /// Removes all log sinks from the collection.
        /// </summary>
		public void Clear()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			_sinks.Clear();
		}
	}
}
