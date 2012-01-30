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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace BlackBox
{
	/// <summary>
	/// A singleton log facade.
	/// </summary>
	public sealed class LogFacade
	{
		private ILogKernel _kernel;

		#region Singleton Implementation

		static readonly LogFacade _instance = new LogFacade();

		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static LogFacade()
		{
		}

		/// <summary>
		/// The singleton log facade instance.
		/// </summary>
		public static LogFacade Instance
		{
			get { return _instance; }
		}

		#endregion

		internal LogFacade()
		{
		}

		/// <summary>
		/// Configures the log facade.
		/// </summary>
		/// <param name="configuration"></param>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public void Configure(LogConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			LogKernel kernel = new LogKernel(configuration);
			Interlocked.Exchange(ref _kernel, kernel);
		}

		/// <summary>
		/// Gets the logger for the calling type.
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ILogger GetLogger()
		{
			// Get the type of the caller.
			StackFrame frame = new StackFrame(1, false);
			Type type = frame.GetMethod().DeclaringType;
			return this.GetLogger(type);
		}

		/// <summary>
		/// Gets the logger for the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public ILogger GetLogger(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (_kernel == null)
			{
				string message = "The log facade has not been configured.";
				throw new BlackBoxException(message);
			}
			return _kernel.GetLogger(type);
		}
	}
}
