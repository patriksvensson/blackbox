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
