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
        Task<Models.Simulator> GetSimulatorAsync(string id, EntityHeader org, EntityHeader user, bool loadSecrets = false, bool loadCSV = false);
        Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForOrgsAsync(string id, ListRequest listRequest, EntityHeader user);
        Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(string deploymentConfigId, ListRequest listRequest, EntityHeader org, EntityHeader user);
        Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(string deviceConfigId, ListRequest listRequest, EntityHeader org, EntityHeader user);
        Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForSolutionAsync(string solutionId, ListRequest listRequest, EntityHeader org, EntityHeader user);
        Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForPipelineModuleAsync(string pipelineConfigId, ListRequest listRequest, EntityHeader org, EntityHeader user);
        Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForDeviceTypesAsync(string pipelineConfigId, ListRequest listRequest, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateSimulatorAsync(Models.Simulator simulator, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteSimulatorAsync(string id, EntityHeader org, EntityHeader user);

        Task<bool> QueryKeyInUse(string key, EntityHeader org);
        Task<DependentObjectCheckResult> CheckInUse(string id, EntityHeader org, EntityHeader user);
    }
}
