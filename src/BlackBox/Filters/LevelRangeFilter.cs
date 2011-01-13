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
	/// Log filter that matches when the log level is within a specific range.
	/// </summary>
	[LogFilterType("levelrange")]
	public sealed class LevelRangeFilter : LogFilter
	{
		private LogLevel _minLevel;
		private LogLevel _maxLevel;
		private bool _minLevelHasBeenSet;
		private bool _maxLevelHasBeenSet;

		/// <summary>
		/// Gets or sets the minimum level.
		/// </summary>
		/// <value>The min level.</value>
		public LogLevel MinLevel
		{
			get { return _minLevel; }
			set
			{
				_minLevel = value;
				_minLevelHasBeenSet = true;
			}
		}

		/// <summary>
		/// Gets or sets the maximum level.
		/// </summary>
		/// <value>The max level.</value>
		public LogLevel MaxLevel
		{
			get { return _maxLevel; }
			set
			{
				_maxLevel = value;
				_maxLevelHasBeenSet = true;
			}
		}

		/// <summary>
		/// Gets or sets the action that will be used if the
		/// log entry's log level is in the specified range.
		/// </summary>
		/// <value>The action.</value>
		public LogFilterResult Action { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LevelRangeFilter"/> class.
		/// </summary>
		public LevelRangeFilter()
		{
			this.Action = LogFilterResult.Filter;
		}

		/// <summary>
		/// Initializes the log filter.
		/// </summary>
		/// <param name="locator">The locator.</param>
		protected internal override void Initialize(IServiceLocator locator)
		{
			if (!_minLevelHasBeenSet)
			{
				throw new BlackBoxException("The minimum log level has not been set.");
			}
			if (!_maxLevelHasBeenSet)
			{
				throw new BlackBoxException("The maximum log level has not been set.");
			}
			if (_minLevel > _maxLevel)
			{
				throw new BlackBoxException("The minimum log level must be less or equal to the maximum log level.");
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
				if (entry.Level >= _minLevel && entry.Level <= _maxLevel)
				{
					return this.Action;
				}
				else
				{
					return LogFilterResult.Neutral;
				}
			}
			else
			{
				return LogFilterResult.Neutral;
			}
		}
	}
}
