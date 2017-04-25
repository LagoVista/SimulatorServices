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
