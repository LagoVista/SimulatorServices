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
