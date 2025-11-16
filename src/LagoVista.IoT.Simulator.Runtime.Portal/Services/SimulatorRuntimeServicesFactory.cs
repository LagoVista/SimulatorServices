// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 798b283f7610083e691a558fa3c6d1846e9662ba74bfe31bb0bf8afd1bdd4580
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Runtime.Portal.Services
{
    public class SimulatorRuntimeServicesFactory : ISimulatorRuntimeServicesFactory
    {
        public ISimulatorRuntimeServices GetServices()
        {
            return new SimulatorRuntimeServices();
        }
    }
}
