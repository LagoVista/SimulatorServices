// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 10efa7206054a6cf4231b3ae5a11dc16c73cb7bb0f90228b2a6bf9b30660113f
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.IoT.Logging.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace LagoVista.IoT.Simulator.Runtime.App
{
    class Program
    {
        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            var result = new Dictionary<string, string>();
            foreach(var arg in args)
            {
                if(!arg.StartsWith('-'))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid command line argument: " + arg);
                    return null;
                }

                var seperator = arg.IndexOf('=');
                if(seperator < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid command line argument: " + arg);
                    return null;
                }

                var key = arg.Substring(1, seperator - 1);
                var value = arg.Substring(seperator + 1);

                result.Add(key, value);
            }

            return result;
        }



        static async Task Main(string[] args)
        {
            var parms = ParseArgs(args);
            var errs = new StringBuilder();

            if (!parms.ContainsKey("orgid")) errs.AppendLine("Missing -uid argument - Organization Id");
            if (!parms.ContainsKey("org")) errs.AppendLine("Missing -usrname argument - Organization Name");
            if (!parms.ContainsKey("uid")) errs.AppendLine("Missing -uid argument - User Id");
            if (!parms.ContainsKey("usrname")) errs.AppendLine("Missing -usrname argument - User Name");
            if (!parms.ContainsKey("id")) errs.AppendLine("Missing -id argument - Id of the Simulator Network");
            if (!parms.ContainsKey("key")) errs.AppendLine("Missing -keyargument - Simulator Network Key");

            if (errs.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(errs.ToString());
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"Starting simulator network for: {parms["id"]}");

            var org = EntityHeader.Create(parms["orgid"], parms["org"]);
            var usr = EntityHeader.Create(parms["uid"], parms["usrname"]);
            var id = parms["id"];
            var key = parms["key"];

            var mgr = new SimulatorRuntimeManager(new SimulatorRuntimeServicesFactory(),new AdminLogger(new LogWriter()));
            mgr.Publisher = new NotificationPublisher();
            await mgr.InitAsync(id, key, org, usr, Core.Interfaces.Environments.Development);

            Console.ReadKey();
        }
    }
}
