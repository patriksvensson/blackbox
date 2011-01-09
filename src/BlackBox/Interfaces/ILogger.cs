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
    /// The logger contract interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets the logger source type.
        /// </summary>
        /// <value>The source.</value>
        Type Source { get; }

        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The message.</param>
        void Write(LogLevel level, string message);

        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The format arguments.</param>
        void Write(LogLevel level, string format, params object[] args);

        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The format arguments.</param>
        void Write(LogLevel level, IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="exception">The exception.</param>
        void Write(LogLevel level, Exception exception);

        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        void Write(LogLevel level, Exception exception, string message);

        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="provider">The format provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The format arguments.</param>
        void Write(LogLevel level, Exception exception, IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The format arguments.</param>
        void Write(LogLevel level, Exception exception, string format, params object[] args);
    }
}
