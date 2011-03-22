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
using System.Globalization;

namespace BlackBox.Formatting
{
	/// <summary>
	/// Represents a format pattern.
	/// </summary>
	/// <typeparam name="TContext">The type of the context.</typeparam>
	public sealed class FormatPattern<TContext>
	{
		private readonly FormatRenderer<TContext>[] _renderers;
		private readonly string _format;

		internal FormatRenderer<TContext>[] Renderers
		{
			get { return _renderers; }
		}

		/// <summary>
		/// Gets the format that was used to create the format pattern.
		/// </summary>
		/// <value>The format.</value>
		public string Format
		{
			get { return _format; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FormatPattern&lt;TContext&gt;"/> class.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="renderers">The renderers.</param>
		internal FormatPattern(string format, FormatRenderer<TContext>[] renderers)
		{
			_format = format;
			_renderers = renderers;
		}

		internal string Render(TContext context)
		{
			StringBuilder accumulator = new StringBuilder();
			foreach (FormatRenderer<TContext> renderer in _renderers)
			{
				accumulator.Append(renderer.Render(context));
			}
			return accumulator.ToString();
		}
	}
}
