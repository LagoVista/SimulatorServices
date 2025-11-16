// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: a3b193467442230669cf1f9e4618a040eaadf48dbd0de4cd8e32bfa674f90e31
// IndexVersion: 2
// --- END CODE INDEX META ---
using Amqp;
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Runtime.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime
{
    public partial class SimulatorRuntime
    {
	
        private string GetSASKey()
        {
            var sasBuilder = new Utils.SharedAccessSignatureBuilder() { Key = _simulator.UserName, KeyName = _simulator.Password, Target = _simulator.DefaultEndPoint, TimeToLive = TimeSpan.FromDays(365) };
            return sasBuilder.ToSignature();
        }

        private bool PutCbsToken(Connection connection, string host, string shareAccessSignature, string audience)
        {
            bool result = true;
            Session session = new Session(connection);

            string cbsReplyToAddress = "cbs-reply-to";
            var cbsSender = new SenderLink(session, "cbs-sender", "$cbs");
            var cbsReceiver = new ReceiverLink(session, cbsReplyToAddress, "$cbs");

            // construct the put-token message
            var request = new Amqp.Message(shareAccessSignature);
            request.Properties = new Amqp.Framing.Properties();
            request.Properties.MessageId = Guid.NewGuid().ToString();
            request.Properties.ReplyTo = cbsReplyToAddress;
            request.ApplicationProperties = new Amqp.Framing.ApplicationProperties();
            request.ApplicationProperties["operation"] = "put-token";
            request.ApplicationProperties["type"] = "azure-devices.net:sastoken";
            request.ApplicationProperties["name"] = audience;
            cbsSender.Send(request);

            // receive the response
            var response = cbsReceiver.Receive();
            if (response == null || response.Properties == null || response.ApplicationProperties == null)
            {
                result = false;
            }
            else
            {
                int statusCode = (int)response.ApplicationProperties["status-code"];
                string statusCodeDescription = (string)response.ApplicationProperties["status-description"];
                if (statusCode != 202 && statusCode != 200) // !Accepted && !OK
                {
                    result = false;
                }
            }

            // the sender/receiver may be kept open for refreshing tokens
            cbsSender.Close();
            cbsReceiver.Close();
            session.Close();

            return result;
        }

        private void StartReceiverThread(CancellationToken token)
        {
            Task.Run(async () =>
            {
                try
                {
                    var audience = $"{_simulator.DefaultEndPoint}/{_simulator.Topic}";
                    bool cbs = PutCbsToken(_amqpConnection, _simulator.DefaultEndPoint, GetSASKey(), audience);
                    var receiveLink = new ReceiverLink(_amqpSession, "receive-link", _simulator.Topic);

                    await _notificationPublisher.PublishAsync(Targets.WebSocket, Channels.Simulator, InstanceId, $"Started Listener Thread on AMQP Listener {_simulator.DefaultEndPoint} on Port {_simulator.DefaultPort} for topic {_simulator.Topic} ");

                    _listening = true;
                    while (_listening)
                    {
                        if (cbs)
                        {
                            try
                            {
                                var received = receiveLink.Receive();
                                if (received != null)
                                {
                                    receiveLink.Accept(received);
                                }
                            }
                            catch (TaskCanceledException) { }
                            catch (AmqpException) { }
                        }
                    }

                    receiveLink.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not listen on AMQP");
                }
            }, token);
        }
    }
}
