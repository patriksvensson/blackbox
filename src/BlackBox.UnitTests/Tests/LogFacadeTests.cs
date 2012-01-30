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

namespace BlackBox.UnitTests.Tests
{
	[TestFixture]
	public class LogFacadeTests
	{
		[Test]
		public void LogFacade_Configure_Accepts_Valid_Configuration()
		{
			LogFacade facade = new LogFacade();
			LogConfiguration configuration = new LogConfiguration();
			facade.Configure(configuration);
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(ArgumentNullException))]
		public void LogFacade_Configure_Throws_If_Configuration_Is_Null()
		{
			LogFacade facade = new LogFacade();
			facade.Configure((LogConfiguration)null);
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(BlackBoxException), ExpectedMessage = "The log facade has not been configured.")]
		public void LogFacade_GetLogger_Throws_If_Kernel_Has_Not_Been_Configured()
		{
			LogFacade facade = new LogFacade();
			facade.GetLogger();
		}

		[Test]
		[ExpectedException(ExpectedException=typeof(ArgumentNullException))]
		public void LogFacade_GetLogger_Throws_If_Logger_Type_Is_Null()
		{
			LogFacade facade = new LogFacade();
			LogConfiguration configuration = new LogConfiguration();
			facade.GetLogger((Type)null);
		}

		[Test]
		public void LogFacade_Can_Infer_Logger_Source_From_Call_stack()
		{
			LogFacade facade = new LogFacade();
			LogConfiguration configuration = new LogConfiguration();
			facade.Configure(configuration);
			ILogger logger = facade.GetLogger();
			Assert.AreEqual(typeof(LogFacadeTests), logger.Source);
		}

		[Test]
		public void LogFacade_Will_Return_Logger_For_Expected_Source()
		{
			LogFacade facade = new LogFacade();
			LogConfiguration configuration = new LogConfiguration();
			facade.Configure(configuration);
			ILogger logger = facade.GetLogger(typeof(System.String));
			Assert.AreEqual(typeof(System.String), logger.Source);
		}
	}
}
