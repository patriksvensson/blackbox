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
using System.Globalization;

namespace BlackBox
{
	internal sealed class Logger : ILogger
	{
		private readonly LogKernel _kernel;
		private readonly Type _source;

		public Type Source
		{
			get { return _source; }
		}

		internal Logger(LogKernel kernel, Type type)
		{
			_kernel = kernel;
			_source = type;
		}

		public void Write(LogLevel level, string message)
		{
			_kernel.Write(level, this, null, message);
		}

		public void Write(LogLevel level, string format, params object[] args)
		{
			_kernel.Write(level, this, null, string.Format(CultureInfo.InvariantCulture, format, args));
		}

		public void Write(LogLevel level, IFormatProvider provider, string format, params object[] args)
		{
			_kernel.Write(level, this, null, string.Format(provider, format, args));
		}

		public void Write(LogLevel level, Exception exception)
		{
			_kernel.Write(level, this, exception, string.Empty);
		}

		public void Write(LogLevel level, Exception exception, string message)
		{
			_kernel.Write(level, this, exception, message);
		}

		public void Write(LogLevel level, Exception exception, IFormatProvider provider, string format, params object[] args)
		{
			_kernel.Write(level, this, exception, string.Format(provider, format, args));
		}

		public void Write(LogLevel level, Exception exception, string format, params object[] args)
		{
			_kernel.Write(level, this, exception, string.Format(CultureInfo.InvariantCulture, format, args));
		}
	}
}
