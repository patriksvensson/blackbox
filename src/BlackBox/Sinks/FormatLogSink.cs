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
    /// <summary>
    /// Base class for a log sink that supports a format pattern.
    /// </summary>
	public abstract class FormatLogSink : LogSink
	{
		private FormatPattern _format;
		private string _formatString;

        /// <summary>
        /// Gets or sets the format used in the format pattern.
        /// </summary>
        /// <value>The format.</value>
		public string Format
		{
			get { return _formatString; }
			set { _formatString = value; }
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatLogSink"/> class.
        /// </summary>
		protected FormatLogSink()
			: base()
		{
			_formatString = "$(message())";
		}

		/// <summary>
		/// Initializes the log sink.
		/// </summary>
		/// <param name="context"></param>
		protected internal override void Initialize(InitializationContext context)
		{
			#region Sanity Check
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			#endregion

			_format = context.FormatPatternFactory.Create(_formatString);
		}

        /// <summary>
        /// Performs formatting of the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
		protected string FormatEntry(ILogEntry entry)
		{
			return _format.Render(entry);
		}
	}
}
