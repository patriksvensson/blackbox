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

namespace BlackBox
{
	/// <summary>
	/// Log sink proxy that is used to group log sinks together.
	/// </summary>
	[LogSinkType("funnel")]
	public sealed class FunnelProxy : LogSinkProxy
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FunnelProxy"/> class.
		/// </summary>
		public FunnelProxy()
		{
		}

		/// <summary>
		/// Performs the writing of the specified entry to the nested log sinks.
		/// </summary>
		/// <param name="entry">The entry.</param>
		protected override void WriteEntry(ILogEntry entry)
		{
			foreach (LogSink sink in this.Sinks)
			{
				sink.Write(entry);
			}
		}

		/// <summary>
		/// Performs the writing of the specified entries to the nested log sinks.
		/// </summary>
		/// <param name="entries">The entries.</param>
		protected override void WriteEntries(ILogEntry[] entries)
		{
			foreach (LogSink sink in this.Sinks)
			{
				sink.Write(entries);
			}
		}
	}
}
