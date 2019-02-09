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
            services.AddTransient<ISimulatorNetworkManager, SimulatorNetworkManager>();
        }

        public static void ConfigureSLWIOC()
        {
            SLWIOC.Register<ISimulatorManager, SimulatorManager>();
            SLWIOC.Register<ISimulatorNetworkManager, SimulatorNetworkManager>();
        }
    }
}
