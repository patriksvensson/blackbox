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

using System.Collections;
using System.Globalization;

namespace BlackBox.Conditions
{
	internal sealed class RelationalExpression : BinaryExpression
	{
		private readonly RelationalOperator _operator;

		internal RelationalOperator Operator
		{
			get { return _operator; }
		}

		internal RelationalExpression(ConditionExpression left, ConditionExpression right, RelationalOperator binaryOperator)
			: base(left, right)
		{
			_operator = binaryOperator;
		}

		internal override object Evaluate(ILogEntry context)
		{
			object leftResult = this.Left.Evaluate(context);
			object rightResult = this.Right.Evaluate(context);

			Comparer comparer = Comparer.Default;

			if (_operator == RelationalOperator.EqualTo)
			{
				return comparer.Compare(leftResult, rightResult) == 0;
			}
			else if (_operator == RelationalOperator.NotEqualTo)
			{
				return comparer.Compare(leftResult, rightResult) != 0;
			}
			else if (_operator == RelationalOperator.GreaterThan)
			{
				return comparer.Compare(leftResult, rightResult) > 0;
			}
			else if (_operator == RelationalOperator.GreaterThanOrEqualTo)
			{
				return comparer.Compare(leftResult, rightResult) >= 0;
			}
			else if (_operator == RelationalOperator.LessThan)
			{
				return comparer.Compare(leftResult, rightResult) < 0;
			}
			else if (_operator == RelationalOperator.LessThanOrEqualTo)
			{
				return comparer.Compare(leftResult, rightResult) <= 0;
			}

			// We don't know what this operator is.
			string message = string.Format(CultureInfo.InvariantCulture, "Unknown operator '{0}'.", _operator);
			throw new ConditionException(message);
		}

		public override string ToString()
		{
			string operatorName = string.Empty;
			switch (_operator)
			{
				case RelationalOperator.EqualTo: operatorName = "=="; break;
				case RelationalOperator.NotEqualTo: operatorName = "!="; break;
				case RelationalOperator.GreaterThan: operatorName = ">"; break;
				case RelationalOperator.GreaterThanOrEqualTo: operatorName = ">="; break;
				case RelationalOperator.LessThan: operatorName = "<"; break;
				case RelationalOperator.LessThanOrEqualTo: operatorName = "<="; break;
				default:
					// We don't know what this operator is.
					operatorName = "?"; break;
			}

			// Concatenate the 
			return string.Concat("(", this.Left, " ", operatorName, " ", this.Right, ")");
		}
	}
}
