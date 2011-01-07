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
	public sealed class FormatPattern<TContext>
	{
		private readonly FormatRenderer<TContext>[] _renderers;
		private readonly string _format;

		internal FormatRenderer<TContext>[] Renderers
		{
			get { return _renderers; }
		}

		public string Format
		{
			get { return _format; }
		} 

		internal FormatPattern(string format, FormatRenderer<TContext>[] renderers)
		{
			_format = format;
			_renderers = renderers;
		}

		internal static FormatPattern<TContext> Create(IServiceLocator locator, string pattern)
		{
			// Resolve the format pattern factory.
			FormatPatternFactory<TContext> factory = locator.Resolve<FormatPatternFactory<TContext>>();
			if (factory == null)
			{
				// The factory could not be resolved from the service locator.
				string message = string.Format(CultureInfo.InvariantCulture, "Could not resolve format pattern factory for context type '{0}'.", typeof(TContext).Name);
				throw new InvalidOperationException(message);
			}

			// Create the format pattern.
			return factory.Create(pattern);
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
