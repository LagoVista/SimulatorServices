using LagoVista.IoT.Simulator.Admin.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LagoVista.IoT.Simulator.Admin.Models;
using System.Threading.Tasks;
using LagoVista.CloudStorage.DocumentDB;
using LagoVista.Core.PlatformSupport;

namespace LagoVista.IoT.Simulator.CloudRepos.Repos
{
    public class SimulatorRepo : DocumentDBRepoBase<Admin.Models.Simulator>, ISimulatorRepo
    {

        private bool _shouldConsolidateCollections;
        public SimulatorRepo(ISimulatorConnectionSettings repoSettings, ILogger logger) : base(repoSettings.SimulatorDocDbStorage.Uri, repoSettings.SimulatorDocDbStorage.AccessKey, repoSettings.SimulatorDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = repoSettings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections
        {
            get
            {
                return _shouldConsolidateCollections;
            }
        }


        public Task AddSimulatorAsync(Admin.Models.Simulator simulator)
        {
            return AddSimulatorAsync(simulator);
        }

        public Task DeleteSimulatorAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<Admin.Models.Simulator> GetSimulatorAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<IEnumerable<SimulatorSummary>> GetSimulatorsForOrgAsync(string orgId)
        {
            var items = await base.QueryAsync(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId);

            return from item in items
                   select item.CreateSummary();
        }

        public async Task<IEnumerable<SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(string deploymentConfigId)
        {
            var items = await base.QueryAsync(qry => qry.OwnerOrganization.Id == deploymentConfigId);

            return from item in items
                   select item.CreateSummary();
        }

        public async Task<IEnumerable<SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(string deviceConfigId)
        {
            var items = await base.QueryAsync(qry => qry.OwnerOrganization.Id == deviceConfigId);

            return from item in items
                   select item.CreateSummary();
        }


        public async Task<IEnumerable<SimulatorSummary>> GetSimulatorsForPipelineModuleConfigAsync(string pipelineModuleConfigId)
        {
            var items = await base.QueryAsync(qry => qry.PipelineModuleConfiguration.Id == pipelineModuleConfigId);

            return from item in items
                   select item.CreateSummary();
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateSimulatorAsync(Admin.Models.Simulator simulator)
        {
            return UpdateSimulatorAsync(simulator);
        }
    }
}
