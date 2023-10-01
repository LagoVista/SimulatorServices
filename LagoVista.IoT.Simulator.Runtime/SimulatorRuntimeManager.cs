using LagoVista.Core;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Simulator.Admin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        String _simulatorNetworkId;
        EntityHeader _org;
        EntityHeader _user;
        Environments _environment;
        String _simAccessKey;

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

        public async Task InitAsync(string simulatorNetworkId, string simAccessKey, EntityHeader org, EntityHeader user, Environments environment)
        {
            _environment = environment;
            _simulatorNetworkId = simulatorNetworkId;
            _org = org;
            _user = user;
            _simAccessKey = simAccessKey;

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var rootUri = "https://api.nuviot.com/api/simulator/network/runtime";

            switch (_environment)
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
            bldr.Append($"{_org.Id}\r\n");
            bldr.Append($"{_user.Id}\r\n");
            bldr.Append($"{_simulatorNetworkId}\r\n");

            var sasKey = GetSignature(requestId, _simAccessKey, bldr.ToString());

            Console.WriteLine("------------------------");
            Console.WriteLine(">" + _simAccessKey + "<");
            Console.WriteLine("------------------------");
            Console.WriteLine(bldr.ToString());
            Console.WriteLine("------------------------");
            Console.WriteLine($"Length: {bldr.ToString().Length}");
            Console.WriteLine("------------------------");
            Console.WriteLine(">" + sasKey + "<");
            Console.WriteLine("------------------------");


            _adminLogger.AddCustomEvent(Core.PlatformSupport.LogLevel.Message, "SimulatorRuntimeManager_InitAsync", $"Requesting configuration from: {rootUri} ");
            Console.WriteLine($"Requesting configuration from: {rootUri}");


            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("SAS", sasKey);
            client.DefaultRequestHeaders.Add(REQUEST_ID, requestId);
            client.DefaultRequestHeaders.Add(ORG_ID, _org.Id);
            client.DefaultRequestHeaders.Add(ORG, _org.Text);
            client.DefaultRequestHeaders.Add(USER_ID, _user.Id);
            client.DefaultRequestHeaders.Add(USER, _user.Text);
            client.DefaultRequestHeaders.Add(NETWORK_ID, _simulatorNetworkId);
            client.DefaultRequestHeaders.Add(DATE, dateStamp);
            client.DefaultRequestHeaders.Add(VERSION, version);

            try
            {
                var json = await client.GetStringAsync(rootUri);

                Runtimes.Clear();

                var network = JsonConvert.DeserializeObject<SimulatorNetwork>(json);
                Console.WriteLine($"Loaded simulator network {network.Name}");

                foreach (var sim in network.Simulators)
                {
                    var services = _factory.GetServices();
                    var runtime = new SimulatorRuntime(services, Publisher, _adminLogger, sim);
                    await runtime.StartAsync();
                    Runtimes.Add(runtime);
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error loading runtime.");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        public async Task StartAsync()
        {
            foreach (var rt in Runtimes)
            {
                await rt.StartAsync();
            }
        }

        public async Task StopAsync()
        {
            foreach (var rt in Runtimes)
            {
                await rt.StopAsync();
            }
        }

        public async Task ReloadAsync()
        {
            await StopAsync();
            await LoadAsync();
            await StartAsync();

            await Publisher.PublishAsync(Targets.WebSocket,
                new Notification()
                {
                    Channel = EntityHeader<Channels>.Create(Channels.Simulator),
                    ChannelId = _simulatorNetworkId,
                    PayloadType = "Simulators",
                    Payload = JsonConvert.SerializeObject(Runtimes, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                    DateStamp = DateTime.UtcNow.ToJSONString(),
                    MessageId = Guid.NewGuid().ToId(),
                    Verbosity = EntityHeader<NotificationVerbosity>.Create(NotificationVerbosity.Normal)
                });
        }
    }
}
