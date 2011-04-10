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
using BlackBox.Conditions;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Conditions
{
    public class MethodExpressionTests
    {
        private ConditionFactory _factory;

        #region Private Helper Methods
        private bool Evaluate(string condition)
        {
            ConditionExpression expression = _factory.ParseCondition(condition);
            Logger logger = new Logger(null, typeof(ConditionExpressionTests));
            ILogEntry entry = new LogEntry(DateTimeOffset.Now, LogLevel.Information, "Hello World", logger, null);
            return (bool)expression.Evaluate(entry);
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
        public void ConditionExpression_LengthExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("length('hello')==4"));
        }

        [Test]
        public void ConditionExpression_LengthExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("length('hello')==5"));
        }

        [Test]
        public void ConditionExpression_StartsWithExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("starts-with('hello', 'ha')"));
        }

        [Test]
        public void ConditionExpression_StartsWithExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("starts-with('hello', 'he')"));
        }

        [Test]
        public void ConditionExpression_ContainsExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("contains('hello', 'al')"));
        }

        [Test]
        public void ConditionExpression_ContainsExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("contains('hello', 'el')"));
        }

        [Test]
        public void ConditionExpression_EndsWithExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("ends-with('hello', 'ol')"));
        }

        [Test]
        public void ConditionExpression_EndsWithExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("ends-with('hello', 'lo')"));
        }

        [Test]
        public void ConditionExpression_EqualsExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("equals('hello', 'al')"));
        }

        [Test]
        public void ConditionExpression_EqualsExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("equals('hello', 'hello')"));
        }
    }
}
