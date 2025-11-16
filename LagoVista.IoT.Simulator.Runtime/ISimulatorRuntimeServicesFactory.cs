// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: b36a859f8e887d424d97ddeaf9124a3354392e505967725e5e69660f701f71c6
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Runtime
{
    public interface ISimulatorRuntimeServicesFactory
    {
        ISimulatorRuntimeServices GetServices();
    }
}
