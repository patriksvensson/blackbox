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
    /// Represents the available log levels.
    /// </summary>
    public enum LogLevel : int
    {
        /// <summary>
        /// Fatal log level.
        /// </summary>
        Fatal = 0,
        /// <summary>
        /// Error log level. 
        /// </summary>
        Error = 1,
        /// <summary>
        /// Warning log level.
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Information log level.
        /// </summary>
        Information = 3,
        /// <summary>
        /// Verbose log level.
        /// </summary>
        Verbose = 4,
        /// <summary>
        /// Debug log level.
        /// </summary>
        Debug = 5
    }
}
