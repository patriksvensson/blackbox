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

namespace BlackBox.Conditions
{
	internal abstract class BinaryExpression : ConditionExpression
	{
		private readonly ConditionExpression _left;
		private readonly ConditionExpression _right;

		internal ConditionExpression Left
		{
			get { return _left; }
		}

		internal ConditionExpression Right
		{
			get { return _right; }
		}

		protected BinaryExpression(ConditionExpression left, ConditionExpression right)
		{
			_left = left;
			_right = right;
		}
	}
}
