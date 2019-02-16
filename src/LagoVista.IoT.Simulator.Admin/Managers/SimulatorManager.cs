using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Simulator.Admin.Repos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LagoVista.Core.Models.AuthorizeResult;

namespace LagoVista.IoT.Simulator.Admin.Managers
{
    public class SimulatorManager : ManagerBase, ISimulatorManager
    {
        ISimulatorRepo _simulatorRepo;
        ISecureStorage _secureStorage;

        public SimulatorManager(ISimulatorRepo simulatorRepo, ISecureStorage secureStorage, IAdminLogger logger, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) :
            base(logger, appConfig, depmanager, security)
        {
            _simulatorRepo = simulatorRepo;
            _secureStorage = secureStorage;
        }

        public async Task<InvokeResult> AddSimulatorAsync(Models.Simulator simulator, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(simulator, AuthorizeActions.Create, user, org);
            ValidationCheck(simulator, Actions.Create);
            if (!String.IsNullOrEmpty(simulator.Password))
            {
                var result = await _secureStorage.AddSecretAsync(org, simulator.Password);
                if (!result.Successful)
                {
                    return result.ToInvokeResult();
                }

                simulator.PasswordSecureId = result.Result;
                simulator.Password = null;
            }

            if (!String.IsNullOrEmpty(simulator.AccessKey))
            {
                var result = await _secureStorage.AddSecretAsync(org, simulator.AccessKey);
                if (!result.Successful)
                {
                    return result.ToInvokeResult();
                }

                simulator.AccessKeySecureId = result.Result;
                simulator.AccessKey = null;
            }

            if (!String.IsNullOrEmpty(simulator.AuthHeader))
            {
                var result = await _secureStorage.AddSecretAsync(org, simulator.AuthHeader);
                if (!result.Successful)
                {
                    return result.ToInvokeResult();
                }

                simulator.AuthHeaderSecureId = result.Result;
                simulator.AuthHeader = null;
            }

            await _simulatorRepo.AddSimulatorAsync(simulator);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeleteSimulatorAsync(string id, EntityHeader org, EntityHeader user)
        {
            var simulator = await _simulatorRepo.GetSimulatorAsync(id);
            await AuthorizeAsync(simulator, AuthorizeActions.Delete, user, org);
            await ConfirmNoDepenenciesAsync(simulator);

            await _simulatorRepo.DeleteSimulatorAsync(id);

            if (!String.IsNullOrEmpty(simulator.PasswordSecureId))
            {
                await _secureStorage.RemoveSecretAsync(org, simulator.PasswordSecureId);
            }

            if (!String.IsNullOrEmpty(simulator.AccessKeySecureId))
            {
                await _secureStorage.RemoveSecretAsync(org, simulator.AccessKeySecureId);
            }

            if (!String.IsNullOrEmpty(simulator.AuthHeaderSecureId))
            {
                await _secureStorage.RemoveSecretAsync(org, simulator.AuthHeaderSecureId);
            }

            return InvokeResult.Success;
        }

        public async Task<Models.Simulator> GetSimulatorAsync(string id, EntityHeader org, EntityHeader user, bool loadSecrets = false)
        {
            var simulator = await _simulatorRepo.GetSimulatorAsync(id);

            if (!String.IsNullOrEmpty(simulator.AccessKeySecureId))
            {
                await _secureStorage.GetSecretAsync(org, simulator.AccessKeySecureId, user);
            }

            if (!String.IsNullOrEmpty(simulator.AccessKeySecureId))
            {
                await _secureStorage.GetSecretAsync(org, simulator.AccessKeySecureId, user);
            }

            if (!String.IsNullOrEmpty(simulator.AuthHeaderSecureId))
            {
                await _secureStorage.GetSecretAsync(org, simulator.AuthHeaderSecureId, user);
            }

            await AuthorizeAsync(simulator, AuthorizeActions.Read, user, org);
            return simulator;
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForOrgsAsync(string orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForOrgAsync(orgId);
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(string orgId, EntityHeader org, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForDeploymentConfigAsync(orgId);
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(string orgId, EntityHeader org, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForDeviceConfigAsync(orgId);
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForDeviceTypesAsync(string orgId, EntityHeader org, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForDeviceTypesAsync(orgId);
        }

        public async Task<IEnumerable<Models.SimulatorSummary>> GetSimulatorsForPipelineModuleAsync(string orgId, EntityHeader org, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(Models.Simulator));
            return await _simulatorRepo.GetSimulatorsForPipelineModuleConfigAsync(orgId);
        }


        public Task<bool> QueryKeyInUse(string key, EntityHeader org)
        {
            return _simulatorRepo.QueryKeyInUseAsync(key, org.Id);
        }

        public async Task<InvokeResult> UpdateSimulatorAsync(Models.Simulator simulator, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(simulator, AuthorizeActions.Create, user, org);

            if (!String.IsNullOrEmpty(simulator.Password))
            {
                if (!String.IsNullOrEmpty(simulator.PasswordSecureId))
                {
                    await _secureStorage.RemoveSecretAsync(org, simulator.PasswordSecureId);
                }

                var result = await _secureStorage.AddSecretAsync(org, simulator.Password);
                if (!result.Successful)
                {
                    return result.ToInvokeResult();
                }

                simulator.PasswordSecureId = result.Result;
                simulator.Password = null;
            }

            if (!String.IsNullOrEmpty(simulator.AccessKey))
            {
                if (!String.IsNullOrEmpty(simulator.AccessKeySecureId))
                {
                    await _secureStorage.RemoveSecretAsync(org, simulator.AccessKeySecureId);
                }

                var result = await _secureStorage.AddSecretAsync(org, simulator.AccessKey);
                if (!result.Successful)
                {
                    return result.ToInvokeResult();
                }

                simulator.AccessKeySecureId = result.Result;
                simulator.AccessKey = null;
            }

            if (!String.IsNullOrEmpty(simulator.AuthHeader))
            {
                if (!String.IsNullOrEmpty(simulator.AuthHeaderSecureId))
                {
                    await _secureStorage.RemoveSecretAsync(org, simulator.AuthHeaderSecureId);
                }

                var result = await _secureStorage.AddSecretAsync(org, simulator.AuthHeader);
                if (!result.Successful)
                {
                    return result.ToInvokeResult();
                }

                simulator.AuthHeaderSecureId = result.Result;
                simulator.AuthHeader = null;
            }

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