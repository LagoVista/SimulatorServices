using LagoVista.CloudStorage.DocumentDB;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Simulator.Admin.Models;
using LagoVista.IoT.Simulator.Admin.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.CloudRepos.Repos
{
    public class SimulatorNetworkRepo : DocumentDBRepoBase<Admin.Models.SimulatorNetwork>, ISimulatorNetworkRepo
    {

        private bool _shouldConsolidateCollections;
        public SimulatorNetworkRepo(ISimulatorConnectionSettings repoSettings, IAdminLogger logger) 
            : base(repoSettings.SimulatorDocDbStorage.Uri, repoSettings.SimulatorDocDbStorage.AccessKey, repoSettings.SimulatorDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = repoSettings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections => _shouldConsolidateCollections;

        public Task AddSimulatorNetworkAsync(SimulatorNetwork network)
        {
            return CreateDocumentAsync(network);
        }

        public Task DeleteSimulatorNetworkAsync(string id)
        {
            return DeleteDocumentAsync(id);
        }

        public Task<SimulatorNetwork> GetSimulatorNetworkAsync(string id)
        {
            return GetDocumentAsync(id);
        }

        public async Task<IEnumerable<SimulatorNetworkSummary>> GetSimulatorNetworksForOrgAsync(string orgId)
        {
            var items = await base.QueryAsync(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId);

            return from item in items.OrderBy(sim=>sim.Name)
                   select item.CreateSummary();
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateSimulatorNetworkAsync(SimulatorNetwork network)
        {
            return base.UpsertDocumentAsync(network);
        }
    }
}
