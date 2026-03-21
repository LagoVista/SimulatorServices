// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 8e640f9816875d86dd1544f2440548aba1bf6a9cc8a35cc8fe4735aef6a4a14b
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LagoVista.IoT.Simulator.CloudRepos
{
    public class SimulatorConnectionSettings : ISimulatorConnectionSettings
    {
        public IConnectionSettings SimulatorDocDbStorage { get; }
        public IConnectionSettings SimulatorTableStorage { get; }

        public SimulatorConnectionSettings(IConfiguration configuration)
        {
            SimulatorDocDbStorage = configuration.CreateDefaultDBStorageSettings();
            SimulatorTableStorage = configuration.CreateDefaultTableStorageSettings();
        }   
    }
}
