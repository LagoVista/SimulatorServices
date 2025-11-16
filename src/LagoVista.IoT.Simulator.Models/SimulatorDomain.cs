// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: fc6fac87a2afa3310a4411228fa86f58bbbf3fcff15acf21a2d1d06597c3fde8
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Attributes;
using LagoVista.Core.Models.UIMetaData;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Admin
{
    public class SimulatorDomain
    {
        public const string SimulatorAdmin = "Simulator Modules";

        [DomainDescription(SimulatorAdmin)]
        public static DomainDescription PipelineModules
        {
            get
            {
                return new DomainDescription()
                {
                    Description = "A set of Models to manage Simulators.",
                    DomainType = DomainDescription.DomainTypes.BusinessObject,
                    Name = "Simulator Admin",
                    CurrentVersion = new Core.Models.VersionInfo()
                    {
                        Major = 0,
                        Minor = 8,
                        Build = 001,
                        DateStamp = new DateTime(2017, 3, 31),
                        Revision = 1,
                        ReleaseNotes = "Initial unstable preview"
                    }
                };
            }
        }

    }
}
