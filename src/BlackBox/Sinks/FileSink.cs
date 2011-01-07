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
using System.IO;
using BlackBox.Formatting;

namespace BlackBox
{
	[LogSinkType("file")]
	public sealed class FileSink : FormatLogSink
	{
		private FormatPattern<ILogEntry> _fileNamePattern;
		private LruCache<string, TextWriter> _fileCache;
		private readonly int _fileCacheSize;
		private string _fileName;
		private readonly object _lock;
		private bool _disposed;

		#region Properties

		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}

		#endregion

		#region Construction

		public FileSink()
			: base()
		{
			_fileCacheSize = 5;
			_fileName = "$(time:format='yyyy-MM-dd').log";
			_lock = new object();

		}

		#endregion

		#region Initialization

		protected internal override void InitializeSink(IServiceLocator locator)
		{
			lock (_lock)
			{
				// Call the base class so the format message sink gets properly initialized.
				base.InitializeSink(locator);

				// Create the LRU file cache and the file name pattern.
				_fileCache = new LruCache<string, TextWriter>(_fileCacheSize);
				_fileNamePattern = FormatPattern<ILogEntry>.Create(locator, _fileName);

				// Subscribe to the item removed event.
				_fileCache.ItemRemoved += new EventHandler<EventArgs<TextWriter>>(_fileCache_ItemRemoved);
			}
		}

		#endregion

		#region IDisposable Members

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!_disposed)
				{
					lock (_lock)
					{
						// Got a cache?
						if (_fileCache != null)
						{
							_fileCache.Dispose();
							_fileCache = null;
						}

						_disposed = true;
					}
				}
			}

			// Call the base class implementation.            
			base.Dispose(disposing);
		}

		#endregion

		#region LRU Cache Events

		private void _fileCache_ItemRemoved(object sender, EventArgs<TextWriter> e)
		{
			if (e.Item != null)
			{
				// When the text writer is removed, we're not going to use it anymore.
				// Close the text writer and set the instance to null.
				e.Item.Close();
			}
		}

		#endregion

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		protected override void WriteEntry(ILogEntry entry)
		{
			lock (_lock)
			{
				if (_disposed)
				{
					throw new ObjectDisposedException("FileSink");
				}

				// Get the filename.
				string filename = _fileNamePattern.Render(entry);

				// Get the text writer from the LRU cache.
				TextWriter writer = _fileCache.Get(filename);
				if (writer == null)
				{
					if (!File.Exists(filename))
					{
						// Get the directory name.
						string directory = Path.GetDirectoryName(filename);
						if (!Directory.Exists(directory))
						{
							// Create the directory.
							Directory.CreateDirectory(directory);
						}
					}

					// Create the text writer.
					writer = new StreamWriter(File.Open(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));

					// Add the text writer to the file cache.
					_fileCache.Put(filename, writer);
				}

				// Write the log message to the writer.
				writer.WriteLine(this.FormatEntry(entry));

				// Flush the writer.
				writer.Flush();
			}
		}
	}
}
