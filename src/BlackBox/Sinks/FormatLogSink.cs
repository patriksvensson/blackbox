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
using BlackBox.Formatting;

namespace BlackBox
{
	public abstract class FormatLogSink : LogSink
	{
		private FormatPattern<ILogEntry> _format;
		private string _formatString;

		public string Format
		{
			get { return _formatString; }
			set { _formatString = value; }
		}

		protected FormatLogSink()
			: base()
		{
		}

		protected internal override void InitializeSink(IServiceLocator locator)
		{
			_format = FormatPattern<ILogEntry>.Create(locator, _formatString);
		}

		protected string FormatEntry(ILogEntry entry)
		{
			return _format.Render(entry);
		}
	}
}
