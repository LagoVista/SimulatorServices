﻿using LagoVista.Core.Models;
using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Networking.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Simulator.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public partial class SimulatorRuntime
    {
        private async void _mqttClient_CommandReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var msg = new ReceivedMessage(e.Message);
            msg.Topic = e.Topic;
            msg.MessageId = e.MessageId;
            await AddReceviedMessage(msg);
        }

        public async Task MQTTConnectAsync()
        {
            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connecting to {_simulator.DefaultTransport.Text} - {_simulator.DefaultEndPoint} on {_simulator.DefaultPort}.");

            _mqttClient = _runtimeService.GetMQTTClient();
            _mqttClient.ShowDiagnostics = true;
            _mqttClient.BrokerHostName = _simulator.DefaultEndPoint;
            _mqttClient.BrokerPort = _simulator.DefaultPort;
            _mqttClient.DeviceId = _simulator.UserName;
            _mqttClient.Password = _simulator.Password;
            var result = await _mqttClient.ConnectAsync();
            if (result.Result == ConnAck.Accepted)
            {
                if (!String.IsNullOrEmpty(_simulator.Subscription))
                {
                    var subscription = new MQTTSubscription()
                    {
                        Topic = _simulator.Subscription.Replace("~deviceid~", _simulator.DeviceId),
                        QOS = EntityHeader<QOS>.Create(QOS.QOS2)
                    };
                    await _mqttClient.SubscribeAsync(subscription);
                    _mqttClient.MessageReceived += _mqttClient_CommandReceived;
                }

                SetConnectedState();

                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connected to {_simulator.DefaultTransport.Text} - {_simulator.DefaultEndPoint} on {_simulator.DefaultPort}.");
            }
            else
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Error connecting to {_simulator.DefaultTransport.Text} - {_simulator.DefaultEndPoint} on {_simulator.DefaultPort}.");
            }
        }


        private async Task<InvokeResult> SendMQTTMessage(MessageTransmissionPlan plan)
        {
            var messageTemplate = plan.Message.Value;

            if (_mqttClient == null)
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "MQTT Client is null, could not send message");
                return InvokeResult.FromError("MQTT Client is null, could not send message");
            }

            if (!_isConnected)
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "MQTT Client is not connected, could not send message");
                return InvokeResult.FromError("MQTT Client is not is not connected, could not send message");
            }

            var qos = QOS.QOS0;

            if (!EntityHeader.IsNullOrEmpty(messageTemplate.QualityOfServiceLevel))
            {
                switch (messageTemplate.QualityOfServiceLevel.Value)
                {
                    case QualityOfServiceLevels.QOS1: qos = QOS.QOS1; break;
                    case QualityOfServiceLevels.QOS2: qos = QOS.QOS2; break;
                }
            }

            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Sending message to MQTT Server {_simulator.DefaultEndPoint} with topic {messageTemplate.Topic}");

            await _mqttClient.PublishAsync(ReplaceTokens(_instance, plan, messageTemplate.Topic), GetMessageBytes(plan), qos, messageTemplate.RetainFlag);

            ReceivedContent = $"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessagePublished}";

            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, ReceivedContent);

            return InvokeResult.Success;
        }
    }
}
