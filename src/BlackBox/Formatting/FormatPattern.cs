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

using System.Text;

namespace BlackBox.Formatting
{
	/// <summary>
	/// Represents a format pattern.
	/// </summary>
	public sealed class FormatPattern
	{
		private readonly FormatRenderer[] _renderers;
		private readonly string _format;

		internal FormatRenderer[] Renderers
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
		/// Initializes a new instance of the <see cref="FormatPattern"/> class.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="renderers">The renderers.</param>
		internal FormatPattern(string format, FormatRenderer[] renderers)
		{
			_format = format;
			_renderers = renderers;
		}

		internal string Render(ILogEntry context)
		{
			StringBuilder accumulator = new StringBuilder();
			foreach (FormatRenderer renderer in _renderers)
			{
				accumulator.Append(renderer.Render(context));
			}
			return accumulator.ToString();
		}
	}
}
