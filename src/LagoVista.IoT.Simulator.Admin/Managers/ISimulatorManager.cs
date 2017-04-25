using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Admin.Managers
{
    public interface ISimulatorManager
    {
        Task<InvokeResult> AddSimulatorAsync(Models.Simulator simulator, EntityHeader org, EntityHeader user);
        Task<Models.Simulator> GetSimulatorAsync(string id, EntityHeader org, EntityHeader user);
        Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForOrgsAsync(string id, EntityHeader user);
        Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(string deploymentConfigId, EntityHeader org, EntityHeader user);
        Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(string deviceConfigId, EntityHeader org, EntityHeader user);
        Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForPipelineModuleAsync(string pipelineConfigId, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateSimulatorAsync(Models.Simulator simulator, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteSimulatorAsync(string id, EntityHeader org, EntityHeader user);

        Task<bool> QueryKeyInUse(string key, EntityHeader org);
        Task<DependentObjectCheckResult> CheckInUse(string id, EntityHeader org, EntityHeader user);
    }
}
