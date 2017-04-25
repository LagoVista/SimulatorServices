using LagoVista.IoT.Simulator.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Admin.Repos
{
    public interface ISimulatorRepo
    {
        Task AddSimulatorAsync(Models.Simulator simulator);
        Task<Models.Simulator> GetSimulatorAsync(string id);
        Task<IEnumerable<SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(string orgId);
        Task<IEnumerable<SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(string orgId);
        Task<IEnumerable<SimulatorSummary>> GetSimulatorsForPipelineModuleConfigAsync(string orgId);
        Task<IEnumerable<SimulatorSummary>> GetSimulatorsForOrgAsync(string orgId);
        Task UpdateSimulatorAsync(Models.Simulator simulator);
        Task DeleteSimulatorAsync(string id);
        Task<bool> QueryKeyInUseAsync(string key, string org);

    }
}
