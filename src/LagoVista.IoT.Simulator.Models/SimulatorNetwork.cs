// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 1c2f0389a4e205aee3b51f946c520d1657e25556715a78874e050e91d9212c50
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Models.Resources;
using Newtonsoft.Json;
using System;
using LagoVista.Core;
using System.Collections.Generic;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.SimulatorNetwork_Title, SimulatorResources.Names.SimulatorNetwork_Help, SimulatorResources.Names.SimulatorNetwork_Description,
        EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources), Icon:"icon-pz-speed-1",
        SaveUrl: "/api/simulator/network", GetUrl: "/api/simulator/network/{id}", DeleteUrl: "/api/simulator/network/{id}", FactoryUrl: "/api/simulator/network/factory", GetListUrl: "/api/simulator/networks")]
    public class SimulatorNetwork : EntityBase, IValidateable, IFormDescriptor, IDescriptionEntity, IIconEntity, ISummaryFactory
    {
        public SimulatorNetwork()
        {
            Id = Guid.NewGuid().ToId();
            Simulators = new List<SimulatorInstance>(); 
            Icon = "icon-pz-speed-1z";
        }


        [FormField(LabelResource: SimulatorResources.Names.Common_Icon, FieldType: FieldTypes.Icon, ResourceType: typeof(SimulatorResources), IsRequired: false, IsUserEditable: true)]
        public string Icon { get; set; }


        [FormField(LabelResource: SimulatorResources.Names.SimulatorNetwork_SimulatorInstances, FieldType: FieldTypes.ChildListInline, FactoryUrl: "/api/simulator/instance/factory", ResourceType: typeof(SimulatorResources))]
        public List<SimulatorInstance> Simulators { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.SimulatorNetwork_AccessKey1, HelpResource: SimulatorResources.Names.SimulatorNetwork_AccessKey_Help, FieldType: FieldTypes.Secret,
            SecureIdFieldName: nameof(SharedAccessKey1SecretId), ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string SharedAccessKey1 { get; set; }

        public string SharedAccessKey1SecretId { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.SimulatorNetwork_AccessKey2, HelpResource: SimulatorResources.Names.SimulatorNetwork_AccessKey_Help, FieldType: FieldTypes.Secret,
            SecureIdFieldName: nameof(SharedAccessKey2SecretId), ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string SharedAccessKey2 { get; set; }

        public string SharedAccessKey2SecretId { get; set; }
        public SimulatorNetworkSummary CreateSummary()
        {
            return new SimulatorNetworkSummary()
            {
                Id = Id,
                Description = Description,
                IsPublic = IsPublic,
                Key = Key,
                Icon = Icon,
                Name = Name
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Icon),
                nameof(Description),
                nameof(SharedAccessKey1),
                nameof(SharedAccessKey2),
                nameof(Simulators),
              };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary();
        }
    }

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.SimulatorNetworks_Title, SimulatorResources.Names.SimulatorNetwork_Help, SimulatorResources.Names.SimulatorNetwork_Description,
    EntityDescriptionAttribute.EntityTypes.Summary, typeof(SimulatorResources), Icon: "icon-pz-speed-1",
    SaveUrl: "/api/simulator/network", GetUrl: "/api/simulator/network/{id}", DeleteUrl: "/api/simulator/network/{id}", FactoryUrl: "/api/simulator/network/factory", GetListUrl: "/api/simulator/networks")]
    public class SimulatorNetworkSummary : SummaryData
    {

    }
}
