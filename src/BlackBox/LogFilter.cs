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
    /// The base class for a log filter.
    /// </summary>
    public abstract class LogFilter
    {
        /// <summary>
        /// Initializes the log filter.
        /// </summary>
        /// <param name="locator">The locator.</param>
        protected internal virtual void Initialize(IServiceLocator locator)
        {
        }

        /// <summary>
        /// Evaluates the specified entry against the log filter.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        protected internal abstract LogFilterResult Evaluate(ILogEntry entry);
    }
}
