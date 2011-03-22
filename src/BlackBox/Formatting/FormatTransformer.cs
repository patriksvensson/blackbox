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

namespace BlackBox.Formatting
{
	/// <summary>
	/// Represents a format transformer that is used to transform
	/// the output of a <see cref="BlackBox.Formatting.FormatRenderer"/>.
	/// </summary>
    public abstract class FormatTransformer : FormatRenderer
    {
        private readonly FormatRenderer _renderer;

        /// <summary>
        /// Gets the renderer whose value we're transforming.
        /// </summary>
        /// <value>The renderer.</value>
        internal FormatRenderer Renderer
        {
            get { return _renderer; }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="FormatTransformer"/> class.
		/// </summary>
		/// <param name="renderer">The renderer.</param>
        protected FormatTransformer(FormatRenderer renderer)
        {
            _renderer = renderer;
        }

        /// <summary>
        /// Renders the format pattern part using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public sealed override string Render(ILogEntry context)
        {
            return this.Transform(_renderer.Render(context));
        }

        /// <summary>
        /// Transforms the specified text, which is the output of a format renderer.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public abstract string Transform(string source);
    }
}
