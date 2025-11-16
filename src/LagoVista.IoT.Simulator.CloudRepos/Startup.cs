// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 7157f08cec51e1c7ba5c42a616082cde422f3bfd0508f2e081a337cef8260936
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Simulator.Admin.Repos;
using LagoVista.IoT.Simulator.CloudRepos.Repos;

namespace LagoVista.IoT.Simulator.CloudRepos
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISimulatorRepo, SimulatorRepo>();
            services.AddTransient<ISimulatorNetworkRepo, SimulatorNetworkRepo>();
        }
    }
}
