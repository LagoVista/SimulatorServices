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
        EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]
    public class SimulatorNetwork : ModelBase, IKeyedEntity, IIDEntity, INamedEntity, IOwnedEntity, IAuditableEntity, IValidateable, INoSQLEntity, IEntityHeaderEntity, IFormDescriptor
    {
        public SimulatorNetwork()
        {
            Id = Guid.NewGuid().ToId();
            Simulators = new List<SimulatorInstance>();
        }

        public string DatabaseName { get; set; }
        public string EntityType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }


        [FormField(LabelResource: SimulatorResources.Names.Common_Key, HelpResource: SimulatorResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string Description { get; set; }


        [FormField(LabelResource: SimulatorResources.Names.Common_IsPublic, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources))]
        public bool IsPublic { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.SimulatorNetwork_SimulatorInstances, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<SimulatorInstance> Simulators { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.SimulatorNetwork_AccessKey1, HelpResource: SimulatorResources.Names.SimulatorNetwork_AccessKey_Help, FieldType: FieldTypes.Secret,
            SecureIdFieldName:nameof(SharedAccessKey1SecretId), ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string SharedAccessKey1 { get; set; }

        public string SharedAccessKey1SecretId { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.SimulatorNetwork_AccessKey2, HelpResource: SimulatorResources.Names.SimulatorNetwork_AccessKey_Help, FieldType: FieldTypes.Secret,
            SecureIdFieldName: nameof(SharedAccessKey2SecretId), ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string SharedAccessKey2 { get; set; }

        public string SharedAccessKey2SecretId { get; set; }

        public string CreationDate { get; set; }
        public string LastUpdatedDate { get; set; }
        public EntityHeader CreatedBy { get; set; }
        public EntityHeader LastUpdatedBy { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }


        public SimulatorNetworkSummary CreateSummary()
        {
            return new SimulatorNetworkSummary()
            {
                Id = Id,
                Description = Description,
                IsPublic = IsPublic,
                Key = Key,
                Name = Name
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Description),
                nameof(SharedAccessKey1),
                nameof(SharedAccessKey2),
                nameof(Simulators),
              };
        }

        public IEntityHeader ToEntityHeader()
        {
            return new EntityHeader()
            {
                Id = Id,
                Text = Name
            };
        }
    }

    public class SimulatorNetworkSummary : SummaryData
    {

    }
}
