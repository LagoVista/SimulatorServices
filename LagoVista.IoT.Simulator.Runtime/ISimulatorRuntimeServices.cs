// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 47bc341d4177eee2af3e949fc0d2bf29ac8d8da88ae3f4b2c0493dee17b0a28b
// IndexVersion: 2
// --- END CODE INDEX META ---
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
