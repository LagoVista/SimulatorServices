﻿using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Simulator.Admin.Managers;
using LagoVista.IoT.Simulator.Admin.Models;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Admin.Rest.Controllers
{
    [Authorize]
    public class SimulatorNetworkController : LagoVistaBaseController
    {
        ISimulatorNetworkManager _simulatorNetworkManager;
        public SimulatorNetworkController(ISimulatorNetworkManager simulatorManager, UserManager<AppUser> userManager, IAdminLogger logger) : base(userManager, logger)
        {
            _simulatorNetworkManager = simulatorManager;
        }

        /// <summary>
        /// Simulator Network - Add
        /// </summary>
        /// <param name="simNetwork"></param>
        /// <returns></returns>
        [HttpPost("/api/simulator/network")]
        public Task<InvokeResult> AddSimulatorNetworkAsync([FromBody] SimulatorNetwork simNetwork)
        {
            return _simulatorNetworkManager.AddSimulatorNetworkAsync(simNetwork, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulator Network - Update
        /// </summary>
        /// <param name="simNetwork"></param>
        /// <returns></returns>
        [HttpPut("/api/simulator/network")]
        public Task<InvokeResult> UpdateSimulatorNetworkAsync([FromBody] SimulatorNetwork simNetwork)
        {
            return _simulatorNetworkManager.UpdateSimulatorNetworkAsync(simNetwork, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulator Network - Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/simulator/network/{id}")]
        public async Task<DetailResponse<SimulatorNetwork>> GetSimulatorNetworkAsync(string id)
        {
            var simNetwork = await _simulatorNetworkManager.GetSimulatorNetworkAsync(id, OrgEntityHeader, UserEntityHeader);
            return DetailResponse<Models.SimulatorNetwork>.Create(simNetwork);
        }

        /// <summary>
        /// Simualtor Network - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/simulator/network/{id}")]
        public Task<InvokeResult> DeleteSimulatorNetworkAsync(string id)
        {
            return _simulatorNetworkManager.DeleteSimulatorNetworkAsync(id, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Simulator Network - Get for Org
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/simulator/networks")]
        public async Task<ListResponse<SimulatorNetworkSummary>> GetSimulatorNetworksAsync()
        {
            var simNetwork = await _simulatorNetworkManager.GetSimulatorNetworksForOrgsAsync(OrgEntityHeader.Id, UserEntityHeader);
            return ListResponse<Models.SimulatorNetworkSummary>.Create(simNetwork);
        }

        /// <summary>
        /// Simulator Network - Key in use 
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/simulator/network/{key}/keyinuse")]
        public Task<bool> QueryKeyInUseAsync(String key)
        {
            return _simulatorNetworkManager.QueryKeyInUse(key, OrgEntityHeader);
        }

        /// <summary>
        /// Simulators Network - Check In Use
        /// </summary>
        /// <returns>.</returns>
        [HttpGet("/api/simulator/network/{id}/inuse")]
        public Task<DependentObjectCheckResult> CheckInUseAsync(String id)
        {
            return _simulatorNetworkManager.CheckInUse(id, OrgEntityHeader, UserEntityHeader);
        }

    }
}
