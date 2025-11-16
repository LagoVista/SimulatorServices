// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 4b0edaf9dbfe13c54ff061f9013c6f2bf81c4a7744edc32821d1f6b34f794ad1
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Simulator.Admin.Models;
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

        public async void UpdateSensorValue(string instanceId, int sensorNumber, double sensorValue)
        {
            var payload = String.Empty;

            for (var idx = 0; idx < 16; ++idx)
            {
                if (sensorNumber == idx)
                {
                    payload += $"{sensorValue}";
                }
                
                if(idx < 15)
                    payload += ",";
            }

            var msg = new MessageTemplate()
            {
                QualityOfServiceLevel = Core.Models.EntityHeader<QualityOfServiceLevels>.Create(QualityOfServiceLevels.QOS0),
                Transport = Core.Models.EntityHeader<TransportTypes>.Create(TransportTypes.MQTT),
                Topic = "nuviot/srvr/dvcsrvc/~deviceid~/iovalues",
                TextPayload = payload,
                PayloadType = Core.Models.EntityHeader<PaylodTypes>.Create(PaylodTypes.String),
            };

            var instance = _mgr.Runtimes.Where(sim => sim.InstanceId == instanceId).FirstOrDefault();
            await instance.SendMessageAsync(msg);

        }

        public async void Reload()
        {
            try
            {
                await Log("Reloading simulator network.");
                await _mgr.ReloadAsync();
                await Log("Finished starting simulator network.");
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
