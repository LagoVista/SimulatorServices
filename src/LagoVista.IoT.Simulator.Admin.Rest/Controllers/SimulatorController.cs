using LagoVista.Core.Interfaces;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Simulator.Admin.Managers;
using LagoVista.IoT.Web.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core;
using LagoVista.Core.Validation;
using System.Threading.Tasks;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.UserAdmin.Models.Account;
using LagoVista.Core.Models;

namespace LagoVista.IoT.Simulator.Admin.Rest.Controllers
{
    [Authorize]
    [Route("api/simulator/admin")]
    public class SimulatorController : LagoVistaBaseController
    {
        ISimulatorManager simulatorManager;
        public SimulatorController(ISimulatorManager simulatorManager, UserManager<AppUser> userManager, ILogger logger) : base(userManager, logger)
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
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet("/api/org/{orgid}/simulators")]
        public async Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForOrgAsync(String orgid)
        {
            var simulators = await simulatorManager.GetSimulatorsForOrgsAsync(orgid, UserEntityHeader);
            return ListResponse<Models.SimulatorSummary>.Create(simulators);
        }

        /// <summary>
        /// Simulators - Get For Deployment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/deployment/instance/{id}/simulators")]
        public async Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForDeploymentConfigAsync(String id)
        {
            var simulators = await simulatorManager.GetSimulatorsForOrgsAsync(id, UserEntityHeader);
            return ListResponse<Models.SimulatorSummary>.Create(simulators);
        }

        /// <summary>
        /// Simulators - Get For Device Config
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/solution/{id}/simulators")]
        public async Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForDeviceConfigAsync(String id)
        {
            var simulators = await simulatorManager.GetSimulatorsForDeviceConfigAsync(id, OrgEntityHeader, UserEntityHeader);
            return ListResponse<Models.SimulatorSummary>.Create(simulators);
        }

        /// <summary>
        /// Simulators - Get For Pipeline Module
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/pipeline/{id}/simulators")]
        public async Task<ListResponse<Models.SimulatorSummary>> GetSimulatorsForPipelineModuleConfigAsync(String id)
        {
            var simulators = await simulatorManager.GetSimulatorsForPipelineModuleAsync(id, OrgEntityHeader, UserEntityHeader);
            return ListResponse<Models.SimulatorSummary>.Create(simulators);
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
            SetOwnedProperties(simulator.Model);
            SetAuditProperties(simulator.Model);

            return simulator;
        }

        /// <summary>
        /// Simulators Message Template - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/simulator/messagetemplate/factory")]
        public DetailResponse<Admin.Models.MessageTemplate> CreateMessageTempate()
        {
            return DetailResponse<Admin.Models.MessageTemplate>.Create();
        }

        /// <summary>
        /// Simulators Message Header - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/simulator/messageheader/factory")]
        public DetailResponse<Admin.Models.MessageHeader> CreateListener()
        {
            return DetailResponse<Admin.Models.MessageHeader>.Create();
        }
    }
}
