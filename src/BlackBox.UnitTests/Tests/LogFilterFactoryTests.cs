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
using System.Reflection;

namespace BlackBox.UnitTests.Tests
{
	[TestFixture]
	public class LogFilterFactoryTests
	{
		[Test]
		public void LogFilterFactory_CreateFactoryWithoutAssemblies()
		{
			LogFilterFactory factory = new LogFilterFactory();
			Assert.IsNotNull(factory.Types);
			Assert.AreEqual(2, factory.Types.Count);

			// The standard sinks should be available.
			Assert.IsTrue(factory.Types.ContainsKey("levelmatch"));
            Assert.IsTrue(factory.Types.ContainsKey("levelrange"));

			// But not the unit test sinks.
			Assert.IsFalse(factory.Types.ContainsKey("denyall"));
		}

		[Test]
		public void LogFilterFactory_CreateFactoryWithOnlyExternalAssemblies()
		{
			List<Assembly> assemblies = new List<Assembly> { this.GetType().Assembly };
			LogFilterFactory factory = new LogFilterFactory(assemblies);
			Assert.IsNotNull(factory.Types);
			Assert.AreEqual(3, factory.Types.Count);

			// The standard sinks should be available.
			Assert.IsTrue(factory.Types.ContainsKey("levelmatch"));
            Assert.IsTrue(factory.Types.ContainsKey("levelrange"));

			// And also the unit test sinks.
			Assert.IsTrue(factory.Types.ContainsKey("denyall"));
		}

		[Test]
		public void LogFilterFactory_BuildFilterHostedInBlackBoxAssembly()
		{
			LogFilterFactory factory = new LogFilterFactory();
			var filter = factory.Build("LevelMatch");
			Assert.IsNotNull(filter);
			Assert.IsInstanceOf<LevelMatchFilter>(filter);
		}

		[Test]
		public void LogFilterFactory_BuildFilterHostedInExternalAssembly()
		{
			LogFilterFactory factory = new LogFilterFactory(new List<Assembly> { this.GetType().Assembly });
			var filter = factory.Build("DenyAll");
			Assert.IsNotNull(filter);
			Assert.IsInstanceOf<DenyAllFilter>(filter);
		}

		[Test]
		public void LogFilterFactory_BuildFilterWithArguments()
		{
			LogFilterFactory factory = new LogFilterFactory();
			Dictionary<string, string> arguments = new Dictionary<string, string>
			{
				{ "Level", "Fatal" }
			};

			var filter = factory.Build("LevelMatch", arguments);
			Assert.IsNotNull(filter);
			Assert.IsInstanceOf<LevelMatchFilter>(filter);
			Assert.AreEqual(LogLevel.Fatal, ((LevelMatchFilter)filter).Level);
		}
	}
}
