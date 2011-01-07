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
using NUnit.Framework;
using BlackBox.Conditions;
using BlackBox.Formatting;

namespace BlackBox.UnitTests.Tests.Conditions
{
	[TestFixture]
	public class ConditionParserTests
	{
		[Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Unknown keyword 'invalid' encountered.")]
		public void ConditionParser_ParseExpressionWithUnknownKeyword()
		{
			ConditionExpression expression = ConditionParser.ParseCondition("invalid>4");
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Expected token but encountered end of expression.")]
		public void ConditionParser_ParseIncompleteExpression_MissingRightBinaryExpression()
		{
			ConditionExpression expression = ConditionParser.ParseCondition("1>");
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Expected token but encountered end of expression.")]
		public void ConditionParser_ParseIncompleteExpression_MissingExpressionAfterAnd()
		{
			ConditionExpression expression = ConditionParser.ParseCondition("1>1 AND");
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Invalid token encountered when parsing literal expression.")]
		public void ConditionParser_ParseIncompleteExpression_2()
		{
			ConditionExpression expression = ConditionParser.ParseCondition("1> AND 1==1");
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Expected to find token of type 'ClosingParenthesis' but found null instead.")]
		public void ConditionParser_ParseIncompleteExpression_MissingClosingParenthesis()
		{
			ConditionExpression expression = ConditionParser.ParseCondition("(1>1");
		}
	}
}
