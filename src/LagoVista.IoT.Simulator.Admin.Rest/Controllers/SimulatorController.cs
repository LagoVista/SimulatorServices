﻿using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Simulator.Admin.Managers;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Admin.Rest.Controllers
{
    [Authorize]
    public class SimulatorController : LagoVistaBaseController
    {
        ISimulatorManager simulatorManager;
        public SimulatorController(ISimulatorManager simulatorManager, UserManager<AppUser> userManager, IAdminLogger logger) : base(userManager, logger)
        {
            this.simulatorManager = simulatorManager;
        }

        /// <summary>
        /// Simulators - Add
        /// </summary>
        /// <param name="simulator"></param>
        /// <returns></returns>
        [HttpPost("/api/simulator")]
        public Task<InvokeResult> AddSimulatorAsync([FromBody] Admin.Models.Simulator simulator)
        {
            return simulatorManager.AddSimulatorAsync(simulator, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulators - Update
        /// </summary>
        /// <param name="simulator"></param>
        /// <returns></returns>
        [HttpPut("/api/simulator")]
        public Task<InvokeResult> UpdateSimulatorAsync([FromBody] Admin.Models.Simulator simulator)
        {
            return simulatorManager.UpdateSimulatorAsync(simulator, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulators - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/simulator/{id}")]
        public async Task<DetailResponse<Admin.Models.Simulator>> GetSimulatorAsync(String id)
        {
            var simulator = await simulatorManager.GetSimulatorAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<Models.Simulator>.Create(simulator);
        }

        /// <summary>
        /// Simulators - Get For Org
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/simulators")]
        public async Task<ListResponse<Models.SimulatorSummary>> GetSimulators()
        {
            return await simulatorManager.GetSimulatorsForOrgsAsync(OrgEntityHeader.Id, GetListRequestFromHeader(), UserEntityHeader);
        }

        /// <summary>
        /// Simulators - Get For Org
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/org/simulators")]
        public async Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForOrgAsync()
        {
            return await simulatorManager.GetSimulatorsForOrgsAsync(OrgEntityHeader.Id, GetListRequestFromHeader(), UserEntityHeader);
        }

        /// <summary>
        /// Simulators - Get For Deployment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/simulators")]
        public async Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(String id)
        {
            return await simulatorManager.GetSimulatorsForDeploymentConfigAsync(id, GetListRequestFromHeader(), OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulators - Get For Device Config
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deviceconfig/{id}/simulators")]
        public Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(String id)
        {
            return simulatorManager.GetSimulatorsForDeviceConfigAsync(id, GetListRequestFromHeader(), OrgEntityHeader, UserEntityHeader);
        }


        /// <summary>
        /// Simulators - Get For Device Config
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/solution/{id}/simulators")]
        public Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForSolutionAsync(String id)
        {
            return simulatorManager.GetSimulatorsForSolutionAsync(id, GetListRequestFromHeader(), OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulators - Get For Pipeline Module
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/{id}/simulators")]
        public  Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForPipelineModuleConfigAsync(String id)
        {
            return simulatorManager.GetSimulatorsForPipelineModuleAsync(id, GetListRequestFromHeader(), OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulators - Get For Device Types
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/devicetype/{id}/simulators")]
        public Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForDeviceTypeAsync(String id)
        {
            return simulatorManager.GetSimulatorsForDeviceTypesAsync(id, GetListRequestFromHeader(), OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulators - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/simulator/{id}")]
        public Task<InvokeResult> DeleteListenerConfigurationAsync(String id)
        {
            return simulatorManager.DeleteSimulatorAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulators - Key in use 
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/simulator/{key}/keyinuse")]
        public Task<bool> QueryKeyInUseListenerConfigurationAsync(String key)
        {
            return simulatorManager.QueryKeyInUse(key, OrgEntityHeader);
        }

        /// <summary>
        /// Simulators - Check In Use
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/simulator/{id}/inuse")]
        public Task<DependentObjectCheckResult> CheckInUse(String id)
        {
            return simulatorManager.CheckInUse(id, OrgEntityHeader, UserEntityHeader);
        }


        /// <summary>
        /// Simulator  - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/simulator/factory")]
        public DetailResponse<Models.Simulator> CreateSimulator()
        {
            var simulator = DetailResponse<Models.Simulator>.Create();
            simulator.Model.SimulatorStates.Add(new Models.SimulatorState()
            {
                Id = Guid.NewGuid().ToId(),
                Description = "Default State",
                Key = "default",
                Name = "Default State"
            });

            SetOwnedProperties(simulator.Model);
            SetAuditProperties(simulator.Model);

            return simulator;
        }

        /// <summary>
        /// Simulators Message Header - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/simulator/messageheader/factory")]
        public DetailResponse<Admin.Models.MessageHeader> CreatemessageHeader()
        {
            return DetailResponse<Admin.Models.MessageHeader>.Create();
        }

        /// <summary>
        /// Simulators Message Template - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/simulator/messagetemplate/factory")]
        public DetailResponse<Admin.Models.MessageTemplate> CreateMessageTemplate()
        {
            return DetailResponse<Admin.Models.MessageTemplate>.Create();
        }

        /// <summary>
        /// Simulators Message Header - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/simulator/dyanimaicAttribute/factory")]
        public DetailResponse<Admin.Models.MessageDynamicAttribute> CreateDynamicAttributes()
        {
            return DetailResponse<Admin.Models.MessageDynamicAttribute>.Create();
        }
    }
}
