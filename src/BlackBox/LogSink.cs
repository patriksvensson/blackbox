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

namespace BlackBox
{
	/// <summary>
	/// The base class for a log sink.
	/// </summary>
	public abstract class LogSink : IDisposable
	{
		private string _name;
		private readonly LogFilterCollection _filters;
		private bool _isInitialized;

		#region Properties

		/// <summary>
		/// Gets or sets the name of the log sink.
		/// </summary>
		/// <value>The name.</value>
		[SkipSerializationAttribute]
		public string Name
		{
			get { return _name; }
			set
			{
				if (_isInitialized)
				{
					// We want to avoid sinks getting renamed after the system have been started.
					throw new BlackBoxException("Cannot change the name of the sink after initialization.");
				}

				_name = value;
			}
		}

		/// <summary>
		/// Gets the log filters associated with this sink.
		/// </summary>
		/// <value>The filters.</value>
		[SkipSerializationAttribute]
		public LogFilterCollection Filters
		{
			get { return _filters; }
		}

		/// <summary>
		/// Gets a value indicating whether this sink has been initialized.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this sink has been initialized; otherwise, <c>false</c>.
		/// </value>
		[SkipSerializationAttribute]
		public bool IsInitialized
		{
			get { return _isInitialized; }
		}

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LogSink"/> class.
		/// </summary>
		protected LogSink()
		{
			_filters = new LogFilterCollection();
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
		}

		#endregion

		internal virtual void PerformInitialization(InitializationContext context)
		{
			// Initalize all filters.
			this.Filters.Initialize(context);

			// Perform custom initialization for this sink.
			this.Initialize(context);

			// We're now properly initialized.
			_isInitialized = true;
		}

		/// <summary>
		/// Initializes the log sink.
		/// </summary>
		/// <param name="context">The context.</param>
		protected internal virtual void Initialize(InitializationContext context)
		{
		}

		/// <summary>
		/// Writes the specified entry to the log sink.
		/// </summary>
		/// <param name="entry">The entry.</param>
		public void Write(ILogEntry entry)
		{
			if (this.Filters.Evaluate(entry) != LogFilterResult.Filter)
			{
				this.WriteEntry(entry);
			}
		}

		/// <summary>
		/// Writes the specified entries to the log sink.
		/// </summary>
		/// <param name="entries">The entries.</param>
		public void Write(ILogEntry[] entries)
		{
			if (entries == null)
			{
				throw new ArgumentNullException("entries");
			}

			List<ILogEntry> entryList = new List<ILogEntry>();
			foreach (ILogEntry entry in entries)
			{
				if (this.Filters.Evaluate(entry) != LogFilterResult.Filter)
				{
					entryList.Add(entry);
				}
			}

			if (entryList.Count > 0)
			{
				this.WriteEntries(entryList.ToArray());
			}
		}

		/// <summary>
		/// Performs the writing of the specified entry to the log sink.
		/// </summary>
		/// <param name="entry">The entry.</param>
		protected abstract void WriteEntry(ILogEntry entry);

		/// <summary>
		/// Performs the writing of the specified entries to the log sink.
		/// </summary>
		/// <param name="entries">The entries.</param>
		protected virtual void WriteEntries(ILogEntry[] entries)
		{
			if (entries == null)
			{
				throw new ArgumentNullException("entries");
			}

			foreach (ILogEntry entry in entries)
			{
				this.Write(entry);
			}
		}
	}
}
