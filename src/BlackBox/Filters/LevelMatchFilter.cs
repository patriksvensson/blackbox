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
    /// <summary>
    /// Log filter that matches when the log level is equal to the specified log level.
    /// </summary>
	[LogFilterType("levelmatch")]
	public sealed class LevelMatchFilter : LogFilter
	{
		private LogLevel _level;
		private bool _levelHasBeenSet;

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>The level.</value>
		public LogLevel Level
		{
			get { return _level; }
			set
			{
				_level = value;
				_levelHasBeenSet = true;
			}
		}

        /// <summary>
        /// Gets or sets the action that will be used if the
        /// log entry's log level match the provided log level.
        /// </summary>
        /// <value>The action.</value>
        public LogFilterResult Action { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelMatchFilter"/> class.
        /// </summary>
        public LevelMatchFilter()
        {
            // Set a default value for the level.
            _level = LogLevel.Information;

            // Filter log entries by default.
            this.Action = LogFilterResult.Filter;
        }

        /// <summary>
        /// Initializes the log filter.
        /// </summary>
        /// <param name="locator">The locator.</param>
		protected internal override void Initialize(IServiceLocator locator)
		{
			if (!_levelHasBeenSet) 
			{
				throw new BlackBoxException("The log level has not been set.");
			}
		}

        /// <summary>
        /// Evaluates the specified entry against the log filter.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
		protected internal override LogFilterResult Evaluate(ILogEntry entry)
		{
			if (entry != null)
			{
                return (entry.Level == this.Level)
                    ? this.Action : LogFilterResult.Neutral;
			}
			else
			{
				return LogFilterResult.Neutral;
			}
		}
	}
}
