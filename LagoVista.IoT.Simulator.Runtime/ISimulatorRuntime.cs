// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: d3808217c7810cc13e263d7773a60b12137a2b5d7f6e55e40879350aa228a3ba
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Admin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public interface ISimulatorRuntime
    {
        Task<InvokeResult> ConnectAsync();

        Task<InvokeResult<String>> SendAsync(MessageTransmissionPlan messageTemplate);

        Task<InvokeResult> DisconnectAsync();

        Task StartAsync();
        Task StopAsync();

        String SimulatorName { get; }

        String InstanceName { get; }
        String InstanceId { get; }

        SimulatorState CurrentState { get; }

        void SetState(string stateKey);

        List<SimulatorState> States { get; }

        SimulatorStatus Status { get; }

        List<MessageTemplate> Messages { get; }
    }
}
