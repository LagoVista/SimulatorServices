// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 02604f337a1b726dfd540e147c8e761c3b01ff5e979aae1998de3aff1e1068ec
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Simulator.Runtime.Portal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.ObjectModel;

namespace LagoVista.IoT.Simulator.Runtime.Portal.Controllers
{

    [Route("/api/simnetwork")]
    public class SimulatorController : Controller
    {
        SimulatorRuntimeManager _simulatorRuntimeManager;

        IHubContext<NotificationHub> _hub;

        public SimulatorController(IHubContext<NotificationHub> hub, SimulatorRuntimeManager simulatorRuntimeManager)
        {
            _hub = hub;
            _simulatorRuntimeManager = simulatorRuntimeManager;
        }

        [Route("/api/simnetwork/simulators")]
        public ObservableCollection<SimulatorRuntime> GetSimulators()
        {
            return _simulatorRuntimeManager.Runtimes;
        }
    }
}
