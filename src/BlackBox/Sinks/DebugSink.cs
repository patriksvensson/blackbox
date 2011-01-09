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
using System.Diagnostics;

namespace BlackBox
{
    /// <summary>
    /// Log sink that writes messages to the debug output.
    /// </summary>
	[LogSinkType("debug")]
	public sealed class DebugSink : FormatLogSink
	{
        /// <summary>
        /// Writes a message to the trace listeners in 
        /// the <see cref="System.Diagnostics.Debug.Listeners"/> collection.
        /// </summary>
        /// <param name="entry">The entry.</param>
		protected override void WriteEntry(ILogEntry entry)
		{
			Debug.WriteLine(this.FormatEntry(entry));
		}
	}
}
