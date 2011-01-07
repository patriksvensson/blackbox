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
	[LogFilterType("levelmatch")]
	public sealed class LevelMatchFilter : LogFilter
	{
		private LogLevel _level;
		private bool _levelHasBeenSet;

		public LogLevel Level
		{
			get { return _level; }
			set
			{
				_level = value;
				_levelHasBeenSet = true;
			}
		}

		protected internal override void Initialize(IServiceLocator locator)
		{
			if (!_levelHasBeenSet) 
			{
				throw new BlackBoxException("Level filter have no level set.");
			}
		}

		protected internal override LogFilterResult Evaluate(ILogEntry entry)
		{
			if (entry != null)
			{
				return entry.Level == this.Level
					? LogFilterResult.Accept
					: LogFilterResult.Filter;
			}
			else
			{
				return LogFilterResult.Neutral;
			}
		}
	}
}
