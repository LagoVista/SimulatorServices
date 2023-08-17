using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Admin.Managers
{
    public interface ISimulatorNetworkManager
    {
        Task<InvokeResult> AddSimulatorNetworkAsync(Models.SimulatorNetwork simulator, EntityHeader org, EntityHeader user);
        Task<Models.SimulatorNetwork> GetSimulatorNetworkAsync(string id, EntityHeader org, EntityHeader user, bool loadSecrets, bool alreadyAuthorized);
        Task<InvokeResult> UpdateSimulatorNetworkAsync(Models.SimulatorNetwork simulator, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteSimulatorNetworkAsync(string id, EntityHeader org, EntityHeader user);

        Task<IEnumerable<Models.SimulatorNetworkSummary>> GetSimulatorNetworksForOrgsAsync(string id, EntityHeader user);


        Task<bool> QueryKeyInUse(string key, EntityHeader org);
        Task<DependentObjectCheckResult> CheckInUse(string id, EntityHeader org, EntityHeader user);
    }
}
