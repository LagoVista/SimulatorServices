using LagoVista.IoT.Simulator.Admin.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Admin
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISimulatorManager, SimulatorManager>();
        }
    }
}
