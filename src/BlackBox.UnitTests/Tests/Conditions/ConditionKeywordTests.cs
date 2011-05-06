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

namespace BlackBox.UnitTests.Tests.Conditions
{
	[TestFixture]
	public class ConditionKeywordTests : ConditionExpressionTests
	{
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

		[Test]
		public void ConditionExpression_LoggerExpressionShouldEvaluateToFalse()
		{
			Assert.AreEqual(false, this.Evaluate("logger=='BlackBox.UnitTests.Tests.Conditions.Testing'", typeof(ConditionExpressionTests)));
		}

		[Test]
		public void ConditionExpression_LoggerExpressionShouldEvaluateToTrue()
		{
			Assert.AreEqual(true, this.Evaluate("logger=='BlackBox.UnitTests.Tests.Conditions.ConditionExpressionTests'", typeof(ConditionExpressionTests)));
		}

		[Test]
		public void ConditionExpression_LoggerNameExpressionShouldEvaluateToFalse()
		{
			Assert.AreEqual(false, this.Evaluate("loggername=='Testing'", typeof(ConditionExpressionTests)));
		}

		[Test]
		public void ConditionExpression_LoggerNameExpressionShouldEvaluateToTrue()
		{
			Assert.AreEqual(true, this.Evaluate("loggername=='ConditionExpressionTests'", typeof(ConditionExpressionTests)));
		}
	}
}
