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
    /// Contains extension methods for <see cref="BlackBox.ILogger"/>.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Creates a new log entry with the log level 'Fatal'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void Fatal(this ILogger logger, string message)
        {
            logger.Write(LogLevel.Fatal, message);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Fatal'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Fatal(this ILogger logger, string format, params object[] args)
        {
            logger.Write(LogLevel.Fatal, format, args);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Error'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void Error(this ILogger logger, string message)
        {
            logger.Write(LogLevel.Error, message);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Error'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Error(this ILogger logger, string format, params object[] args)
        {
            logger.Write(LogLevel.Error, format, args);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Warning'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void Warning(this ILogger logger, string message)
        {
            logger.Write(LogLevel.Warning, message);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Warning'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Warning(this ILogger logger, string format, params object[] args)
        {
            logger.Write(LogLevel.Warning, format, args);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Information'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void Information(this ILogger logger, string message)
        {
            logger.Write(LogLevel.Information, message);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Information'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Information(this ILogger logger, string format, params object[] args)
        {
            logger.Write(LogLevel.Information, format, args);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Verbose'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void Verbose(this ILogger logger, string message)
        {
            logger.Write(LogLevel.Verbose, message);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Verbose'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Verbose(this ILogger logger, string format, params object[] args)
        {
            logger.Write(LogLevel.Verbose, format, args);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Debug'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void Debug(this ILogger logger, string message)
        {
            logger.Write(LogLevel.Debug, message);
        }

        /// <summary>
        /// Creates a new log entry with the log level 'Debug'.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Debug(this ILogger logger, string format, params object[] args)
        {
            logger.Write(LogLevel.Debug, format, args);
        }
    }
}
