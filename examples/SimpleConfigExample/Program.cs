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
using BlackBox;

namespace SimpleConfigExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create the configuration that resides in App.Config.
            LogConfiguration configuration = LogConfiguration.FromConfigSection("BlackBox");

            // Create the log kernel and create a logger.
            LogKernel kernel = new LogKernel(configuration);
            ILogger logger = kernel.GetLogger();

            // Write a message to the logger.
            logger.Write(LogLevel.Information, "Hello World!");

            // Wait for the user to press a key.
            Console.WriteLine("Press ANY key to quit.");
            Console.ReadKey(true);
        }
    }
}
