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

        public async void SetState(string instanceId, string newState)
        {
            try
            {
                var instance = _mgr.Runtimes.Where(sim => sim.InstanceId == instanceId).FirstOrDefault();
                if (instance == null)
                {
                    await Error($"Could not find instance with instance id {instanceId}");
                    return;
                }

                await Log($"Setting new state on simulator {instance.InstanceName}.");
                instance?.SetState(newState);
                await Log($"Set new state on simulator {instance.InstanceName}.");
                instance?.RestartAsync();
                await Log($"Restarted simulator {instance.InstanceName}.");
            }
            catch (Exception ex)
            {
                await Error($"Error setting state: " + ex.Message);
            }
        }

        public async void Reload()
        {
            try
            {
                await Log("Reloading simulator network.");
                await _mgr.ReloadAsync();
                await Log("Finsihed starting simulator network.");
            }
            catch (Exception ex)
            {
                await Error($"Error reloading simulator network: " + ex.Message);
            }
        }

        public async void SendMessage(string instanceId, string msgId)
        {
            try
            {
                var instance = _mgr.Runtimes.Where(sim => sim.InstanceId == instanceId).FirstOrDefault();
                if (instance == null)
                {
                    await Error($"Could not find instance with instance id {instanceId}");
                    return;
                }

                await instance?.SendMessageAsync(msgId);
                await Log($"Sent message to simulator: " + instance.InstanceName);
            }
            catch (Exception ex)
            {
                await Error($"Error sending message system: " + ex.Message);
            }
        }

        public async void Start()
        {
            try
            {
                await _mgr.StartAsync();
            }
            catch (Exception ex)
            {
                await Error($"Error starting system: " + ex.Message);
            }
        }

        public async void Stop()
        {
            try
            {
                await _mgr.StopAsync();
            }
            catch (Exception ex)
            {
                await Error($"Error stopping system: " + ex.Message);
            }
        }

        public async void StartSimulator(string instanceId)
        {
            try
            {
                var instance = _mgr.Runtimes.Where(sim => sim.InstanceId == instanceId).FirstOrDefault();
                if (instance == null)
                {
                    await Error($"Could not find instance with instance id {instanceId}");
                    return;
                }

                await instance?.StartAsync();
                await Log($"Simulator Started: {instance.InstanceName}");
            }
            catch (Exception ex)
            {
                await Error($"Error starting simulator: " + ex.Message);
            }
        }

        public async void StopSimulator(string instanceId)
        {
            try
            {
                var instance = _mgr.Runtimes.Where(sim => sim.InstanceId == instanceId).FirstOrDefault();
                if (instance == null)
                {
                    await Error($"Could not find instance with instance id {instanceId}");
                    return;
                }

                await instance?.StopAsync();
                await Log($"Simulator Stopped: {instance.InstanceName}");
            }
            catch (Exception ex)
            {
                await Error($"Error stopping simulator: " + ex.Message);
            }
        }

        public async Task Send(string message)
        {
            try
            {
                await Clients.All.SendAsync("Send", message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
        }

        public async Task Log(string log)
        {
            try
            {
                await Clients.All.SendAsync("Log", log);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
        }

        public async Task Error(string message)
        {
            try
            {
                await Clients.All.SendAsync("Error", message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
        }

        public Task NotifyAsync(String msg)
        {
            try
            {
                return Clients.All.SendAsync("update", new string[] { msg });
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
                return Task.CompletedTask;
            }            
        }

        public override Task OnConnectedAsync()
        {
            var cid = Context.ConnectionId;

            return base.OnConnectedAsync();
        }
    }
}
