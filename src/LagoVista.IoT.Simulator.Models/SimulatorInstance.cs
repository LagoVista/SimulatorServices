using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    public class SimulatorInstance
    {
        public SimulatorInstance()
        {
            TransmissionPlan = new List<MessageTransmissionPlan>();
        }

        public EntityHeader<Simulator> Simulator { get; set; }

        public string DeviceId { get; set; }

        public List<MessageTransmissionPlan> TransmissionPlan { get; set; }  
    }

    public class MessageTransmissionPlan
    {
        public int PeriodMS { get; set; }

        public EntityHeader<MessageTemplate> Message { get; set; }

        public List<DynamicValues> Values { get; set; }
    }

    public class DynamicValues
    {
        public EntityHeader<MessageDynamicAttribute> Attribute { get; set; }
        public string Value { get; set; }
    }
}
