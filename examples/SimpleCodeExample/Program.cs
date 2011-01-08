using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackBox;

namespace SimpleCodeExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create the configuration.
            LogConfiguration configuration = new LogConfiguration();

            // Add a console sink to the configuration.
            configuration.Sinks.Add(new ConsoleSink { Format = "$(time(format='HH:mm:ss')): $(message())" });

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
