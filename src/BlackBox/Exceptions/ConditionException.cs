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

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace BlackBox
{
	/// <summary>
	/// Represent errors that occur in condition expressions.
	/// </summary>
	[Serializable]
	public sealed class ConditionException : BlackBoxException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConditionException"/> class.
		/// </summary>
		public ConditionException()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConditionException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public ConditionException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConditionException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public ConditionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConditionException"/> class.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public ConditionException(string format, params object[] args)
			: base(string.Format(CultureInfo.InvariantCulture, format, args))
		{
		}

		private ConditionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
