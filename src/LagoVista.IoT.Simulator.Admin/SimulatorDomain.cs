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
