// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: ecf8a8a3d7d1d793691470753dc7726a1767ca1f1b98c678d78c071ff3644315
// IndexVersion: 2
// --- END CODE INDEX META ---
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
