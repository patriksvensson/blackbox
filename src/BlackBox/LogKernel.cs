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
using System.Threading;
using BlackBox.Formatting;
using System.Diagnostics;

namespace BlackBox
{
	/// <summary>
	/// The BlackBox logging kernel.
	/// </summary>
	public sealed class LogKernel : ILogKernel, IDisposable
	{        
		private readonly ReaderWriterLockSlim _lock;
		private readonly ServiceProvider _serviceProvider;
		private readonly FormatRendererTypeMap<ILogEntry> _formatRendererTypeMap;
		private readonly FormatPatternFactory<ILogEntry> _formatPatternFactory;
		private readonly LoggerCollection _loggers;
		private LogConfiguration _configuration;
		private bool _disposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogKernel"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public LogKernel(LogConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration", "Log configuration cannot be null.");
			}

			_serviceProvider = new ServiceProvider();
			_formatRendererTypeMap = new FormatRendererTypeMap<ILogEntry>();
			_formatPatternFactory = new FormatPatternFactory<ILogEntry>(_formatRendererTypeMap);
			_lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
			_loggers = new LoggerCollection();

			// Register services.
			_serviceProvider.Register<FormatPatternFactory<ILogEntry>>(_formatPatternFactory);

			// Configure the kernel.
			this.Configure(configuration);
		}

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
		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_configuration.Dispose();

					if (_lock != null)
					{
						_lock.Dispose();
					}
				}
				_disposed = true;
			}
		}

		#endregion

		private void Configure(LogConfiguration configuration)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			using (_lock.AcquireWriteLock())
			{
				if (_configuration != null)
				{
					// TODO: Dispose the configuration here.
					throw new NotImplementedException("Configuration should be disposed.");
				}
		  
				// Set the active configuration.
				_configuration = configuration ?? new LogConfiguration();

				// Initialize all filters.
				_configuration.Filters.Initialize(_serviceProvider);

				// Initialize all sinks.
				_configuration.Sinks.Initialize(_serviceProvider);
			}
		}

		/// <summary>
		/// Gets the logger for the calling type.
		/// </summary>
		/// <returns></returns>
		public ILogger GetLogger()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			// Get the type of the caller.
			StackFrame frame = new StackFrame(1, false);
			Type type = frame.GetMethod().DeclaringType;
			return GetLogger(type);
		}

		/// <summary>
		/// Gets the logger for the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public ILogger GetLogger(Type type)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			using (_lock.AcquireWriteLock())
			{
				if (_loggers.Contains(type))
				{
					return _loggers[type];
				}
				else
				{
					Logger logger = new Logger(this, type);
					_loggers.Add(logger);
					return logger;
				}
			}
		}

		internal void Write(LogLevel level, ILogger logger, Exception exception, string message)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().FullName);
			}

			using (_lock.AcquireReadLock())
			{
				// Create the log entry.
				LogEntry entry = new LogEntry(DateTimeOffset.Now, level, message, logger, exception);

				// Write the entry to all sinks.
				foreach (LogSink sink in _configuration.Sinks)
				{
					sink.Write(entry);
				}
			}
		}
	}
}
