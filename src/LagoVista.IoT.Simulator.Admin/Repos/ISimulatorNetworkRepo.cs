using LagoVista.IoT.Simulator.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Admin.Repos
{
    public interface ISimulatorNetworkRepo
    {
        Task AddSimulatorNetworkAsync(SimulatorNetwork network);
        Task DeleteSimulatorNetworkAsync(string id);
        Task<SimulatorNetwork> GetSimulatorNetworkAsync(string id);
        Task<IEnumerable<SimulatorNetworkSummary>> GetSimulatorNetworksForOrgAsync(string orgId);
        Task UpdateSimulatorNetworkAsync(SimulatorNetwork network);
        Task<bool> QueryKeyInUseAsync(string key, string org);
    }
}
