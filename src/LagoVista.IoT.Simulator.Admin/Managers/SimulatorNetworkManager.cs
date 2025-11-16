// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 49748f6d55f6fed43939895a61b7d94a30e3de5815bdac3eee10636f7f722cfe
// IndexVersion: 2
// --- END CODE INDEX META ---
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
        ISecureStorage _secureStorage;

        public SimulatorNetworkManager(ISimulatorNetworkRepo simulatorRepo, ISecureStorage secureStorage, IAdminLogger logger, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) :
            base(logger, appConfig, depmanager, security)
        {
            _repo = simulatorRepo;
            _secureStorage = secureStorage;
        }
    
        public async Task<InvokeResult> AddSimulatorNetworkAsync(SimulatorNetwork simulatorNetwork, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(simulatorNetwork, AuthorizeActions.Create, user, org);

            var addResult = await _secureStorage.AddSecretAsync(org, simulatorNetwork.SharedAccessKey1);
            if (!addResult.Successful) return addResult.ToInvokeResult();
            simulatorNetwork.SharedAccessKey1SecretId = addResult.Result;
            simulatorNetwork.SharedAccessKey1 = null;

            addResult = await _secureStorage.AddSecretAsync(org, simulatorNetwork.SharedAccessKey2);
            if (!addResult.Successful) return addResult.ToInvokeResult();
            simulatorNetwork.SharedAccessKey2SecretId = addResult.Result;
            simulatorNetwork.SharedAccessKey2 = null;

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
            await _repo.DeleteSimulatorNetworkAsync(id);

            return InvokeResult.Success;
        }

        /// when we are called from client app rahter then web API, authorization is done via the headers
        public async Task<SimulatorNetwork> GetSimulatorNetworkAsync(string id, EntityHeader org, EntityHeader user, bool loadSecrets, bool alreadyAuthorized)
        {
            var simulator = await _repo.GetSimulatorNetworkAsync(id);
            if(!alreadyAuthorized)
             await AuthorizeAsync(simulator, AuthorizeActions.Read, user, org);

            if(loadSecrets)
            {
                var keyResult =  await _secureStorage.GetSecretAsync(org, simulator.SharedAccessKey1SecretId, user);
                if (!keyResult.Successful) throw new Exception("Could not find secure id for simulatorNetwork.SharedAccessKey1SecretId");
                simulator.SharedAccessKey1 = keyResult.Result;

                keyResult = await _secureStorage.GetSecretAsync(org, simulator.SharedAccessKey2SecretId, user);
                if (!keyResult.Successful) throw new Exception("Could not find secure id for simulatorNetwork.SharedAccessKey2SecretId");
                simulator.SharedAccessKey2 = keyResult.Result;
            }

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

            if (!String.IsNullOrEmpty(simulatorNetwork.SharedAccessKey1))
            {
                var addResult = await _secureStorage.AddSecretAsync(org, simulatorNetwork.SharedAccessKey1);
                await _secureStorage.RemoveSecretAsync(org, simulatorNetwork.SharedAccessKey1SecretId);
                if (!addResult.Successful) return addResult.ToInvokeResult();
                simulatorNetwork.SharedAccessKey1SecretId = addResult.Result;
                simulatorNetwork.SharedAccessKey1 = null;
            }

            if (!String.IsNullOrEmpty(simulatorNetwork.SharedAccessKey2))
            {
                await _secureStorage.RemoveSecretAsync(org, simulatorNetwork.SharedAccessKey2SecretId);

                var addResult = await _secureStorage.AddSecretAsync(org, simulatorNetwork.SharedAccessKey2);
                if (!addResult.Successful) return addResult.ToInvokeResult();
                simulatorNetwork.SharedAccessKey2SecretId = addResult.Result;
                simulatorNetwork.SharedAccessKey2 = null;
            }

            
            await _repo.UpdateSimulatorNetworkAsync(simulatorNetwork);
            return InvokeResult.Success;
        }
    }
}
