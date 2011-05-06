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
using System.Threading;

namespace BlackBox
{
	internal sealed class LogEntry : ILogEntry
	{
		private readonly DateTimeOffset _timestamp;
		private readonly string _message;
		private readonly Exception _exception;
		private readonly ILogger _logger;
		private readonly LogLevel _level;
		private readonly int _threadId;
		private readonly string _threadName;
		private readonly Type _source;

		#region Properties

		public Exception Exception
		{
			get { return _exception; }
		}

		internal ILogger Logger
		{
			get { return _logger; }
		}

		public Type Source
		{
			get { return _source; }
		}

		public LogLevel Level
		{
			get { return _level; }
		}

		public int ThreadId
		{
			get { return _threadId; }
		}

		public string ThreadName
		{
			get { return _threadName; }
		}

		public DateTimeOffset Timestamp
		{
			get { return _timestamp; }
		}

		public string Message
		{
			get { return _message; }
		}

		#endregion

		internal LogEntry(DateTimeOffset timestamp, LogLevel logLevel, string message, ILogger logger, Exception exception)
		{
			_timestamp = timestamp;
			_level = logLevel;
			_message = message;
			_logger = logger;
			_exception = exception;
			_source = logger.Source;

			// Get thread information.
			Thread currentThread = Thread.CurrentThread;
			_threadName = currentThread.Name;
			_threadId = currentThread.ManagedThreadId;
		}
	}
}
