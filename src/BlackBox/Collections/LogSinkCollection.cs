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
	public sealed class LogSinkCollection : IEnumerable<LogSink>, IDisposable
	{
		private readonly List<LogSink> _sinks;
		private bool _disposed;        

		public int Count
		{
			get { return _sinks.Count; }
		}

		internal LogSink this[int index]
		{
			get { return _sinks[index]; }
		}

		public LogSinkCollection()
		{
			_sinks = new List<LogSink>();
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

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

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		public void Add(LogSink sink)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			_sinks.Add(sink);
		}

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

		public bool Remove(LogSink sink)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			return _sinks.Remove(sink);
		}

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
