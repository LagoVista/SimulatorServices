using LagoVista.Client.Core;
using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Networking.Models;
using LagoVista.Core.Validation;
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
    public class SimulatorRuntime : ISimulatorRuntime
    {
        LagoVista.IoT.Simulator.Admin.Models.Simulator _simulator;

        DeviceClient _azureIoTHubClient;
        IMQTTDeviceClient _mqttClient;
        ITCPClient _tcpClient;
        IUDPClient _udpClient;
        Random _random = new Random();
        Timer _timer;
        int _pointIndex;

        ISimulatorRuntimeServices _runtimeService;

        private DateTime _connected;
        private DateTime _lastAccess;
        

        public SimulatorRuntime(ISimulatorRuntimeServices runtimeService, LagoVista.IoT.Simulator.Admin.Models.Simulator simulator)
        {
            _runtimeService = runtimeService;

            _connected = DateTime.UtcNow;
            _lastAccess = _connected;

            _simulator = simulator;

            InstanceId = Guid.NewGuid().ToId();
        }

        private String BuildRequestContent(MessageTemplate messageTemplate)
        {
            var sentContent = new StringBuilder();

            switch (messageTemplate.Transport.Value)
            {

                case TransportTypes.TCP:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port   : {messageTemplate.Port}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;
                case TransportTypes.UDP:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port   : {messageTemplate.Port}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;
                case TransportTypes.AzureIoTHub:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port   : {messageTemplate.Port}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;

                case TransportTypes.AzureEventHub:
                    sentContent.AppendLine($"Host   : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;

                case TransportTypes.AzureServiceBus:
                    sentContent.AppendLine($"Host   : {messageTemplate.Name}");
                    //sentContent.AppendLine($"Queue   : {MsgTemplate.Qu}");
                    sentContent.AppendLine($"Body");
                    sentContent.AppendLine($"---------------------------------");
                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;
                case TransportTypes.MQTT:
                    sentContent.AppendLine($"Host         : {_simulator.DefaultEndPoint}");
                    sentContent.AppendLine($"Port         : {messageTemplate.Port}");
                    sentContent.AppendLine($"Topics");
                    sentContent.AppendLine($"Publish      : {ReplaceTokens(messageTemplate, messageTemplate.Topic)}");
                    sentContent.AppendLine($"Subscription : {ReplaceTokens(messageTemplate, _simulator.Subscription)}");

                    sentContent.Append(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));

                    break;
                case TransportTypes.RestHttps:
                case TransportTypes.RestHttp:
                    {
                        var protocol = messageTemplate.Transport.Value == TransportTypes.RestHttps ? "https" : "http";
                        var uri = $"{protocol}://{_simulator.DefaultEndPoint}:{_simulator.DefaultPort}{ReplaceTokens(messageTemplate, messageTemplate.PathAndQueryString)}";
                        sentContent.AppendLine($"Method       : {messageTemplate.HttpVerb}");
                        sentContent.AppendLine($"Endpoint     : {uri}");
                        var contentType = String.IsNullOrEmpty(messageTemplate.ContentType) ? "text/plain" : messageTemplate.ContentType;
                        sentContent.AppendLine($"Content Type : {contentType}");

                        if (_simulator.BasicAuth)
                        {
                            var authCreds = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(_simulator.UserName + ":" + _simulator.Password));
                            sentContent.AppendLine($"Authorization: Basic {authCreds}");
                        }
                        if (messageTemplate.MessageHeaders.Any())
                        {
                            sentContent.AppendLine($"Custom Headers");
                        }

                        var idx = 1;
                        foreach (var hdr in messageTemplate.MessageHeaders)
                        {
                            sentContent.AppendLine($"\t{idx++}. {hdr.HeaderName}={ReplaceTokens(messageTemplate, hdr.Value)}");
                        }

                        if (messageTemplate.HttpVerb == "POST" || messageTemplate.HttpVerb == "PUT")
                        {
                            sentContent.AppendLine("");
                            sentContent.AppendLine("Post Content");
                            sentContent.AppendLine("=========================");
                            sentContent.AppendLine(ReplaceTokens(messageTemplate, messageTemplate.TextPayload));
                        }
                    }
                    break;
            }

            return sentContent.ToString();
        }

        private byte[] GetMessageBytes(MessageTemplate messageTemplate)
        {
            if (EntityHeader.IsNullOrEmpty(messageTemplate.PayloadType) || messageTemplate.PayloadType.Value == PaylodTypes.Binary)
            {
                return GetBinaryPayload(messageTemplate.BinaryPayload);
            }
            else
            {
                var msgText = ReplaceTokens(messageTemplate, messageTemplate.TextPayload);
                return System.Text.UTF8Encoding.UTF8.GetBytes(msgText);
            }
        }

        private byte[] GetBinaryPayload(string binaryPayload)
        {
            if (String.IsNullOrEmpty(binaryPayload))
            {
                return new byte[0];
            }

            try
            {
                var bytes = new List<Byte>();

                if (binaryPayload.Length % 2 == 0 && !binaryPayload.StartsWith("0x"))
                {
                    for (var idx = 0; idx < binaryPayload.Length; idx += 2)
                    {
                        var byteStr = binaryPayload.Substring(idx, 2);
                        bytes.Add(Byte.Parse(byteStr, System.Globalization.NumberStyles.HexNumber));
                    }
                }
                else
                {
                    var bytesList = binaryPayload.Split(' ');
                    foreach (var byteStr in bytesList)
                    {
                        var lowerByteStr = byteStr.ToLower();
                        if (lowerByteStr.Contains("soh"))
                        {
                            bytes.Add(0x01);
                        }
                        else if (lowerByteStr.Contains("stx"))
                        {
                            bytes.Add(0x02);
                        }
                        else if (lowerByteStr.Contains("etx"))
                        {
                            bytes.Add(0x03);
                        }
                        else if (lowerByteStr.Contains("eot"))
                        {
                            bytes.Add(0x04);
                        }
                        else if (lowerByteStr.Contains("ack"))
                        {
                            bytes.Add(0x06);
                        }
                        else if (lowerByteStr.Contains("cr"))
                        {
                            bytes.Add(0x0d);
                        }
                        else if (lowerByteStr.Contains("lf"))
                        {
                            bytes.Add(0x0a);
                        }
                        else if (lowerByteStr.Contains("nak"))
                        {
                            bytes.Add(0x15);
                        }
                        else if (lowerByteStr.Contains("esc"))
                        {
                            bytes.Add(0x1b);
                        }
                        else if (lowerByteStr.Contains("del"))
                        {
                            bytes.Add(0x1b);
                        }
                        else if ((lowerByteStr.StartsWith("x")))
                        {
                            bytes.Add(Byte.Parse(byteStr.Substring(1), System.Globalization.NumberStyles.HexNumber));
                        }
                        else if (lowerByteStr.StartsWith("0x"))
                        {
                            bytes.Add(Byte.Parse(byteStr.Substring(2), System.Globalization.NumberStyles.HexNumber));
                        }
                        else
                        {
                            bytes.Add(Byte.Parse(byteStr, System.Globalization.NumberStyles.HexNumber));
                        }
                    }
                }

                return bytes.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(Simulator.Runtime.SimulatorRuntimeResources.SendMessage_InvalidBinaryPayload + " " + ex.Message);
            }
        }

        private async Task ReceiveDataFromAzure()
        {
            while (_azureIoTHubClient != null)
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

        private async void _mqttClient_CommandReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var msg = new ReceivedMessage(e.Message);
            msg.Topic = e.Topic;
            msg.MessageId = e.MessageId;
            await AddReceviedMessage(msg);
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
                        var connectionString = $"HostName={_simulator.DefaultEndPoint};DeviceId={_simulator.DeviceId};SharedAccessKey={_simulator.AccessKey}";
                        _azureIoTHubClient = DeviceClient.CreateFromConnectionString(connectionString, Microsoft.Azure.Devices.Client.TransportType.Amqp_Tcp_Only);
                        await _azureIoTHubClient.OpenAsync();
                        _receivingTask = Task.Run(ReceiveDataFromAzure);
                        SetConnectedState();
                        break;
                    case TransportTypes.MQTT:
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
                        }
                        else
                        {
                            await ShowMessageAsync($"{SimulatorRuntimeResources.Simulator_ErrorConnecting}: {result.Result.ToString()}");
                        }

                        break;
                    case TransportTypes.TCP:
                        _tcpClient = _runtimeService.GetTCPClient();
                        await _tcpClient.ConnectAsync(_simulator.DefaultEndPoint, _simulator.DefaultPort);
                        StartReceiveThread();
                        SetConnectedState();
                        break;
                    case TransportTypes.UDP:
                        _udpClient = _runtimeService.GetUDPCLient();
                        await _udpClient.ConnectAsync(_simulator.DefaultEndPoint, _simulator.DefaultPort);
                        StartReceiveThread();
                        SetConnectedState();
                        break;
                }

                return InvokeResult.Success;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await ShowMessageAsync(ex.Message);
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

        private String ReplaceTokens(MessageTemplate msg, String input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return String.Empty;
            }

            foreach (var attr in msg.DynamicAttributes)
            {
                input = input.Replace($"~{attr.Key}~", attr.DefaultValue);
            }

            input = input.Replace($"~deviceid~", _simulator.DeviceId);
            input = input.Replace($"~datetime~", DateTime.Now.ToString());
            input = input.Replace($"~username~", _simulator.UserName);
            input = input.Replace($"~password~", _simulator.Password);
            input = input.Replace($"~accesskey~", _simulator.AccessKey);
            input = input.Replace($"~password~", _simulator.Password);
            input = input.Replace($"~datetimeiso8601~", DateTime.UtcNow.ToJSONString());

            var floatRegEx = new Regex(@"~random-float,(?'min'[+-]?(([0-9]*[.]?)?[0-9]+)),(?'max'[+-]?([0-9]*[.])?[0-9]+)~");
            var intRegEx = new Regex(@"~random-int,(?'min'[+-]?\d+),(?'max'[+-]?\d+)~");
            var floatMatches = floatRegEx.Matches(input);

            foreach (Match match in floatMatches)
            {
                if (float.TryParse(match.Groups["min"].Value, out float minValue) && float.TryParse(match.Groups["max"].Value, out float maxValue))
                {
                    if (minValue > maxValue)
                    {
                        var tmp = maxValue;
                        maxValue = minValue;
                        minValue = tmp;
                    }

                    Debug.WriteLine(minValue + " " + maxValue);
                    var delta = maxValue - minValue;
                    var value = delta * _random.NextDouble() + minValue;
                    input = input.Replace(match.Value, Math.Round(value, 2).ToString());
                }
            }

            var intMatches = intRegEx.Matches(input);

            foreach (Match match in intMatches)
            {
                if (int.TryParse(match.Groups["min"].Value, out int minValue) && int.TryParse(match.Groups["max"].Value, out int maxValue))
                {
                    if (minValue > maxValue)
                    {
                        var tmp = maxValue;
                        maxValue = minValue;
                        minValue = tmp;
                    }
                    var delta = maxValue - minValue;
                    var value = _random.Next(minValue, maxValue);
                    input = input.Replace(match.Value, value.ToString());
                }
            }

            if (msg.AppendCR)
            {
                input += "\r";
            }

            if (msg.AppendLF)
            {
                input += "\n";
            }

            return input;
        }

        private async Task SendServiceBusMessage(MessageTemplate messageTemplate)
        {
            if (String.IsNullOrEmpty(_simulator.AccessKey))
            {
                await ShowMessageAsync("Access Key is missing");
                return;
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

            await client.SendAsync(msg);
            await client.CloseAsync();

            ReceivedContent = $"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessagePublished}";
        }

        private async Task SendEventHubMessage(MessageTemplate messageTemplate)
        {
            if (String.IsNullOrEmpty(_simulator.AccessKey))
            {
                await ShowMessageAsync("Access Key is missing");
                return;
            }

            var connectionString = $"Endpoint=sb://{_simulator.DefaultEndPoint}.servicebus.windows.net/;SharedAccessKeyName={_simulator.AccessKeyName};SharedAccessKey={_simulator.AccessKey}";
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString) { EntityPath = _simulator.HubName };

            var client = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            await client.SendAsync(new EventData(GetMessageBytes(messageTemplate)));
            ReceivedContent = $"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessageSent}";
        }

        private async Task SendIoTHubMessage(MessageTemplate messageTemplate)
        {
            var textPayload = ReplaceTokens(messageTemplate, messageTemplate.TextPayload);
            var msg = new Microsoft.Azure.Devices.Client.Message(GetMessageBytes(messageTemplate));
            await _azureIoTHubClient.SendEventAsync(msg);

            ReceivedContent = $"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessagePublished}";
        }

        private async Task SendMQTTMessage(MessageTemplate messageTemplate)
        {
            var qos = QOS.QOS0;

            if (!EntityHeader.IsNullOrEmpty(messageTemplate.QualityOfServiceLevel))
            {
                switch (messageTemplate.QualityOfServiceLevel.Value)
                {
                    case QualityOfServiceLevels.QOS1: qos = QOS.QOS1; break;
                    case QualityOfServiceLevels.QOS2: qos = QOS.QOS2; break;
                }
            }

            await _mqttClient.PublishAsync(ReplaceTokens(messageTemplate, messageTemplate.Topic), GetMessageBytes(messageTemplate), qos, messageTemplate.RetainFlag);

            ReceivedContent = $"{DateTime.Now} {SimulatorRuntimeResources.SendMessage_MessagePublished}";
        }

        private async Task SendGeoMessage(MessageTemplate messageTemplate)
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
                        await ShowMessageAsync("Password is missing");
                        return;
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
                    return;
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
                }
                else
                {
                    ReceivedContent = $"{responseMessage.StatusCode} - {responseMessage.ReasonPhrase}";
                }
            }

            if (this._pointIndex < pointArray.Length)
            {
                _timer = new Timer(SentNextPoint, null, delay, Timeout.Infinite);
            }
        }

        private async Task SendRESTRequest(MessageTemplate messageTemplate)
        {
            if (messageTemplate.PayloadType.Id == MessageTemplate.PayloadTypes_GeoPath)
            {
                await SendGeoMessage(messageTemplate);
            }

            using (var client = new HttpClient())
            {
                var protocol = messageTemplate.Transport.Value == TransportTypes.RestHttps ? "https" : "http";
                var uri = $"{protocol}://{_simulator.DefaultEndPoint}:{_simulator.DefaultPort}{ReplaceTokens(messageTemplate, messageTemplate.PathAndQueryString)}";

                if (!_simulator.Anonymous)
                {
                    if (String.IsNullOrEmpty(_simulator.Password))
                    {
                        await ShowMessageAsync("Password is missing");
                        return;
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
                    return;
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
                }
                else
                {
                    ReceivedContent = $"{responseMessage.StatusCode} - {responseMessage.ReasonPhrase}";
                }
            }
        }

        private async Task SendTCPMessage(MessageTemplate messageTemplate)
        {
            var buffer = GetMessageBytes(messageTemplate);
            await _udpClient.WriteAsync(buffer, 0, buffer.Length);
        }

        private async Task SendUDPMessage(MessageTemplate messageTemplate)
        {
            var buffer = GetMessageBytes(messageTemplate);
            await _tcpClient.WriteAsync(buffer, 0, buffer.Length);
        }

        private async void SentNextPoint(Object obj)
        {
            await SendRESTRequest(obj as MessageTemplate);
        }

        public async Task<String> Send(MessageTemplate messageTemplate)
        {
            IsBusy = true;

            try
            {
                switch (messageTemplate.Transport.Value)
                {
                    case TransportTypes.TCP: await SendTCPMessage(messageTemplate); break;
                    case TransportTypes.UDP: await SendUDPMessage(messageTemplate); break;
                    case TransportTypes.AzureServiceBus: await SendServiceBusMessage(messageTemplate); break;
                    case TransportTypes.AzureEventHub: await SendEventHubMessage(messageTemplate); break;
                    case TransportTypes.AzureIoTHub: await SendIoTHubMessage(messageTemplate); break;
                    case TransportTypes.MQTT: await SendMQTTMessage(messageTemplate); break;
                    case TransportTypes.RestHttps:
                    case TransportTypes.RestHttp: await SendRESTRequest(messageTemplate); break;
                }

                return BuildRequestContent(messageTemplate);
            }
            catch (Exception ex)
            {
                await ShowMessageAsync(ex.Message);
                ReceivedContent = ex.Message;
                return ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task DisconnectAsync()
        {
            switch (_simulator.DefaultTransport.Value)
            {
                /*case TransportTypes.AMQP:
                    ConnectButtonVisible = true;
                    break;*/
                case TransportTypes.AzureIoTHub:
                    if (_azureIoTHubClient != null)
                    {
                        await _azureIoTHubClient.CloseAsync();
                        _azureIoTHubClient.Dispose();
                        _azureIoTHubClient = null;
                    }

                    break;
                case TransportTypes.MQTT:
                    if (_mqttClient != null)
                    {
                        _mqttClient.Disconnect();
                        _mqttClient = null;
                    }

                    break;
                case TransportTypes.TCP:
                    if (_tcpClient != null)
                    {
                        await _tcpClient.DisconnectAsync();
                        _tcpClient.Dispose();
                    }

                    break;
                case TransportTypes.UDP:
                    if (_udpClient != null)
                    {
                        await _udpClient.DisconnectAsync();
                        _udpClient.Dispose();
                    }
                    break;
            }

            if(_receivingTask != null)
            {
                
            }

            SetDisconnectedState(); 
        }

        private Task ShowMessageAsync(string msg)
        {
            return Task.FromResult(default(object));
        }        

        public bool IsBusy
        {
            get { return _runtimeService.IsBusy; }
            set { _runtimeService.IsBusy = value; }
        }

        public string InstanceId { get; }

        public string ReceivedContent
        {
            get { return _runtimeService.ReceivedContent; }
            set { _runtimeService.ReceivedContent = value; }
        }

        private bool _isConnected;

        private void SetConnectedState()
        {
            _isConnected = true;
            _runtimeService.Connected();
        }

        private void SetDisconnectedState()
        {
            _isConnected = false;
            _runtimeService.Disconnected();
        }

        private Task AddReceviedMessage(ReceivedMessage msg)
        {
            return _runtimeService.AddReceviedMessage(msg);
        }
    }
}
