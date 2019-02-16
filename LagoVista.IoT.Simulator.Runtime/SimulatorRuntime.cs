using Amqp;
using LagoVista.Client.Core;
using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Networking.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Simulator.Admin.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public partial class SimulatorRuntime : ISimulatorRuntime
    {
        LagoVista.IoT.Simulator.Admin.Models.Simulator _simulator;

        DeviceClient _azureIoTHubClient;
        private static Connection _amqpConnection;
        private static Session _amqpSession;

        IMQTTDeviceClient _mqttClient;
        ITCPClient _tcpClient;
        IUDPClient _udpClient;
        Random _random = new Random();
        Timer _timer;
        int _pointIndex;

        ISimulatorRuntimeServices _runtimeService;

        private DateTime _connected;
        private DateTime _lastAccess;

        IAdminLogger _adminLogger;

        bool _listening;

        INotificationPublisher _notificationPublisher;

        public SimulatorRuntime(ISimulatorRuntimeServices runtimeService, INotificationPublisher notificationPublisher, IAdminLogger adminLogger, LagoVista.IoT.Simulator.Admin.Models.Simulator simulator)
        {
            _runtimeService = runtimeService;
            _notificationPublisher = notificationPublisher;
            _connected = DateTime.UtcNow;
            _lastAccess = _connected;

            _simulator = simulator;

            _adminLogger = adminLogger;

            InstanceId = Guid.NewGuid().ToId();
        }

     



        private void StartReceiveThread()
        {
            Task.Run(async () =>
            {
                while (_isConnected)
                {
                    switch (_simulator.DefaultTransport.Value)
                    {
                        case TransportTypes.TCP:
                            {
                                var response = await _tcpClient.ReceiveAsync();
                                await AddReceviedMessage(new ReceivedMessage(response));
                            }

                            break;
                        case TransportTypes.UDP:
                            {
                                var response = await _udpClient.ReceiveAsync();
                                await AddReceviedMessage(new ReceivedMessage(response));
                            }
                            break;
                    }
                }
            });
        }


        Task _receivingTask;

        public async Task<InvokeResult> ConnectAsync()
        {
            try
            {
                IsBusy = true;
                switch (_simulator.DefaultTransport.Value)
                {
                    /*                    case TransportTypes.AMQP:
                                            {
                                                var connectionString = $"Endpoint=sb://{Model.DefaultEndPoint}.servicebus.windows.net/;SharedAccessKeyName={Model.AccessKeyName};SharedAccessKey={Model.AccessKey}";
                                                var bldr = new EventHubsConnectionStringBuilder(connectionString)
                                                {
                                                    EntityPath = Model.HubName
                                                };

                                                _isConnected = true;
                                            }

                                            break;*/

                    case TransportTypes.AzureIoTHub:
                        await ConnectAzureIoTHubAsync();
                        break;
                    case TransportTypes.MQTT:                        
                        await MQTTConnectAsync();
                        break;
                    case TransportTypes.TCP:
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connecting to {_simulator.DefaultTransport.Text} - {_simulator.DefaultEndPoint} on {_simulator.DefaultPort}.");

                        _tcpClient = _runtimeService.GetTCPClient();
                        await _tcpClient.ConnectAsync(_simulator.DefaultEndPoint, _simulator.DefaultPort);
                        StartReceiveThread();
                        SetConnectedState();

                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connected to {_simulator.DefaultTransport.Text} - {_simulator.DefaultEndPoint} on {_simulator.DefaultPort}.");
                        break;
                    case TransportTypes.UDP:
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connecting to {_simulator.DefaultTransport.Text} - {_simulator.DefaultEndPoint} on {_simulator.DefaultPort}.");

                        _udpClient = _runtimeService.GetUDPCLient();
                        await _udpClient.ConnectAsync(_simulator.DefaultEndPoint, _simulator.DefaultPort);
                        StartReceiveThread();
                        SetConnectedState();

                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connected to {_simulator.DefaultTransport.Text} - {_simulator.DefaultEndPoint} on {_simulator.DefaultPort}.");
                        break;
                    default:
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Attempt to connect to {_simulator.DefaultTransport.Text} that does not allow connections..");
                        break;
                }

                return InvokeResult.Success;

            }
            catch (Exception ex)
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Error connecting to {_simulator.DefaultTransport.Text} - {ex.Message}.");

                if (_mqttClient != null)
                {
                    _mqttClient.Dispose();
                    _mqttClient = null;
                }

                if (_azureIoTHubClient != null)
                {
                    await _azureIoTHubClient.CloseAsync();
                    _azureIoTHubClient.Dispose();
                    _azureIoTHubClient = null;
                }

                if (_tcpClient != null)
                {
                    await _tcpClient.DisconnectAsync();
                    _tcpClient.Dispose();
                    _tcpClient = null;
                }

                if (_udpClient != null)
                {
                    await _udpClient.DisconnectAsync();
                    _udpClient.Dispose();
                    _udpClient = null;
                }

                SetDisconnectedState();

                return InvokeResult.FromException("ConnectAsync", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

     

        private async Task<InvokeResult> SendServiceBusMessage(MessageTemplate messageTemplate)
        {
            if (String.IsNullOrEmpty(_simulator.DefaultEndPoint))
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Default End Point is Missing to send to Service Bus.");
                return InvokeResult.FromError("Default End Point is Missing to send to Event Hub.");
            }

            if (String.IsNullOrEmpty(_simulator.AccessKeyName))
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Access Key Name is Missing to send to Service Bus.");
                return InvokeResult.FromError("Access Key Name is Missing to send to Event Hub.");
            }

            if (String.IsNullOrEmpty(_simulator.QueueName))
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Queue Name is Missing to send to Service Bus.");
                return InvokeResult.FromError("Queue Name is Missing to send to Event Hub.");
            }

            if (String.IsNullOrEmpty(_simulator.AccessKey))
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Access Key is Missing to send to Service Bus.");
                return InvokeResult.FromError("Access Key is Missing to send to Event Hub.");
            }

            var connectionString = $"Endpoint=sb://{_simulator.DefaultEndPoint}.servicebus.windows.net/;SharedAccessKeyName={_simulator.AccessKeyName};SharedAccessKey={_simulator.AccessKey}";
            var bldr = new ServiceBusConnectionStringBuilder(connectionString)
            {
                EntityPath = messageTemplate.QueueName
            };

            var client = new QueueClient(bldr, ReceiveMode.PeekLock, Microsoft.Azure.ServiceBus.RetryExponential.Default);

            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                Body = GetMessageBytes(messageTemplate),
                To = messageTemplate.To,
                ContentType = messageTemplate.ContentType
            };

            if (!String.IsNullOrEmpty(msg.MessageId))
            {
                msg.MessageId = msg.MessageId;
            }

            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Sending message to Service Bus {_simulator.DefaultEndPoint}");

            await client.SendAsync(msg);
            await client.CloseAsync();

            ReceivedContent = $"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessagePublished}";

            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, ReceivedContent);

            return InvokeResult.Success;
        }

        private async Task<InvokeResult> SendEventHubMessage(MessageTemplate messageTemplate)
        {
            if (String.IsNullOrEmpty(_simulator.DefaultEndPoint))
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Default End Point is Missing to send to Event Hub.");
                return InvokeResult.FromError("Default End Point is Missing to send to Event Hub.");
            }

            if (String.IsNullOrEmpty(_simulator.AccessKeyName))
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Access Key Name is Missing to send to Event Hub.");
                return InvokeResult.FromError("Access Key Name is Missing to send to Event Hub.");
            }

            if (String.IsNullOrEmpty(_simulator.HubName))
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Hub Name is Missing to send to Event Hub.");
                return InvokeResult.FromError("Hub Name is Missing to send to Event Hub.");
            }

            if (String.IsNullOrEmpty(_simulator.AccessKey))
            {
                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Access Key is Missing to send to Event Hub.");
                return InvokeResult.FromError("Access Key is Missing to send to Event Hub.");
            }

            var connectionString = $"Endpoint=sb://{_simulator.DefaultEndPoint}.servicebus.windows.net/;SharedAccessKeyName={_simulator.AccessKeyName};SharedAccessKey={_simulator.AccessKey}";
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString) { EntityPath = _simulator.HubName };

            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Sending message to Azure Event Hub {_simulator.DefaultEndPoint}");

            var client = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            await client.SendAsync(new EventData(GetMessageBytes(messageTemplate)));

            ReceivedContent = $"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessagePublished}";

            return InvokeResult.Success;
        }    

        private async Task<InvokeResult> SendGeoMessage(MessageTemplate messageTemplate)
        {
            var pointArray = messageTemplate.TextPayload.Split('\r');
            var geoLocation = pointArray[_pointIndex++];
            var parts = geoLocation.Split(',');
            var lat = Convert.ToDouble(parts[0]);
            var lon = Convert.ToDouble(parts[1]);
            var delay = Convert.ToInt32(parts[2]) * 1000;

            using (var client = new HttpClient())
            {
                var protocol = messageTemplate.Transport.Value == TransportTypes.RestHttps ? "https" : "http";
                var uri = $"{protocol}://{_simulator.DefaultEndPoint}:{_simulator.DefaultPort}{ReplaceTokens(messageTemplate, messageTemplate.PathAndQueryString)}";

                if (!this._simulator.Anonymous)
                {
                    if (String.IsNullOrEmpty(_simulator.Password))
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Simulator is not anonymous however credentials not supplied.");
                        return InvokeResult.FromError("Simulator is not anonymous however credentials not supplied.");
                    }

                    var authCreds = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(_simulator.UserName + ":" + _simulator.Password));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authCreds);
                }

                System.Net.Http.HttpResponseMessage responseMessage = null;

                foreach (var hdr in messageTemplate.MessageHeaders)
                {
                    client.DefaultRequestHeaders.Add(hdr.HeaderName, ReplaceTokens(messageTemplate, hdr.Value));
                }

                var messageBody = ReplaceTokens(messageTemplate, messageTemplate.TextPayload);
                messageBody = $"{{'latitude':{lat}, 'longitude':{lon}}}";

                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Sending Geo Point {messageBody} via {messageTemplate.HttpVerb} to {uri}.");

                try
                {
                    switch (messageTemplate.HttpVerb)
                    {
                        case MessageTemplate.HttpVerb_GET:
                            responseMessage = await client.GetAsync(uri);
                            break;
                        case MessageTemplate.HttpVerb_POST:
                            responseMessage = await client.PostAsync(uri, new StringContent(messageBody, Encoding.UTF8, String.IsNullOrEmpty(messageTemplate.ContentType) ? "text/plain" : messageTemplate.ContentType));
                            break;
                        case MessageTemplate.HttpVerb_PUT:
                            responseMessage = await client.PutAsync(uri, new StringContent(messageBody, Encoding.UTF8, String.IsNullOrEmpty(messageTemplate.ContentType) ? "text/plain" : messageTemplate.ContentType));
                            break;
                        case MessageTemplate.HttpVerb_DELETE:
                            responseMessage = await client.DeleteAsync(uri);
                            break;
                    }
                }
                catch (HttpRequestException ex)
                {
                    var fullResponseString = new StringBuilder();
                    fullResponseString.AppendLine(ex.Message);
                    if (ex.InnerException != null)
                    {
                        fullResponseString.AppendLine(ex.InnerException.Message);
                    }

                    ReceivedContent = fullResponseString.ToString();

                    await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Error sending GeoPoint: {ex.Message}.");

                    return InvokeResult.FromException("SendRESTRequestAsync", ex);
                }

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    var fullResponseString = new StringBuilder();
                    fullResponseString.AppendLine($"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessageSent}");
                    fullResponseString.AppendLine($"Response Code: {(int)responseMessage.StatusCode} ({responseMessage.ReasonPhrase})");
                    foreach (var hdr in responseMessage.Headers)
                    {
                        fullResponseString.AppendLine($"{hdr.Key}\t:{hdr.Value.FirstOrDefault()}");
                    }
                    fullResponseString.AppendLine();
                    fullResponseString.Append(responseContent);
                    ReceivedContent = fullResponseString.ToString();
                    if (this._pointIndex < pointArray.Length)
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Queue up point {_pointIndex} to send.");
                        _timer = new Timer(SentNextPoint, null, delay, Timeout.Infinite);
                    }

                    return InvokeResult.Success;
                }
                else
                {
                    ReceivedContent = $"{responseMessage.StatusCode} - {responseMessage.ReasonPhrase}";
                    await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Error sending GeoPoint: {ReceivedContent}.");
                    return InvokeResult.FromError(ReceivedContent);
                }
            }

        }

        private async Task<InvokeResult> SendRESTRequestAsync(MessageTemplate messageTemplate)
        {
            if (messageTemplate.PayloadType.Id == MessageTemplate.PayloadTypes_GeoPath)
            {
                return await SendGeoMessage(messageTemplate);
            }

            using (var client = new HttpClient())
            {
                var protocol = messageTemplate.Transport.Value == TransportTypes.RestHttps ? "https" : "http";

                String uri = null;
                if (!String.IsNullOrEmpty(messageTemplate.EndPoint))
                {
                    uri = $"{protocol}://{messageTemplate.EndPoint}:{messageTemplate.Port}{ReplaceTokens(messageTemplate, messageTemplate.PathAndQueryString)}";
                }
                else if (!String.IsNullOrEmpty(_simulator.DefaultEndPoint))
                {
                    uri = $"{protocol}://{_simulator.DefaultEndPoint}:{_simulator.DefaultPort}{ReplaceTokens(messageTemplate, messageTemplate.PathAndQueryString)}";
                }

                if (String.IsNullOrEmpty(uri))
                {
                    await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "End Point must be provided at the Simulator or Message Level");
                    return InvokeResult.FromError("End Point must be provided at the Simulator or Message Level");
                }

                if (!_simulator.Anonymous)
                {
                    if (_simulator.BasicAuth)
                    {
                        if (String.IsNullOrEmpty(_simulator.Password) || String.IsNullOrEmpty(_simulator.UserName))
                        {
                            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Simulator is not anonymous however user name and password not supplied for basic auth.");
                            return InvokeResult.FromError("Simulator is not anonymous however user name and password not supplied for basic auth.");
                        }

                        var authCreds = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(_simulator.UserName + ":" + _simulator.Password));
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authCreds);
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(_simulator.AuthHeader))
                        {
                            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, "Simulator is not anonymous however auth header is not provided and not basic auth.");
                            return InvokeResult.FromError("Simulator is not anonymous however auth header is not provided and not basic auth..");
                        }

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_simulator.AuthHeader);
                    }
                }

                System.Net.Http.HttpResponseMessage responseMessage = null;

                foreach (var hdr in messageTemplate.MessageHeaders)
                {
                    client.DefaultRequestHeaders.Add(hdr.HeaderName, ReplaceTokens(messageTemplate, hdr.Value));
                }

                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Sending: {messageTemplate.HttpVerb} to {uri}.");

                var messageBody = ReplaceTokens(messageTemplate, messageTemplate.TextPayload);
                try
                {
                    switch (messageTemplate.HttpVerb)
                    {
                        case MessageTemplate.HttpVerb_GET:
                            responseMessage = await client.GetAsync(uri);
                            break;
                        case MessageTemplate.HttpVerb_POST:
                            responseMessage = await client.PostAsync(uri, new StringContent(messageBody, Encoding.UTF8, String.IsNullOrEmpty(messageTemplate.ContentType) ? "text/plain" : messageTemplate.ContentType));
                            break;
                        case MessageTemplate.HttpVerb_PUT:
                            responseMessage = await client.PutAsync(uri, new StringContent(messageBody, Encoding.UTF8, String.IsNullOrEmpty(messageTemplate.ContentType) ? "text/plain" : messageTemplate.ContentType));
                            break;
                        case MessageTemplate.HttpVerb_DELETE:
                            responseMessage = await client.DeleteAsync(uri);
                            break;
                        default:
                            await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Uknown HTTP Verb [{messageTemplate.HttpVerb}]");
                            return InvokeResult.FromError($"Uknown HTTP Verb [{messageTemplate.HttpVerb}]");
                    }
                }
                catch (HttpRequestException ex)
                {
                    var fullResponseString = new StringBuilder();
                    fullResponseString.AppendLine(ex.Message);
                    if (ex.InnerException != null)
                    {
                        fullResponseString.AppendLine(ex.InnerException.Message);
                    }

                    await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Error sending message: {ex.Message}.");

                    ReceivedContent = fullResponseString.ToString();
                    return InvokeResult.FromException("SendRESTRequestAsync", ex);
                }

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    var fullResponseString = new StringBuilder();
                    fullResponseString.AppendLine($"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessageSent}");
                    fullResponseString.AppendLine($"Response Code: {(int)responseMessage.StatusCode} ({responseMessage.ReasonPhrase})");
                    foreach (var hdr in responseMessage.Headers)
                    {
                        fullResponseString.AppendLine($"{hdr.Key}\t:{hdr.Value.FirstOrDefault()}");
                    }
                    fullResponseString.AppendLine();
                    fullResponseString.Append(responseContent);
                    ReceivedContent = fullResponseString.ToString();

                    if (ReceivedContent.Length > 255)
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, ReceivedContent.Substring(0, 250));
                    }
                    else
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, ReceivedContent);
                    }

                    await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Success sending message.");

                    return InvokeResult.Success;
                }
                else
                {
                    ReceivedContent = $"{responseMessage.StatusCode} - {responseMessage.ReasonPhrase}";

                    await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Error sending message: {ReceivedContent}.");
                    return InvokeResult.FromError(ReceivedContent);
                }
            }
        }

        private async Task<InvokeResult> SendTCPMessage(MessageTemplate messageTemplate)
        {
            var buffer = GetMessageBytes(messageTemplate);
            await _udpClient.WriteAsync(buffer, 0, buffer.Length);

            return InvokeResult.Success;
        }

        private async Task<InvokeResult> SendUDPMessage(MessageTemplate messageTemplate)
        {
            var buffer = GetMessageBytes(messageTemplate);
            await _tcpClient.WriteAsync(buffer, 0, buffer.Length);

            return InvokeResult.Success;
        }

        private async void SentNextPoint(Object obj)
        {
            await SendRESTRequestAsync(obj as MessageTemplate);
        }

        public async Task<InvokeResult<string>> SendAsync(MessageTemplate messageTemplate)
        {
            IsBusy = true;

            try
            {
                InvokeResult res = InvokeResult.FromError("");

                switch (messageTemplate.Transport.Value)
                {
                    case TransportTypes.TCP: res = await SendTCPMessage(messageTemplate); break;
                    case TransportTypes.UDP: res = await SendUDPMessage(messageTemplate); break;
                    case TransportTypes.AzureServiceBus: res = await SendServiceBusMessage(messageTemplate); break;
                    case TransportTypes.AzureEventHub: res = await SendEventHubMessage(messageTemplate); break;
                    case TransportTypes.AzureIoTHub: res = await SendIoTHubMessage(messageTemplate); break;
                    case TransportTypes.MQTT: res = await SendMQTTMessage(messageTemplate); break;
                    case TransportTypes.RestHttps:
                    case TransportTypes.RestHttp: res = await SendRESTRequestAsync(messageTemplate); break;
                }

                if (res.Successful)
                {
                    var msg = BuildRequestContent(messageTemplate);
                    return InvokeResult<string>.Create(msg);
                }
                else
                {
                    return InvokeResult<string>.FromInvokeResult(res);
                }
            }
            catch (Exception ex)
            {
                _adminLogger.AddException("Send", ex);

                ReceivedContent = ex.Message;

                await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Error sending {messageTemplate.Transport.Value} message {ex.Message}.");

                return InvokeResult<string>.FromException("SendAsync", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<InvokeResult> DisconnectAsync()
        {
            switch (_simulator.DefaultTransport.Value)
            {
                case TransportTypes.AMQP:
                    break;
                case TransportTypes.AzureIoTHub:

                    if (_azureIoTHubClient != null)
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Disconnecting from {_simulator.DefaultTransport.Text}.");

                        await _azureIoTHubClient.CloseAsync();
                        _azureIoTHubClient.Dispose();
                        _azureIoTHubClient = null;
                    }
                    else
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Disconnecting called for {_simulator.DefaultTransport.Text} but not connected.");
                        return InvokeResult.FromError($"Disconnecting called for {_simulator.DefaultTransport.Text} but not connected.");
                    }

                    break;
                case TransportTypes.MQTT:
                    if (_mqttClient != null)
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Disconnecting from {_simulator.DefaultTransport.Text}.");

                        _mqttClient.Disconnect();
                        _mqttClient = null;
                    }
                    else
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Disconnecting called for {_simulator.DefaultTransport.Text} but not connected.");
                        return InvokeResult.FromError($"Disconnecting called for {_simulator.DefaultTransport.Text} but not connected.");
                    }

                    break;
                case TransportTypes.TCP:
                    if (_tcpClient != null)
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Disconnecting from {_simulator.DefaultTransport.Text}.");

                        await _tcpClient.DisconnectAsync();
                        _tcpClient.Dispose();
                    }
                    else
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Disconnecting called for {_simulator.DefaultTransport.Text} but not connected.");
                        return InvokeResult.FromError($"Disconnecting called for {_simulator.DefaultTransport.Text} but not connected.");
                    }

                    break;
                case TransportTypes.UDP:
                    if (_udpClient != null)
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Disconnecting from {_simulator.DefaultTransport.Text}.");

                        await _udpClient.DisconnectAsync();
                        _udpClient.Dispose();
                    }
                    else
                    {
                        await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Disconnecting called for {_simulator.DefaultTransport.Text} but not connected.");
                        return InvokeResult.FromError($"Disconnecting called for {_simulator.DefaultTransport.Text} but not connected.");
                    }
                    break;

                default:
                    await _notificationPublisher.PublishTextAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Disconnecting called for {_simulator.DefaultTransport.Text} but not a connection to be closed.");
                    return InvokeResult.FromError($"Disconnecting called for {_simulator.DefaultTransport.Text} but not a connection to be closed.");
            }

            if (_receivingTask != null)
            {

            }

            SetDisconnectedState();

            return InvokeResult.Success;
        }

        public bool IsBusy
        {
            get { return _runtimeService.IsBusy; }
            set { _runtimeService.IsBusy = value; }
        }

        public string InstanceId { get; }


        private String _receivedContent;
        public string ReceivedContent
        {
            get { return _receivedContent; }
            set
            {
                _receivedContent = value;
                _runtimeService.ReceivedContent = value;
            }
        }

        private bool _isConnected;

        private async void SetConnectedState()
        {
            var connectionStatus = new ConnectionStatus()
            {
                Connected = true,
                DateStamp = DateTime.UtcNow.ToJSONString(),
                EndPoint = _simulator.DefaultEndPoint,
                TransportType = _simulator.DefaultTransport,
            };

            await _notificationPublisher.PublishAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connected to {_simulator.DefaultTransport.Text}.", connectionStatus);

            _isConnected = true;
            _runtimeService.Connected();
        }

        private async void SetDisconnectedState()
        {
            _isConnected = false;

            var connectionStatus = new ConnectionStatus()
            {
                Connected = false,
                DateStamp = DateTime.UtcNow.ToJSONString(),
                EndPoint = _simulator.DefaultEndPoint,
                TransportType = _simulator.DefaultTransport,
            };

            await _notificationPublisher.PublishAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Connected to {_simulator.DefaultTransport.Text}.", connectionStatus);

            _runtimeService.Disconnected();
        }

        private async Task AddReceviedMessage(ReceivedMessage msg)
        {
            await _notificationPublisher.PublishAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Topic: {msg.Topic}");
            await _notificationPublisher.PublishAsync(Targets.WebSocket, Channels.Simulator, InstanceId, msg.TextPayload);
            await _runtimeService.AddReceviedMessage(msg);
        }
    }

    public class ConnectionStatus
    {
        public bool Connected { get; set; }
        public String DateStamp { get; set; }
        public string EndPoint { get; set; }
        public EntityHeader<TransportTypes> TransportType { get; set; }
    }
}
