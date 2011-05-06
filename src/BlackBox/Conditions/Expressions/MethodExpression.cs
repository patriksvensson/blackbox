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
using System.Linq;
using BlackBox;
using System.Globalization;

namespace BlackBox.Conditions
{
    internal abstract class MethodExpression : ConditionExpression
    {
        private readonly ConditionExpression[] _arguments;

        internal ConditionExpression[] Arguments
        {
            get { return _arguments; }
        }

        protected MethodExpression(ConditionExpression[] arguments)
        {
            _arguments = arguments;
        }

        public sealed override string ToString()
        {
            // Get the method epxression attribute.
            string methodName = string.Empty;
            MethodExpressionAttribute attribute = this.GetType().GetAttribute<MethodExpressionAttribute>(false);
            methodName = attribute != null ? attribute.Name : "unknown-method";

            if (_arguments.Length > 0)
            {
                var arguments = _arguments.Select(x => x.ToString()).ToArray();
                return string.Concat(methodName, "(", string.Join(", ", arguments), ")");
            }
            else
            {
                return string.Concat(methodName, "()");
            }
        }
    }
}