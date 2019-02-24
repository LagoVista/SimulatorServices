using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public interface ISimulatorRuntime
    {
        Task<InvokeResult> ConnectAsync();

        Task<InvokeResult<String>> SendAsync(MessageTransmissionPlan messageTemplate);

        Task<InvokeResult> DisconnectAsync();

        void Start();
        void Stop();

        String SimulatorName { get; }

        String InstanceName { get; }
        String InstanceId { get; }

        String CurrentState { get; }

        bool IsActive { get; }
    }
}
