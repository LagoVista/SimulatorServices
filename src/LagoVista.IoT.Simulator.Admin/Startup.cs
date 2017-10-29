using LagoVista.Core.Interfaces;
using LagoVista.Core.IOC;
using LagoVista.IoT.Simulator.Admin.Managers;

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
