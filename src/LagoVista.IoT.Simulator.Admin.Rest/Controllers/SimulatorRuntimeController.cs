using LagoVista.Core.Exceptions;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Simulator.Admin.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.IoT.Simulator.Admin.Models;

namespace LagoVista.IoT.Simulator.Admin.Rest.Controllers
{
    public class SimulatorRuntimeController : Controller
    {
        public const string REQUEST_ID = "x-nuviot-runtime-request-id";
        public const string ORG_ID = "x-nuviot-orgid";
        public const string ORG = "x-nuviot-org";
        public const string USER_ID = "x-nuviot-userid";
        public const string USER = "x-nuviot-user";
        public const string NETWORK_ID = "x-nuviot-sim-network-id";
        public const string DATE = "x-nuviot-date";
        public const string VERSION = "x-nuviot-version";

        ISimulatorNetworkManager _simNetworkManager;
        ISimulatorManager _simManager;
        IAdminLogger _logger;
        ISecureStorage _secureStorage;

        public SimulatorRuntimeController(ISimulatorNetworkManager simNetworkManager, ISimulatorManager simManager, IAdminLogger logger, ISecureStorage secureStorage)
        {
            _simNetworkManager = simNetworkManager;
            _simManager = simManager;
            _secureStorage = secureStorage;
            _logger = logger;
        }

        private void CheckHeader(HttpRequest request, String header)
        {
            if (!request.Headers.Keys.Contains(header) || String.IsNullOrEmpty(request.Headers[header].ToString()))
            {
                throw new NotAuthorizedException($"Missing request id header: {header}");
            }
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
            return $"SAS {requestId}:{b64Str}";
        }


        protected EntityHeader OrgEntityHeader { get; private set; }
        protected EntityHeader UserEntityHeader { get; private set; }
        

        [HttpGet("/api/simulator/network/runtime")]
        public async Task<SimulatorNetwork> ValidateRequest()
        {
            var request = HttpContext.Request;

            CheckHeader(request, REQUEST_ID);
            CheckHeader(request, ORG_ID);
            CheckHeader(request, ORG);
            CheckHeader(request, NETWORK_ID);
            CheckHeader(request, DATE);
            CheckHeader(request, VERSION);

            var authheader = request.Headers["Authorization"];

            var requestId = request.Headers[REQUEST_ID];
            var dateStamp = request.Headers[DATE];
            var orgId = request.Headers[ORG_ID];
            var org = request.Headers[ORG];
            var networkId = request.Headers[NETWORK_ID];
            var userId = request.Headers[USER_ID];
            var userName = request.Headers[USER];

            var version = request.Headers[VERSION];

            var bldr = new StringBuilder();
            //Adding the \r\n manualy ensures that the we don't have any 
            //platform specific code messing with our signature.
            bldr.Append($"{requestId}\r\n");
            bldr.Append($"{dateStamp}\r\n");
            bldr.Append($"{version}\r\n");
            bldr.Append($"{orgId}\r\n");
            bldr.Append($"{userId}\r\n");
            bldr.Append($"{networkId}\r\n");

            OrgEntityHeader = EntityHeader.Create(orgId, org);
            UserEntityHeader = EntityHeader.Create(userId, userName);

            var network = await _simNetworkManager.GetSimulatorNetworkAsync(networkId, OrgEntityHeader, UserEntityHeader);
            if(network == null)
            {
                throw new Exception("Could not find simulator for network id [networkId]");
            }

            var key1 = await _secureStorage.GetSecretAsync(OrgEntityHeader, network.SharedAccessKey1SecretId, UserEntityHeader);
            if (!key1.Successful)
            {
                throw new Exception(key1.Errors.First().Message);
            }

            var calculatedFromFirst = GetSignature(requestId, key1.Result, bldr.ToString());

            if (calculatedFromFirst != authheader)
            {
                var key2 = await _secureStorage.GetSecretAsync(OrgEntityHeader, network.SharedAccessKey2, UserEntityHeader);
                if (!key2.Successful)
                {
                    throw new Exception(key2.Errors.First().Message);
                }

                var calculatedFromSecond = GetSignature(requestId, key2.Result, bldr.ToString());
                if (calculatedFromSecond != authheader)
                {
                    throw new UnauthorizedAccessException("Invalid signature.");
                }
            }

            foreach(var simulator in network.Simulators)
            {
                simulator.Simulator.Value = await _simManager.GetSimulatorAsync(simulator.Simulator.Id, OrgEntityHeader, UserEntityHeader, true);
            }

            return network;
        }
    }
}
