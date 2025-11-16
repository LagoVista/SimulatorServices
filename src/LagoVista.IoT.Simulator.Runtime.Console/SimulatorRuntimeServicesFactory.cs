// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 72ae6485855400f7cc553a6e68ce6de86055b8ac96ff04738323e2216804ff18
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Runtime
{
    public class SimulatorRuntimeServicesFactory : ISimulatorRuntimeServicesFactory
    {
        public ISimulatorRuntimeServices GetServices()
        {
            return new SimulatorRuntimeServices();
        }
    }
}
