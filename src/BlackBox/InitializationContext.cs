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
using System.Reflection;
using BlackBox.Formatting;

namespace BlackBox
{
	/// <summary>
	/// The BlackBox initialization context.
	/// More or less a container for stuff that's needed while initializing a filter or a sink.
	/// </summary>
	public sealed class InitializationContext : IDisposable
	{
		private readonly FormatPatternFactory _formatPatternFactory;

		/// <summary>
		/// Gets the format pattern factory.
		/// </summary>
		/// <value>The format pattern factory.</value>
		public FormatPatternFactory FormatPatternFactory
		{
			get { return _formatPatternFactory; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InitializationContext"/> class.
		/// </summary>
		/// <param name="assemblies">The assemblies.</param>
		internal InitializationContext(IEnumerable<Assembly> assemblies)
		{
			// Create the format pattern factory.
			_formatPatternFactory = new FormatPatternFactory(
				new FormatRendererTypeMap(assemblies));
		}

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}

		#endregion
	}
}
