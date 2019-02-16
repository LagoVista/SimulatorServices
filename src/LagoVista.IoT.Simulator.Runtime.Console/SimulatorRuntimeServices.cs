using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Client.Core;
using LagoVista.Core.Networking.Interfaces;

namespace LagoVista.IoT.Simulator.Runtime
{
    public class SimulatorRuntimeServices : ISimulatorRuntimeServices
    {
        public bool IsBusy { get; set; }
        public string ReceivedContent { get; set; }

        public Task AddReceviedMessage(ReceivedMessage msg)
        {
            return Task.FromResult(default(object));
        }

        public void Connected()
        {
            
        }

        public void Disconnected()
        {

        }

        public IMQTTDeviceClient GetMQTTClient()
        {
            return new LagoVista.MQTT.Core.Clients.MQTTDeviceClient(new MQTT.DotNetCore.MqttNetworkChannel());
        }

        public ITCPClient GetTCPClient()
        {
            throw new NotImplementedException();
        }

        public IUDPClient GetUDPCLient()
        {
            throw new NotImplementedException();
        }
    }
}
