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

using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Extensions
{
	[TestFixture]
	public class LogFilterExtensionsTests
	{
		#region Private Nested Test Classes

		private class FakeLogFilterWithoutAttribute : LogFilter
		{
			protected internal override LogFilterResult Evaluate(ILogEntry entry)
			{
				return LogFilterResult.Accept;
			}
		}

		#endregion

		[Test]
		public void LogFilterExtensions_GetFilterType()
		{
			ConditionFilter filter = new ConditionFilter();
			LogFilterTypeAttribute attribute = filter.GetFilterType();
			Assert.IsNotNull(attribute);
			Assert.AreEqual("condition", attribute.Name);
		}

		[Test]
		public void LogFilterExtensions_GetFilterTypeReturnsNullIfAttributeWasNotFound()
		{
			FakeLogFilterWithoutAttribute filter = new FakeLogFilterWithoutAttribute();
			LogFilterTypeAttribute attribute = filter.GetFilterType();
			Assert.IsNull(attribute);
		}
	}
}
