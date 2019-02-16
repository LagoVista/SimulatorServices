using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public interface ISimulatorRuntime
    {
        Task<InvokeResult> ConnectAsync();

        Task<InvokeResult<String>> SendAsync(MessageTemplate messageTemplate);

        Task<InvokeResult> DisconnectAsync();
    }
}
