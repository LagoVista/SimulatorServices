// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: b71cf96ce95f63bd7c11812d91fb7b8f547b225c853f54ce71d2941d3bd9ea41
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Logging.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime.Portal.Services
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
            /*
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"---------------------{record.LogLevel}-----------------------------");
            Console.WriteLine(record.Message);
            Console.WriteLine($"---------------------{record.LogLevel}-----------------------------");
            Console.WriteLine();
            Console.ResetColor();
            */
            return Task.FromResult(default(object));
        }
    }
}
