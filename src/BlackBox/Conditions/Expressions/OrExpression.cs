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

namespace BlackBox.Conditions
{
    internal sealed class OrExpression : BinaryExpression
    {
        internal OrExpression(ConditionExpression left, ConditionExpression right)
            : base(left, right)
        {
        }

        internal override object Evaluate(ILogEntry context)
        {
            // Left side true?
            bool leftResult = (bool)this.Left.Evaluate(context);
            if (leftResult)
            {
                return true;
            }

            // Right side true?            
            bool rightResult = (bool)this.Right.Evaluate(context);
            if (rightResult)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Concat("(", this.Left, " OR ", this.Right, ")");
        }
    }
}
