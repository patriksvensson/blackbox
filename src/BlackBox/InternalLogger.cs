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

using System.Diagnostics;

namespace BlackBox
{
	internal sealed class InternalLogger : IInternalLogger
	{
		public bool Enabled { get; set; }
		public LogLevel Threshold { get; set; }

		public InternalLogger()
		{
			this.Enabled = false;
			this.Threshold = LogLevel.Verbose;
		}

		public void Write(LogLevel level, string message)
		{
			if (this.Enabled && this.Threshold >= level)
			{
				Trace.WriteLine(string.Concat("[BLACKBOX] ", message));
			}
		}
	}
}
