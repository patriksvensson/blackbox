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

namespace BlackBox.UnitTests.Tests.Conditions
{
    [TestFixture]
    public class ConditionTokenizerTests
    {
        [Test]
        public void ConditionTokenizer_ParseCondition()
        {
            string condition = "(1==2) OR (3<2 OR 1>=1)";
            ConditionTokenizer tokenizer = new ConditionTokenizer(new StringBuffer(condition));
            ConditionToken[] tokens = tokenizer.Tokenize();
            Assert.AreEqual(15, tokens.Length);
            Assert.AreEqual(ConditionTokenType.OpeningParenthesis, tokens[0].TokenType);
            Assert.AreEqual(ConditionTokenType.Number, tokens[1].TokenType);
            Assert.AreEqual(ConditionTokenType.EqualTo, tokens[2].TokenType);
            Assert.AreEqual(ConditionTokenType.Number, tokens[3].TokenType);
            Assert.AreEqual(ConditionTokenType.ClosingParenthesis, tokens[4].TokenType);
            Assert.AreEqual(ConditionTokenType.Or, tokens[5].TokenType);
            Assert.AreEqual(ConditionTokenType.OpeningParenthesis, tokens[6].TokenType);
            Assert.AreEqual(ConditionTokenType.Number, tokens[7].TokenType);
            Assert.AreEqual(ConditionTokenType.LessThan, tokens[8].TokenType);
            Assert.AreEqual(ConditionTokenType.Number, tokens[9].TokenType);
            Assert.AreEqual(ConditionTokenType.Or, tokens[10].TokenType);
            Assert.AreEqual(ConditionTokenType.Number, tokens[11].TokenType);
            Assert.AreEqual(ConditionTokenType.GreaterThanOrEqualTo, tokens[12].TokenType);
            Assert.AreEqual(ConditionTokenType.Number, tokens[13].TokenType);
            Assert.AreEqual(ConditionTokenType.ClosingParenthesis, tokens[14].TokenType);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Unexpected token '$' encountered.")]
        public void ConditionTokenizer_ParseConditionWithInvalidToken()
        {
            string condition = "(1==2) OR (3$2 OR 1>=1)";
            ConditionTokenizer tokenizer = new ConditionTokenizer(new StringBuffer(condition));
            ConditionToken[] tokens = tokenizer.Tokenize();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ConditionException), ExpectedMessage = "Unterminated string literal.")]
        public void ConditionTokenizer_ParseConditionWithUnterminatedStringLiteral()
        {
            string condition = "'aa'=='aa";
            ConditionTokenizer tokenizer = new ConditionTokenizer(new StringBuffer(condition));
            ConditionToken[] tokens = tokenizer.Tokenize();
        }
    }
}
