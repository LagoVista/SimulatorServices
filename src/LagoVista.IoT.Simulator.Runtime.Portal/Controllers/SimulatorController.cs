using LagoVista.IoT.Simulator.Runtime.Portal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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
