// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 7157f08cec51e1c7ba5c42a616082cde422f3bfd0508f2e081a337cef8260936
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Simulator.Admin.Models;
using LagoVista.IoT.Simulator.Admin.Repos;
using LagoVista.IoT.Simulator.CloudRepos.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

namespace LagoVista.DependencyInjection
{
    public static class SimulatorServices
    {
        public static void AddSimulatorServicesModule(this IServiceCollection services, IConfigurationRoot configRoot, IAdminLogger logger)
        {
            LagoVista.IoT.Simulator.CloudRepos.Startup.ConfigureServices(services);
            LagoVista.IoT.Simulator.Admin.Startup.ConfigureServices(services);
            services.AddMetaDataHelper<Simulator>();
        }
    }
}

