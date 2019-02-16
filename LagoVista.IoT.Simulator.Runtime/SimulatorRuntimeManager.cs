using LagoVista.IoT.Runtime.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Runtime
{
    public class SimulatorRuntimeManager
    {
        INotificationPublisher _notificationPublisher;

        public List<SimulatorRuntime> _runtimes = new List<SimulatorRuntime>();
 
        public SimulatorRuntimeManager(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }


    }
}
