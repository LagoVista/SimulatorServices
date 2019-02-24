﻿using System;
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
