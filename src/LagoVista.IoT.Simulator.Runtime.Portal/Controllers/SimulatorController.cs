using Microsoft.AspNetCore.Mvc;
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

        public SimulatorController(SimulatorRuntimeManager simulatorRuntimeManager)
        {
            _simulatorRuntimeManager = simulatorRuntimeManager;
        }

        [Route("/api/simnetwork/simulators")]
        public ObservableCollection<SimulatorRuntime> GetSimulators()
        {
            return _simulatorRuntimeManager.Runtimes;
        }
    }
}
