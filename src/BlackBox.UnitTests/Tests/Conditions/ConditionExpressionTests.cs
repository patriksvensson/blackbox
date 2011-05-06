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
using BlackBox.Conditions;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Conditions
{
	[TestFixture]
	public class ConditionExpressionTests
	{
        private ConditionFactory _factory;

		#region Private Helper Methods

		protected bool Evaluate(string condition)
		{
			return this.Evaluate(condition, "", LogLevel.Information, null, null);
		}

		protected bool Evaluate(string condition, string message)
		{
			return this.Evaluate(condition, message, LogLevel.Information, null, null);
		}

		protected bool Evaluate(string condition, LogLevel level)
		{
			return this.Evaluate(condition, "", level, null, null);
		}

		protected bool Evaluate(string condition, Exception exception)
		{
			return this.Evaluate(condition, "", LogLevel.Information, null, exception);
		}

		protected bool Evaluate(string condition, Type loggerType)
		{
			return this.Evaluate(condition, "", LogLevel.Information, loggerType, null);
		}

		protected bool Evaluate(string condition, string message, LogLevel level, Type loggerType, Exception exception)
		{
            ConditionExpression expression = _factory.ParseCondition(condition);
			Logger logger = new Logger(null, loggerType ?? typeof(ConditionExpressionTests));
			ILogEntry entry = new LogEntry(DateTimeOffset.Now, level, message, logger, exception);
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
	}
}
