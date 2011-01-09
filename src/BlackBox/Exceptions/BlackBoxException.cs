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
using System.Runtime.Serialization;
using System.Globalization;

namespace BlackBox
{
    /// <summary>
    /// Represent errors that occur in BlackBox.
    /// </summary>
    [Serializable]
    public class BlackBoxException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlackBoxException"/> class.
        /// </summary>
        public BlackBoxException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlackBoxException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BlackBoxException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlackBoxException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BlackBoxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlackBoxException"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public BlackBoxException(string format, params object[] args)
            : base(string.Format(CultureInfo.InvariantCulture, format, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlackBoxException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected BlackBoxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
