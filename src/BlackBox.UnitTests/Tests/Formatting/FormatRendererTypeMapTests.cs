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
using BlackBox.Formatting;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Formatting
{
	[TestFixture]
	public class FormatRendererTypeMapTests
	{
		[Test]
		public void FormatRendererTypeMap_BuildTimeRenderer()
		{
			var typeMap = new FormatRendererTypeMap();
			Type type = typeMap.GetRendererType("time");
			Assert.IsNotNull(type);
			Assert.AreEqual(typeof(TimeRenderer), type);
		}
	}
}
