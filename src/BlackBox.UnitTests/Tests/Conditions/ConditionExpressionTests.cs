﻿//
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
    public class ConditionExpressionTests
    {
        #region Private Helper Methods

        private bool Evaluate(string condition)
        {
            return this.Evaluate(condition, "", LogLevel.Information, null);
        }

        private bool Evaluate(string condition, string message)
        {
            return this.Evaluate(condition, message, LogLevel.Information, null);
        }

        private bool Evaluate(string condition, LogLevel level)
        {
            return this.Evaluate(condition, "", level, null);
        }

        private bool Evaluate(string condition, Exception exception)
        {
            return this.Evaluate(condition, "", LogLevel.Information, exception);
        }

        private bool Evaluate(string condition, string message, LogLevel level, Exception exception)
        {
            ConditionExpression expression = ConditionParser.ParseCondition(condition);
            Logger logger = new Logger(null, typeof(ConditionExpressionTests));
            ILogEntry entry = new LogEntry(DateTimeOffset.Now, level, message, logger, exception);
            return (bool)expression.Evaluate(entry);
        }

        #endregion

        [Test]
        public void ConditionExpression_EqualsExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("1==2"));
        }

        [Test]
        public void ConditionExpression_EqualsExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("1==1"));
        }

        [Test]
        public void ConditionExpression_GreaterThanExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("1>2"));
        }

        [Test]
        public void ConditionExpression_GreaterThanExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("2>1"));
        }

        [Test]
        public void ConditionExpression_GreaterThanOrEqualsExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("1>=2"));
        }

        [Test]
        public void ConditionExpression_GreaterThanOrEqualsExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("1>=1"));
            Assert.AreEqual(true, this.Evaluate("2>=1"));
        }

        [Test]
        public void ConditionExpression_LessThanOrEqualsExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("2<=1"));
        }

        [Test]
        public void ConditionExpression_LessThanOrEqualsExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("1<=1"));
            Assert.AreEqual(true, this.Evaluate("1<=2"));
        }

        [Test]
        public void ConditionExpression_LessThanExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("2<1"));
        }

        [Test]
        public void ConditionExpression_LessThanExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("1<2"));
        }

        [Test]
        public void ConditionExpression_NotExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("!True"));
        }

        [Test]
        public void ConditionExpression_NotExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("!False"));
        }

        [Test]
        public void ConditionExpression_OrExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("2==1 OR 1==2"));
        }

        [Test]
        public void ConditionExpression_OrExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("1==1 OR 1==2"));
        }

        [Test]
        public void ConditionExpression_AndExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("1==1 AND 1==2"));
        }

        [Test]
        public void ConditionExpression_AndExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("1==1 AND 2==2"));
        }

        [Test]
        public void ConditionExpression_MessageExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("message=='testing'", "testing"));
        }

        [Test]
        public void ConditionExpression_MessageExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("message=='testin'", "testing"));
        }

        [Test]
        public void ConditionExpression_LogLevelExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("level==1", LogLevel.Information));
        }

        [Test]
        public void ConditionExpression_LogLevelExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("level==3", LogLevel.Information));
        }

        [Test]
        public void ConditionExpression_LogLevelNameExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("levelname=='Fatal'", LogLevel.Information));
        }

        [Test]
        public void ConditionExpression_LogLevelNameExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("levelname=='Information'", LogLevel.Information));
        }

        [Test]
        public void ConditionExpression_HasExceptionExpressionShouldEvaluateToFalse()
        {
            Assert.AreEqual(false, this.Evaluate("has-exception==true", (Exception)null));
        }

        [Test]
        public void ConditionExpression_HasExceptionExpressionShouldEvaluateToTrue()
        {
            Assert.AreEqual(true, this.Evaluate("has-exception==true", new ArgumentException()));
        }
    }
}
