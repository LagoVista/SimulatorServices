// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 5470d98dd6ae40af32f7e7a80254c2e60b66bb140fd343b7ce7c23fe77f1918f
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Logging.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public class LogWriter : ILogWriter
    {
        public Task WriteError(LogRecord record)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("---------------------Error-----------------------------");
            Console.WriteLine(record.Message);
            Console.WriteLine(record.StackTrace);
            Console.WriteLine("---------------------Error-----------------------------");
            Console.WriteLine();
            Console.ResetColor();

            return Task.FromResult(default(object));
        }

        public Task WriteEvent(LogRecord record)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"---------------------{record.LogLevel}-----------------------------");
            Console.WriteLine(record.Message);
            Console.WriteLine($"---------------------{record.LogLevel}-----------------------------");
            Console.WriteLine();
            Console.ResetColor();

            return Task.FromResult(default(object));
        }
    }
}
