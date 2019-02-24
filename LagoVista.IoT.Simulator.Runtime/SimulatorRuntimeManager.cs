using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using LagoVista.Core;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Digests;
using Newtonsoft.Json;
using LagoVista.IoT.Simulator.Admin.Models;
using System.Collections.ObjectModel;

namespace LagoVista.IoT.Simulator.Runtime
{
    public class SimulatorRuntimeManager
    {
        public const string REQUEST_ID = "x-nuviot-runtime-request-id";
        public const string ORG_ID = "x-nuviot-orgid";
        public const string ORG = "x-nuviot-org";
        public const string USER_ID = "x-nuviot-userid";
        public const string USER = "x-nuviot-user";
        public const string NETWORK_ID = "x-nuviot-sim-network-id";
        public const string DATE = "x-nuviot-date";
        public const string VERSION = "x-nuviot-version";

        ISimulatorRuntimeServicesFactory _factory;
        IAdminLogger _adminLogger;

        public ObservableCollection<SimulatorRuntime> Runtimes { get; }

        public SimulatorRuntimeManager(ISimulatorRuntimeServicesFactory factory, IAdminLogger adminLogger)
        {
            _adminLogger = adminLogger;
            _factory = factory;

            Runtimes = new ObservableCollection<SimulatorRuntime>();
        }

        private string GetSignature(string requestId, string key, string source)
        {
            var encData = Encoding.UTF8.GetBytes(source);

            var hmac = new HMac(new Sha256Digest());

            hmac.Init(new KeyParameter(Encoding.UTF8.GetBytes(key)));

            var resultBytes = new byte[hmac.GetMacSize()];
            hmac.BlockUpdate(encData, 0, encData.Length);
            hmac.DoFinal(resultBytes, 0);

            var b64Str = System.Convert.ToBase64String(resultBytes);
            return $"{requestId}:{b64Str}";
        }

        public INotificationPublisher Publisher { get; set; }

        public async Task InitAsync(string simulatorNetworkId, string key, EntityHeader org, EntityHeader user, Environments environment)
        {            
            var rootUri = "https://api.nuviot.com/api/simulator/network/runtime";

            switch(environment)
                {
                case Environments.Development:
                    rootUri = "https://dev.nuviot.com/api/simulator/network/runtime";
                    break;
                case Environments.Testing:
                    rootUri = "https://test.nuviot.com/api/simulator/network/runtime";
                    break;
                case Environments.LocalDevelopment:
                    rootUri = "http://localhost:5001/api/simulator/network/runtime";
                    break;
                default:
                    break;
            }

            var requestId = Guid.NewGuid().ToId();
            var dateStamp = DateTime.UtcNow.ToJSONString();
            var version = "1.0.0";

            var bldr = new StringBuilder();
            //Adding the \r\n manualy ensures that the we don't have any 
            //platform specific code messing with our signature.
            bldr.Append($"{requestId}\r\n");
            bldr.Append($"{dateStamp}\r\n");
            bldr.Append($"{version}\r\n");
            bldr.Append($"{org.Id}\r\n");
            bldr.Append($"{user.Id}\r\n");
            bldr.Append($"{simulatorNetworkId}\r\n");

            var sasKey = GetSignature(requestId, key, bldr.ToString());

            _adminLogger.AddCustomEvent(Core.PlatformSupport.LogLevel.Message, "SimulatorRuntimeManager_InitAsync", $"Requesting configuration from: {rootUri} ");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("SAS", sasKey);
            client.DefaultRequestHeaders.Add(REQUEST_ID, requestId);
            client.DefaultRequestHeaders.Add(ORG_ID, org.Id);
            client.DefaultRequestHeaders.Add(ORG, org.Text);
            client.DefaultRequestHeaders.Add(USER_ID, user.Id);
            client.DefaultRequestHeaders.Add(USER, user.Text);
            client.DefaultRequestHeaders.Add(NETWORK_ID, simulatorNetworkId);
            client.DefaultRequestHeaders.Add(DATE, dateStamp);
            client.DefaultRequestHeaders.Add(VERSION, version);

            var json = await client.GetStringAsync(rootUri);

  
            var network = JsonConvert.DeserializeObject<SimulatorNetwork>(json);
            foreach(var sim in network.Simulators)
            {
                var services = _factory.GetServices();
                var runtime = new SimulatorRuntime(services, Publisher, _adminLogger, sim);
                await runtime.ConnectAsync();
                runtime.Start();
                Runtimes.Add(runtime);
            }
        }
    }
}
