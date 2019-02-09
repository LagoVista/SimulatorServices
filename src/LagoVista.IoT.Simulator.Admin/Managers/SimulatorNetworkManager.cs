using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Simulator.Admin.Models;
using LagoVista.IoT.Simulator.Admin.Repos;
using static LagoVista.Core.Models.AuthorizeResult;

namespace LagoVista.IoT.Simulator.Admin.Managers
{
    public class SimulatorNetworkManager : ManagerBase, ISimulatorNetworkManager
    {
        ISimulatorNetworkRepo _repo;

        public SimulatorNetworkManager(ISimulatorNetworkRepo simulatorRepo, IAdminLogger logger, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) :
            base(logger, appConfig, depmanager, security)
        {
            _repo = simulatorRepo;
        }
    
        public async Task<InvokeResult> AddSimulatorNetworkAsync(SimulatorNetwork simulatorNetwork, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(simulatorNetwork, AuthorizeActions.Create, user, org);
            ValidationCheck(simulatorNetwork, Actions.Create);
            await _repo.AddSimulatorNetworkAsync(simulatorNetwork);
            return InvokeResult.Success;
        }

        public async Task<DependentObjectCheckResult> CheckInUse(string id, EntityHeader org, EntityHeader user)
        {
            var simulator = await _repo.GetSimulatorNetworkAsync(id);
            await AuthorizeAsync(simulator, AuthorizeActions.Read, user, org);
            return await CheckForDepenenciesAsync(simulator);
        }

        public async Task<InvokeResult> DeleteSimulatorNetworkAsync(string id, EntityHeader org, EntityHeader user)
        {
            var simulator = await _repo.GetSimulatorNetworkAsync(id);
            await AuthorizeAsync(simulator, AuthorizeActions.Delete, user, org);
            await ConfirmNoDepenenciesAsync(simulator);
            await _repo.GetSimulatorNetworkAsync(id);
            return InvokeResult.Success;
        }

        public async Task<SimulatorNetwork> GetSimulatorNetworkAsync(string id, EntityHeader org, EntityHeader user)
        {
            var simulator = await _repo.GetSimulatorNetworkAsync(id);
            await AuthorizeAsync(simulator, AuthorizeActions.Read, user, org);
            return simulator;
        }

        public async Task<IEnumerable<SimulatorNetworkSummary>> GetSimulatorNetworksForOrgsAsync(string orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(Models.SimulatorNetwork));
            return await _repo.GetSimulatorNetworksForOrgAsync(orgId);
        }

        public Task<bool> QueryKeyInUse(string key, EntityHeader org)
        {
            return _repo.QueryKeyInUseAsync(key, org.Id);
        }

        public async Task<InvokeResult> UpdateSimulatorNetworkAsync(SimulatorNetwork simulatorNetwork, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(simulatorNetwork, AuthorizeActions.Update, user, org);
            ValidationCheck(simulatorNetwork, Actions.Update);
            await _repo.UpdateSimulatorNetworkAsync(simulatorNetwork);
            return InvokeResult.Success;
        }
    }
}
