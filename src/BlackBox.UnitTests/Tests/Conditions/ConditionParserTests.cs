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

using BlackBox.Conditions;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Conditions
{
	[TestFixture]
	public class ConditionParserTests
    {
        private ConditionFactory _factory;

        #region Private Helper Methods
        private ConditionExpression ParseCondition(string condition)
        {
            return _factory.ParseCondition(condition);
        }
        #endregion

        #region Test Setup

        [TestFixtureSetUp]
        public void Setup()
        {
            _factory = new ConditionFactory();
        }

        #endregion

        [Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Unknown keyword 'invalid' encountered.")]
		public void ConditionParser_ParseExpressionWithUnknownKeyword()
		{
			this.ParseCondition("invalid>4");
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Expected token but encountered end of expression.")]
		public void ConditionParser_ParseIncompleteExpression_MissingRightBinaryExpression()
		{
            this.ParseCondition("1>");
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Expected token but encountered end of expression.")]
		public void ConditionParser_ParseIncompleteExpression_MissingExpressionAfterAnd()
		{
            this.ParseCondition("1>1 AND");
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Invalid token encountered when parsing literal expression.")]
		public void ConditionParser_ParseIncompleteExpression_2()
		{
            this.ParseCondition("1> AND 1==1");
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Expected to find token of type 'ClosingParenthesis' but found null instead.")]
		public void ConditionParser_ParseIncompleteExpression_MissingClosingParenthesis()
		{
            this.ParseCondition("(1>1");
		}

        [Test]
        [ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Method expression for type 'BlackBox.Conditions.LengthMethodExpression' expects 1 argument(s) but got 2.")]
        public void ConditionParser_ParseIncompleteExpression_TooManyArgumentsToMethod()
        {
            this.ParseCondition("length('test', 'test2')");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Method expression for type 'BlackBox.Conditions.LengthMethodExpression' expects 1 argument(s) but got 0.")]
        public void ConditionParser_ParseIncompleteExpression_TooFewArgumentsToMethod()
        {
            this.ParseCondition("length()");
        }
	}
}
