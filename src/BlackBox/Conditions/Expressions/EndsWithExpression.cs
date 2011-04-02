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

namespace BlackBox.Conditions
{
    [MethodExpression("ends-with", 2)]
    internal class EndsWithExpression : MethodExpression
    {
        internal EndsWithExpression(ConditionExpression[] arguments)
            : base(arguments)
        {
        }

        internal override object Evaluate(ILogEntry context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            string actual = this.Arguments[0].Evaluate(context) as string;
            string expected = this.Arguments[1].Evaluate(context) as string;

            if (actual != null && expected != null)
            {
                // Check if the actual string ends with the expected one.
                return actual.EndsWith(expected, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
