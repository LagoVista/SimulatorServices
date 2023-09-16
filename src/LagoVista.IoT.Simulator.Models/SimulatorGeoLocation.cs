using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    /* Point to be used to send geo locations to an instance.*/
    public class SimulatorGeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public double Seconds { get; set; }
    }
}
