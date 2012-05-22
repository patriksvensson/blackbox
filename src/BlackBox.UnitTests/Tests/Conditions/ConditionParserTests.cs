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

        #region Test Setup

        [TestFixtureSetUp]
        public void Setup()
        {
            _factory = new ConditionFactory();
        }

        #endregion

        [Test]
		public void ConditionParser_ParseExpressionWithUnknownKeyword()
		{
            Assert.That(() => _factory.ParseCondition("invalid>4"),
                Throws.Exception.TypeOf<ConditionException>()
                .With.Property("Message").EqualTo("Unknown keyword 'invalid' encountered."));
		}

		[Test]
		public void ConditionParser_ParseIncompleteExpression_MissingRightBinaryExpression()
		{
            Assert.That(() => _factory.ParseCondition("1>"),
                Throws.Exception.TypeOf<ConditionException>()
                .With.Property("Message").EqualTo("Expected token but encountered end of expression."));

		}

		[Test]
		public void ConditionParser_ParseIncompleteExpression_MissingExpressionAfterAnd()
		{
            Assert.That(() => _factory.ParseCondition("1>1 AND"),
                Throws.Exception.TypeOf<ConditionException>()
                .With.Property("Message").EqualTo("Expected token but encountered end of expression."));
		}

		[Test]
		public void ConditionParser_ParseIncompleteExpression_2()
		{
            Assert.That(() => _factory.ParseCondition("1> AND 1==1"),
                Throws.Exception.TypeOf<ConditionException>()
                .With.Property("Message").EqualTo("Invalid token encountered when parsing literal expression."));
		}

		[Test]
		public void ConditionParser_ParseIncompleteExpression_MissingClosingParenthesis()
		{
            Assert.That(() => _factory.ParseCondition("(1>1"),
                Throws.Exception.TypeOf<ConditionException>()
                .With.Property("Message").EqualTo("Expected to find token of type 'ClosingParenthesis' but found null instead."));
		}

        [Test]
        public void ConditionParser_ParseIncompleteExpression_TooManyArgumentsToMethod()
        {
            Assert.That(() => _factory.ParseCondition("length('test', 'test2')"),
                Throws.Exception.TypeOf<ConditionException>()
                .With.Property("Message").EqualTo("Method expression for type 'BlackBox.Conditions.LengthMethodExpression' expects 1 argument(s) but got 2."));
        }

        [Test]
        public void ConditionParser_ParseIncompleteExpression_TooFewArgumentsToMethod()
        {
            Assert.That(() => _factory.ParseCondition("length()"),
                Throws.Exception.TypeOf<ConditionException>()
                .With.Property("Message").EqualTo("Method expression for type 'BlackBox.Conditions.LengthMethodExpression' expects 1 argument(s) but got 0."));
        }
	}
}
