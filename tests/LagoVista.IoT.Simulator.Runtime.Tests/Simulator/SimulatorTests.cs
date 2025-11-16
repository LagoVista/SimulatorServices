// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 2a503713820cd1762d9ad1852b61bb1b4688128852df5c783631b94975bccd8a
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Client.Core;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Simulator.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Simulator.Runtime.Tests.Simulator
{
    [TestClass]
    public class SimulatorTests
    {
        Mock<ISimulatorRuntimeServices> _runtimeServices = new Mock<ISimulatorRuntimeServices>();
        Mock<ITCPClient> _tcpClient = new Mock<ITCPClient>();
        Mock<IUDPClient> _udpClient = new Mock<IUDPClient>();

        IMQTTDeviceClient _mqttClient;


        IAdminLogger _adminLogger = new AdminLogger(new Utils.LogWriter());
        INotificationPublisher _notifPublisher = new Utils.NotifPublisher();

        [TestInitialize]
        public void Init()
        {
    _runtimeServices.Setup<ITCPClient>(rs => rs.GetTCPClient()).Returns(_tcpClient.Object);
            _runtimeServices.Setup<IUDPClient>(rs => rs.GetUDPCLient()).Returns(_udpClient.Object);
        }

        LagoVista.IoT.Simulator.Admin.Models.SimulatorInstance GetSimulator(Admin.Models.TransportTypes transport)
        {
            var sim = new Admin.Models.Simulator()
            {
                DefaultTransport = Core.Models.EntityHeader<Admin.Models.TransportTypes>.Create(transport)
            };

            return new SimulatorInstance()
            {
                 Simulator = Core.Models.EntityHeader<Admin.Models.Simulator>.Create(sim)
            };
        }

        private void AssertSuccess(InvokeResult<string> result)
        {
            AssertSuccess(result.ToInvokeResult());
        }

        private void AssertSuccess(InvokeResult result)
        {
            if (!result.Successful)
            {
                foreach (var err in result.Errors)
                {
                    Console.WriteLine(err.Message);
                    Console.WriteLine(err.Details);
                }
            }

            Assert.IsTrue(result.Successful);
            Console.WriteLine(result.Successful);
        }

        [TestMethod]
        public async Task CreateRESTSimulator()
        {
            var sim = GetSimulator(TransportTypes.RestHttp);
            sim.Simulator.Value.Anonymous = true;

            var runtime = new SimulatorRuntime(_runtimeServices.Object, _notifPublisher, _adminLogger, sim);

            var msg = new MessageTemplate()
            {
                Transport = Core.Models.EntityHeader<TransportTypes>.Create(TransportTypes.RestHttp),
                PayloadType = Core.Models.EntityHeader<PaylodTypes>.Create(PaylodTypes.String),
                TextPayload = "abc123",
                EndPoint = "www.software-logistics.com",
                Port = 80,
                HttpVerb = "GET"
            };

            var plan = new MessageTransmissionPlan()
            {
                Message = Core.Models.EntityHeader<MessageTemplate>.Create(msg)
            };
           

            var result = await runtime.SendAsync(plan);
            AssertSuccess(result);            
        }

        [TestMethod]
        public async Task CreatAzureIoTHubSimulator()
        {
            var azIoTAccessKey = Environment.GetEnvironmentVariable("TEST_AZ_IOT_LISTENER_KEY");
            Assert.IsFalse(String.IsNullOrEmpty(azIoTAccessKey), "TEST_AZ_IOT_LISTENER_KEY");

            var azIoTAccountId = Environment.GetEnvironmentVariable("TEST_AZ_IOT_LISTENER_ACCOUNT_ID");
            Assert.IsFalse(String.IsNullOrEmpty(azIoTAccountId), "TEST_AZ_IOT_LISTENER_ACCOUNT_ID");

            var azIoTAccountPolicyName = Environment.GetEnvironmentVariable("TEST_AZ_IOT_LISTENER_POLICY_NAME");
            Assert.IsFalse(String.IsNullOrEmpty(azIoTAccountPolicyName), "TEST_AZ_IOT_LISTENER_POLICY_NAME");

            var ehCompatName = Environment.GetEnvironmentVariable("TEST_AZ_IOT_LISTENER_EH_COMPAT_NAME");
            Assert.IsFalse(String.IsNullOrEmpty(ehCompatName), "TEST_AZ_IOT_LISTENER_EH_COMPAT_NAME");

            var ehCompatEndPoint = Environment.GetEnvironmentVariable("TEST_AZ_IOT_LISTENER_EH_COMPAT_ENDPOINT");
            Assert.IsFalse(String.IsNullOrEmpty(ehCompatEndPoint), "TEST_AZ_IOT_LISTENER_EH_COMPAT_ENDPOINT");

            var azDeviceId = Environment.GetEnvironmentVariable("TEST_AZ_IOT_LISTENER_DEVICEID");
            Assert.IsFalse(String.IsNullOrEmpty(azDeviceId), "Missing TEST_AZ_IOT_LISTENER_DEVICEID");

            var sim = GetSimulator(TransportTypes.AzureIoTHub);
            sim.Simulator.Value.AccessKey = azIoTAccessKey;
            sim.Simulator.Value.AccessKeyName = azIoTAccountPolicyName;
            
            var msg = new MessageTemplate()
            {
                Transport = Core.Models.EntityHeader<TransportTypes>.Create(TransportTypes.RestHttp),
                PayloadType = Core.Models.EntityHeader<PaylodTypes>.Create(PaylodTypes.String),
                TextPayload = "abc123",
                EndPoint = "www.software-logistics.com",                
                Port = 80,
                HttpVerb = "GET"
            };


            var plan = new MessageTransmissionPlan()
            {
                Message = Core.Models.EntityHeader<MessageTemplate>.Create(msg)
            };

            var runtime = new SimulatorRuntime(_runtimeServices.Object, _notifPublisher, _adminLogger, sim);

            var result = await runtime.SendAsync(plan);
            AssertSuccess(result);
        }

        [TestMethod]
        public async Task CreateEHSimulator()
        {
            var ehAccountId = Environment.GetEnvironmentVariable("TEST_EH_LISTENER_ACCOUNT_ID");
            Assert.IsFalse(String.IsNullOrEmpty(ehAccountId), "Missing TEST_EH_LISTENER_ACCOUNT_ID");

            var ehPolicyName = Environment.GetEnvironmentVariable("TEST_EH_LISTENER_POLICY_NAME");
            Assert.IsFalse(String.IsNullOrEmpty(ehPolicyName), "Missing TEST_EH_LISTENER_POLICY_NAME");

            var ehAccessKey = Environment.GetEnvironmentVariable("TEST_EH_LISTENER_KEY");
            Assert.IsFalse(String.IsNullOrEmpty(ehAccessKey), "Missing TEST_EH_LISTENER_KEY");

            var sim = GetSimulator(TransportTypes.AzureEventHub);
            

            var msg = new MessageTemplate()
            {
                Transport = Core.Models.EntityHeader<TransportTypes>.Create(TransportTypes.RestHttp),
                PayloadType = Core.Models.EntityHeader<PaylodTypes>.Create(PaylodTypes.String),
                TextPayload = "abc123",
                EndPoint = "www.software-logistics.com",
                Port = 80,
                HttpVerb = "GET"
            };

            var runtime = new SimulatorRuntime(_runtimeServices.Object, _notifPublisher, _adminLogger, sim);

            var plan = new MessageTransmissionPlan()
            {
                Message = Core.Models.EntityHeader<MessageTemplate>.Create(msg)
            };

            var result = await runtime.SendAsync(plan);
            AssertSuccess(result);
        }

        [TestMethod]
        public async Task CreateSBSimulator()
        {
            var sbTrasnamitterKey = Environment.GetEnvironmentVariable("TEST_SB_ACCESSKEY");
            Assert.IsFalse(String.IsNullOrEmpty(sbTrasnamitterKey));

            var accountId = System.Environment.GetEnvironmentVariable("TEST_SB_ACCOUNT_ID");
            Assert.IsFalse(String.IsNullOrEmpty(accountId), "TEST_SB_ACCOUNT_ID");

            var sbPolicyName = System.Environment.GetEnvironmentVariable("TEST_SB_POLICY_NAME");
            Assert.IsFalse(String.IsNullOrEmpty(sbPolicyName), "TEST_SB_POLICY_NAME");

            var sim = GetSimulator(TransportTypes.AzureServiceBus);

            var msg = new MessageTemplate()
            {
                Transport = Core.Models.EntityHeader<TransportTypes>.Create(TransportTypes.RestHttp),
                PayloadType = Core.Models.EntityHeader<PaylodTypes>.Create(PaylodTypes.String),
                TextPayload = "abc123",
                EndPoint = "www.software-logistics.com",
                Port = 80,
                HttpVerb = "GET"
            };


            var plan = new MessageTransmissionPlan()
            {
                Message = Core.Models.EntityHeader<MessageTemplate>.Create(msg)
            };

            var runtime = new SimulatorRuntime(_runtimeServices.Object, _notifPublisher, _adminLogger, sim);

            var result = await runtime.SendAsync(plan);
            AssertSuccess(result);
        }

        [TestMethod]
        public async Task CreateMQTTSimulator()
        {
            var testHostName = Environment.GetEnvironmentVariable("TEST_MQTT_HOST_NAME");
            Assert.IsFalse(String.IsNullOrEmpty(testHostName), "Missing environment variable TEST_MQTT_HOST_NAME");

            var testUserName = Environment.GetEnvironmentVariable("TEST_MQTT_USER_NAME");
            Assert.IsFalse(String.IsNullOrEmpty(testUserName), "Missing environment variable TEST_MQTT_USER_NAME");

            var testPwd = Environment.GetEnvironmentVariable("TEST_MQTT_PASSWORD");
            Assert.IsFalse(String.IsNullOrEmpty(testPwd), "Missing environment variable TEST_MQTT_PASSWORD");

            var sim = GetSimulator(TransportTypes.MQTT);
            sim.Simulator.Value.DefaultEndPoint = testHostName;
            sim.Simulator.Value.DefaultPort = 1883;
            sim.Simulator.Value.UserName = testUserName;
            sim.Simulator.Value.Password = testPwd;

            var msg = new MessageTemplate()
            {
                PayloadType = Core.Models.EntityHeader<PaylodTypes>.Create(PaylodTypes.String),
                Topic = "sendit",
                TextPayload = "abc123",
            };

            var runtime = new SimulatorRuntime(_runtimeServices.Object, _notifPublisher, _adminLogger, sim);

            var connectResult = await runtime.ConnectAsync();
            AssertSuccess(connectResult);

            var plan = new MessageTransmissionPlan()
            {
                Message = Core.Models.EntityHeader<MessageTemplate>.Create(msg)
            };

            var result = await runtime.SendAsync(plan);
            AssertSuccess(result);

            var disconnectResult = await runtime.DisconnectAsync();
            AssertSuccess(disconnectResult);
            
        }

    }
}
