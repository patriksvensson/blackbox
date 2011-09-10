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

using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Configuration
{
	[TestFixture]
	public class LogConfigurationTests
	{
		[Test]
		public void InternalLogger_ConfigurationHasInternalLogger()
		{
			LogConfiguration configuration = new LogConfiguration();
			Assert.IsNotNull(configuration.InternalLogger, "The internal logger should not be null.");
		}

		[Test]
		public void LogConfiguration_LoadConfigurationFromDefaultConfigSection()
		{
			LogConfiguration configuration = LogConfiguration.FromConfigSection();
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.AreEqual("BlackBox.ConsoleSink", configuration.Sinks[0].Name);
		}

		[Test]
		public void LogConfiguration_LoadConfigurationFromCustomConfigSection()
		{
			LogConfiguration configuration = LogConfiguration.FromConfigSection("CustomBlackBox");
			Assert.IsNotNull(configuration);
			Assert.AreEqual(1, configuration.Sinks.Count);
			Assert.AreEqual("Custom.ConsoleSink", configuration.Sinks[0].Name);
		}

		[Test]
		public void LogConfiguration_Clone()
		{
			LogConfiguration configuration = new LogConfiguration();
			LogConfiguration clonedConfiguration = new LogConfiguration();
			Assert.AreNotSame(configuration, clonedConfiguration);
		}
	}
}
