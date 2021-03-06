﻿//
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

namespace BlackBox
{
	/// <summary>
	/// The base class for a log sink proxy.
	/// </summary>
	public abstract class LogSinkProxy : LogSink
	{
		private readonly LogSinkCollection _sinks;
		private bool _disposed;

		/// <summary>
		/// Gets the nested sinks.
		/// </summary>
		/// <value>The sinks.</value>
		[SkipSerializationAttribute]
		public LogSinkCollection Sinks
		{
			get { return _sinks; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogSinkProxy"/> class.
		/// </summary>
		protected LogSinkProxy()
		{
			_sinks = new LogSinkCollection();
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!_disposed)
				{
					if (_sinks != null)
					{
						_sinks.Dispose();
					}
					_disposed = true;
				}
			}
			base.Dispose(disposing);
		}

		internal override void PerformInitialization(InitializationContext context)
		{
			// Initialize the child sinks.
			this.Sinks.Initialize(context);

			// Call the base class implementation.
			base.PerformInitialization(context);
		}
	}
}
