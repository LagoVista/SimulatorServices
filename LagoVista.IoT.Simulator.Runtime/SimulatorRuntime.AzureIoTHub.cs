using LagoVista.Core.Validation;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Simulator.Admin.Models;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public partial class SimulatorRuntime
    {
        private async Task ReceiveDataFromAzure()
        {
            while (_azureIoTHubClient != null)
            {
                try
                {
                    var message = await _azureIoTHubClient.ReceiveAsync();
                    if (message != null)
                    {
                        try
                        {
                            var msg = new ReceivedMessage(message.GetBytes());
                            msg.MessageId = message.MessageId;
                            msg.Topic = message.To;
                            await AddReceviedMessage(msg);
                            await _azureIoTHubClient.CompleteAsync(message);
                        }
                        catch
                        {
                            await _azureIoTHubClient.RejectAsync(message);
                        }
                    }
                }
                catch (TaskCanceledException) { }
            }
        }

        private async Task ConnectAzureIoTHubAsync()
        {
            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connecting to {_simulator.DefaultTransport.Text} - {_simulator.DefaultEndPoint} with device id {_simulator.DeviceId}.");

            var connectionString = $"HostName={_simulator.DefaultEndPoint};DeviceId={_simulator.DeviceId};SharedAccessKey={_simulator.AccessKey}";
            _azureIoTHubClient = DeviceClient.CreateFromConnectionString(connectionString, Microsoft.Azure.Devices.Client.TransportType.Amqp_Tcp_Only);
            await _azureIoTHubClient.OpenAsync();

            _receiveTaskCancelTokenSource = new CancellationTokenSource();
            _receivingTask = Task.Run(ReceiveDataFromAzure, _receiveTaskCancelTokenSource.Token);
            SetConnectedState();

            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connected to {_simulator.DefaultTransport.Text} - {_simulator.DefaultEndPoint} on {_simulator.DefaultPort}.");
        }

        private async Task DisconnectAzureIoTHubAsync()
        {
            await _azureIoTHubClient.CloseAsync();
            _azureIoTHubClient.Dispose();

            _receiveTaskCancelTokenSource?.Cancel();
        }

        private async Task<InvokeResult> SendIoTHubMessage(MessageTransmissionPlan plan)
        {
            var messageTemplate = plan.Message.Value;

            if (_azureIoTHubClient == null)
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Azure IoT Hub is null, could not send message");
                return InvokeResult.FromError("Azure IoT Hub is null, could not send message");
            }

            if (!_isConnected)
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Azure IoT Hub is not connected, could not send message");
                return InvokeResult.FromError("Azure IoT Hub is not connected, could not send message");
            }

            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Sending message to Azure IoT Hub {_simulator.DefaultEndPoint}");

            var textPayload = ReplaceTokens(_instance, plan, messageTemplate.TextPayload);
            var msg = new Microsoft.Azure.Devices.Client.Message(GetMessageBytes(plan));
            await _azureIoTHubClient.SendEventAsync(msg);

            ReceivedContent = $"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessagePublished}";

            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, ReceivedContent);

            return InvokeResult.Success;
        }
    }
}
