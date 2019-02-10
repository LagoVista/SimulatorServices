using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public interface ISimulatorRuntime
    {
        Task<InvokeResult> ConnectAsync();

        Task<String> Send(MessageTemplate messageTemplate);

        Task DisconnectAsync();
    }
}
