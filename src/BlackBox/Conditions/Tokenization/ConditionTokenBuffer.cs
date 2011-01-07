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
using System.Globalization;

namespace BlackBox.Conditions
{
	internal sealed class ConditionTokenBuffer : Buffer<ConditionToken>
	{
		internal ConditionTokenBuffer(IEnumerable<ConditionToken> tokens)
			: base(tokens)
		{
		}

		internal ConditionToken ExpectToken(ConditionTokenType tokenType)
		{
			return this.ExpectToken(tokenType, false);
		}

		internal ConditionToken ExpectToken(ConditionTokenType tokenType, bool consume)
		{
			if (this.Current == null)
			{
				// We've reached the end of the buffer.
				var message = string.Format(CultureInfo.InvariantCulture, "Expected to find token of type '{0}' but found null instead.", tokenType);
				throw new ConditionException(message);
			}
			if (this.Current == null || this.Current.TokenType != tokenType)
			{
				var message = string.Format(CultureInfo.InvariantCulture, "Expected to find token of type '{0}' but found '{1}' instead.", tokenType, this.Current.TokenType);
				throw new ConditionException(message);
			}

			ConditionToken token = this.Current;
			if (consume)
			{
				// Consume the current token.
				this.Read();
			}
			return token;
		}
	}
}
