using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Admin.Models;
using System.Threading.Tasks;
using LagoVista.IoT.Simulator.Admin.Repos;
using LagoVista.Core.Managers;
using static LagoVista.Core.Models.AuthorizeResult;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Simulator.Admin.Managers
{
    public class SimulatorManager : ManagerBase, ISimulatorManager
    {
        ISimulatorRepo _simulatorRepo;

        public SimulatorManager(ISimulatorRepo simulatorRepo, ILogger logger, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) :
            base(logger, appConfig, depmanager, security)
        {
            _simulatorRepo = simulatorRepo;
        }

        public async Task<InvokeResult> AddSimulatorAsync(Models.Simulator simulator, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(simulator, AuthorizeActions.Create, user, org);
            ValidationCheck(simulator, Actions.Create);
            await _simulatorRepo.AddSimulatorAsync(simulator);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeleteSimulatorAsync(string id, EntityHeader org, EntityHeader user)
        {
            var simulator = await _simulatorRepo.GetSimulatorAsync(id);
            await AuthorizeAsync(simulator, AuthorizeActions.Create, user, org);
            await ConfirmNoDepenenciesAsync(simulator);
            await _simulatorRepo.DeleteSimulatorAsync(id);
            return InvokeResult.Success;
        }

        public async Task<Models.Simulator> GetSimulatorAsync(string id, EntityHeader org, EntityHeader user)
        {
            var simulator = await _simulatorRepo.GetSimulatorAsync(id);
            await AuthorizeAsync(simulator, AuthorizeActions.Read, user, org);
            return simulator;
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForOrgsAsync(string orgId, EntityHeader user)
        {
            await AuthorizeOrgAccess(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForOrgAsync(orgId);
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(string orgId, EntityHeader org, EntityHeader user)
        {
            await AuthorizeOrgAccess(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForDeploymentConfigAsync(orgId);
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(string orgId, EntityHeader org, EntityHeader user)
        {
            await AuthorizeOrgAccess(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForDeviceConfigAsync(orgId);
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForDeviceTypesAsync(string orgId, EntityHeader org, EntityHeader user)
        {
            await AuthorizeOrgAccess(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForDeviceTypesAsync(orgId);
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForPipelineModuleAsync(string orgId, EntityHeader org, EntityHeader user)
        {
            await AuthorizeOrgAccess(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForPipelineModuleConfigAsync(orgId);
        }


        public Task<bool> QueryKeyInUse(string key, EntityHeader org)
        {
            return _simulatorRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public async Task<InvokeResult> UpdateSimulatorAsync(Models.Simulator simulator, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(simulator, AuthorizeActions.Create, user, org);
            await _simulatorRepo.UpdateSimulatorAsync(simulator);
            return InvokeResult.Success;
        }

        public async Task<DependentObjectCheckResult> CheckInUse(string id, EntityHeader org, EntityHeader user)
        {
            var simulator = await _simulatorRepo.GetSimulatorAsync(id);
            await AuthorizeAsync(simulator, AuthorizeActions.Read, user, org);
            return await CheckForDepenenciesAsync(simulator);
        }
    }
}