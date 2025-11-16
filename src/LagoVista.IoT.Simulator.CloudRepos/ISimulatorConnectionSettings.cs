// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 8e640f9816875d86dd1544f2440548aba1bf6a9cc8a35cc8fe4735aef6a4a14b
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.CloudRepos
{
    public interface ISimulatorConnectionSettings
    {
        IConnectionSettings SimulatorDocDbStorage { get; set; }
        IConnectionSettings SimulatorTableStorage { get; set; }

        bool ShouldConsolidateCollections { get; }
    }
}
