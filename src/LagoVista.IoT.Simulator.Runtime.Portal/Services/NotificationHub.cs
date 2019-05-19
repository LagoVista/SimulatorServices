using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime.Portal.Services
{
    public class NotificationHub : Hub
    {
        SimulatorRuntimeManager _mgr;

        public NotificationHub(SimulatorRuntimeManager mgr)
        {
            _mgr = mgr;
        }        

        public void SetState(string instanceId, string newState)
        {
            var instance = _mgr.Runtimes.Where(sim => sim.InstanceId == instanceId).FirstOrDefault();
            instance?.SetState(newState);
            instance?.RestartAsync();
        }

        public async void Reload()
        {
            await _mgr.ReloadAsync();
        }

        public async void SendMessage(string instanceId, string msgId)
        {
            var instance = _mgr.Runtimes.Where(sim => sim.InstanceId == instanceId).FirstOrDefault();
            await instance?.SendMessageAsync(msgId);
        }

        public async void Start()
        {
            await _mgr.StartAsync();
        }

        public async void Stop()
        {
            await _mgr.StopAsync();
        }

        public async void StartSimulator(string instanceId)
        {
            var instance = _mgr.Runtimes.Where(sim => sim.InstanceId == instanceId).FirstOrDefault();
            await instance?.StartAsync();
        }

        public async void StopSimulator(string instanceId)
        {
            var instance = _mgr.Runtimes.Where(sim => sim.InstanceId == instanceId).FirstOrDefault();
            await instance?.StopAsync();
        }

        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }

        public Task NotifyAsync(String msg)
        {
           return Clients.All.SendAsync("update", new string[] { msg });
        }

        public override Task OnConnectedAsync()
        {
            var cid = Context.ConnectionId;

            return base.OnConnectedAsync();
        }
    }
}
