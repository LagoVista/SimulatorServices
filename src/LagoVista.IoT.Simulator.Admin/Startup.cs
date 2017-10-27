using LagoVista.Core.IOC;
using LagoVista.IoT.Simulator.Admin.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

using System.Resources;

[assembly: NeutralResourcesLanguage("en")]

namespace LagoVista.IoT.Simulator.Admin
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISimulatorManager, SimulatorManager>();
        }

        public static void ConfigureSLWIOC()
        {
            SLWIOC.Register<ISimulatorManager, SimulatorManager>();
        }

    }
}
