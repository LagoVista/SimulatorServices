using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Logging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime.Tests.Utils
{
    public class LogWriter : ILogWriter
    {
        public Task WriteError(LogRecord record)
        {
            Console.WriteLine("---------------------Error-----------------------------");
            Console.WriteLine(record.Message);
            Console.WriteLine(record.StackTrace);
            Console.WriteLine("---------------------Error-----------------------------");
            Console.WriteLine();

            return Task.FromResult(default(object));
        }

        public Task WriteEvent(LogRecord record)
        {
            Console.WriteLine($"---------------------{record.LogLevel}-----------------------------");
            Console.WriteLine(record.Message);
            Console.WriteLine($"---------------------{record.LogLevel}-----------------------------");
            Console.WriteLine();

            return Task.FromResult(default(object));
        }
    }
}
