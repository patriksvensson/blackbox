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

namespace BlackBox
{
	/// <summary>
	/// The log entry contract interface.
	/// </summary>
	public interface ILogEntry
	{
		/// <summary>
		/// Gets the associated exception.
		/// </summary>
		/// <value>The exception.</value>
		Exception Exception { get; }

		/// <summary>
		/// Gets the log level.
		/// </summary>
		/// <value>The level.</value>
		LogLevel Level { get; }

		/// <summary>
		/// Gets the log message.
		/// </summary>
		/// <value>The message.</value>
		string Message { get; }

		/// <summary>
		/// Gets the logger source.
		/// </summary>
		/// <value>The source.</value>
		Type Source { get; }

		/// <summary>
		/// Gets the id of the thread that the log entry was created on.
		/// </summary>
		/// <value>The thread id.</value>
		int ThreadId { get; }

		/// <summary>
		/// Gets the name of the thread that the log entry was created on.
		/// </summary>
		/// <value>The name of the thread.</value>
		string ThreadName { get; }

		/// <summary>
		/// Gets the time when the log entry was created.
		/// </summary>
		/// <value>The timestamp.</value>
		DateTimeOffset Timestamp { get; }
	}
}
