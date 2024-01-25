using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Simulator.Admin.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Admin.Repos
{
    public interface ISimulatorRepo
    {
        Task AddSimulatorAsync(Models.Simulator simulator);
        Task<Models.Simulator> GetSimulatorAsync(string id);
        Task<ListResponse<SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(string orgId, string deploymentConfigId, ListRequest listRequest);
        Task<ListResponse<SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(string orgId, string deviceConfigId, ListRequest listRequest);
        Task<ListResponse<SimulatorSummary>> GetSimulatorsForSolutionAsync(string orgId, string solutionId, ListRequest listRequest);
        Task<ListResponse<SimulatorSummary>> GetSimulatorsForPipelineModuleConfigAsync(string orgId, string pipelienModuleId, ListRequest listRequest);
        Task<ListResponse<SimulatorSummary>> GetSimulatorsForDeviceTypesAsync(string orgId, string deviceTypeid, ListRequest listRequest);
        Task<ListResponse<SimulatorSummary>> GetSimulatorsForOrgAsync(string orgId, ListRequest listRequest);
        Task UpdateSimulatorAsync(Models.Simulator simulator);
        Task DeleteSimulatorAsync(string id);
        Task<bool> QueryKeyInUseAsync(string key, string org);

    }
}
