// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 3d2f71cdd8fa47e3093b45c81bb9de4eac89eb40364e49b5ea53e7545de74505
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Simulator.Admin.Repos;
using System.Collections.Generic;
using System.Linq;
using LagoVista.IoT.Simulator.Admin.Models;
using System.Threading.Tasks;
using LagoVista.CloudStorage.DocumentDB;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Models.UIMetaData;

namespace LagoVista.IoT.Simulator.CloudRepos.Repos
{
    public class SimulatorRepo : DocumentDBRepoBase<Admin.Models.Simulator>, ISimulatorRepo
    {
        private bool _shouldConsolidateCollections;
        public SimulatorRepo(ISimulatorConnectionSettings repoSettings, IAdminLogger logger) : base(repoSettings.SimulatorDocDbStorage.Uri, repoSettings.SimulatorDocDbStorage.AccessKey, repoSettings.SimulatorDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = repoSettings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections => _shouldConsolidateCollections;

        public Task AddSimulatorAsync(Admin.Models.Simulator simulator)
        {
            return CreateDocumentAsync(simulator);
        }

        public Task DeleteSimulatorAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<Admin.Models.Simulator> GetSimulatorAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public  Task<ListResponse<SimulatorSummary>> GetSimulatorsForOrgAsync(string orgId, ListRequest listRequest)
        {
            return base.QuerySummaryAsync<SimulatorSummary, Admin.Models.Simulator>(qry => qry.OwnerOrganization.Id == orgId, sim => sim.Name, listRequest);
        }

        public  Task<ListResponse<SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(string orgId, string deploymentConfigId, ListRequest listRequest)
        {
            return base.QuerySummaryAsync<SimulatorSummary, Admin.Models.Simulator>(qry => qry.DeploymentConfiguration != null && qry.DeploymentConfiguration.Id == deploymentConfigId && qry.OwnerOrganization.Id == orgId, sim => sim.Name, listRequest);
        }

        public Task<ListResponse<SimulatorSummary>> GetSimulatorsForSolutionAsync(string orgId, string solutionId, ListRequest listRequest)
        {
            return base.QuerySummaryAsync<SimulatorSummary, Admin.Models.Simulator>(qry => qry.Solution != null && qry.Solution.Id == solutionId && qry.OwnerOrganization.Id == orgId, sim => sim.Name, listRequest);
        }


        public Task<ListResponse<SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(string orgId, string deviceConfigId, ListRequest listRequest)
        {
            return base.QuerySummaryAsync<SimulatorSummary, Admin.Models.Simulator>(qry => qry.DeviceConfiguration != null && qry.DeviceConfiguration.Id == deviceConfigId && 
                qry.OwnerOrganization.Id == orgId, sim => sim.Name, listRequest);
        }

        public Task<ListResponse<SimulatorSummary>> GetSimulatorsForPipelineModuleConfigAsync(string orgId, string pipelineModuleConfigId, ListRequest listRequest)
        {
            return base.QuerySummaryAsync<SimulatorSummary, Admin.Models.Simulator>(qry => qry.PipelineModuleConfiguration != null && 
                qry.PipelineModuleConfiguration.Id == pipelineModuleConfigId && qry.OwnerOrganization.Id == orgId, 
                sim => sim.Name, listRequest);
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateSimulatorAsync(Admin.Models.Simulator simulator)
        {
            return base.UpsertDocumentAsync(simulator);
        }

        public Task<ListResponse<SimulatorSummary>> GetSimulatorsForDeviceTypesAsync(string orgId, string deviceTypeId, ListRequest listRequest)
        {
            return base.QuerySummaryAsync<SimulatorSummary, Admin.Models.Simulator>(qry => qry.DeviceType != null && qry.DeviceType.Id == deviceTypeId && qry.OwnerOrganization.Id == orgId, sim=>sim.Name, listRequest);
        }
    }
}
