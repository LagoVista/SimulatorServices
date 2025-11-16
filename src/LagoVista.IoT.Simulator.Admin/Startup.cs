// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 0e25093a970824fc3f6976d15320a90e747530b3c00cb90715c37db96e460e7f
// IndexVersion: 2
// --- END CODE INDEX META ---
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
