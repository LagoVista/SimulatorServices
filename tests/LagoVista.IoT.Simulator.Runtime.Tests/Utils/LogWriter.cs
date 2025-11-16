// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 9155aad19b5f5d8eb85e7b49f5f81ff6e9d5956fc4bf7f10eeeb43773b686bad
// IndexVersion: 2
// --- END CODE INDEX META ---
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
