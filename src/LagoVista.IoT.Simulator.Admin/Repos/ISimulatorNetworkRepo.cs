// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 12f5cbf431a92ce4c20648f6995abd4d25307cec2b8ca6735fbbc71f2825d0d5
// IndexVersion: 2
// --- END CODE INDEX META ---
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
