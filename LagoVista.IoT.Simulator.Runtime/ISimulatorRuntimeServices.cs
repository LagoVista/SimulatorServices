using LagoVista.Client.Core;
using LagoVista.Core.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public interface ISimulatorRuntimeServices
    {
        ITCPClient GetTCPClient();
        IUDPClient GetUDPCLient();
        IMQTTDeviceClient GetMQTTClient();

        void Connected();
        void Disconnected();
        bool IsBusy { get; set; }
        String ReceivedContent { get; set; }
        Task AddReceviedMessage(ReceivedMessage msg);
    }
}
