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

using BlackBox.Conditions;
using System;

namespace BlackBox
{
	/// <summary>
	/// Log filter that matches when the specified condition evaluates to true.
	/// </summary>
	[LogFilterType("condition")]
	public sealed class ConditionFilter : LogFilter
	{
		private ConditionExpression _expression;

		/// <summary>
		/// Gets or sets the condition that will be used by the filter.
		/// </summary>
		/// <value>The condition.</value>
		public string Condition { get; set; }

		/// <summary>
		/// Gets or sets the action that will be used if the
		/// condition evaluates to true.
		/// </summary>
		/// <value>The action.</value>
		public LogFilterResult Action { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ConditionFilter"/> class.
		/// </summary>
		public ConditionFilter()
		{
			this.Action = LogFilterResult.Filter;
		}

		/// <summary>
		/// Initializes the log filter.
		/// </summary>
		/// <param name="context"></param>
		protected internal override void Initialize(InitializationContext context)
		{
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
			if (string.IsNullOrEmpty(this.Condition))
			{
				throw new BlackBoxException("The filter condition has not been set.");
			}

            // Parse the expression.
            _expression = context.ConditionFactory.ParseCondition(this.Condition);
		}

		/// <summary>
		/// Evaluates the specified entry against the log filter.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		protected internal override LogFilterResult Evaluate(ILogEntry entry)
		{
			return (bool)_expression.Evaluate(entry)
				? this.Action : LogFilterResult.Neutral;
		}
	}
}
