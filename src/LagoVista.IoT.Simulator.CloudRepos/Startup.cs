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
        }
    }
}
