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
	internal sealed class ConstantExpression : ConditionExpression
	{
		private readonly object _value;

		internal object Value
		{
			get { return _value; }
		}

		internal ConstantExpression(object value)
		{
			_value = value;
		}

		internal override object Evaluate(ILogEntry context)
		{
			return _value;
		}

		public override string ToString()
		{
			if(_value == null)
			{
				return "null";
			}
			else if (_value is string)
			{
				return string.Concat("'", _value.ToString(), "'");
			}
			else
			{
				return _value.ToString();
			}
		}
	}
}
