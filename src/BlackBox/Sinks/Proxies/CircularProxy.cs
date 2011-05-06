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
