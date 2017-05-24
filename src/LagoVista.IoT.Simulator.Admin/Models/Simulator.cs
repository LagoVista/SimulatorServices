using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.Simulator.Admin.Resources;
using System.Collections.Generic;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.Simulator_Title, SimulatorResources.Names.Simulator_Description, SimulatorResources.Names.Simulator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]

    public class Simulator : ModelBase,  IKeyedEntity, IIDEntity, INamedEntity, IOwnedEntity, IAuditableEntity, IValidateable, INoSQLEntity
    {
        public Simulator()
        {
            MessageTemplates = new List<MessageTemplate>();
        }

        public string DatabaseName { get; set; }
        public string EntityType { get; set; }
        public string Id { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }
  

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Key, HelpResource: Resources.SimulatorResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_IsPublic, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources))]
        public bool IsPublic { get; set; }


        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_Deployment_Config, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(SimulatorResources))]
        public EntityHeader DeploymentConfiguration { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_Device_Config, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(SimulatorResources))]
        public EntityHeader DeviceConfiguration { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_DeviceType, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(SimulatorResources))]
        public EntityHeader DeviceType { get; set; }


        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_PipelineModule_Config, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(SimulatorResources))]
        public EntityHeader PipelineModuleConfiguration { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string Description { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_MessageTemplates, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<MessageTemplate> MessageTemplates { get; set; }


        public string CreationDate { get; set; }
        public string LastUpdatedDate { get; set; }
        public EntityHeader CreatedBy { get; set; }
        public EntityHeader LastUpdatedBy { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }

        public SimulatorSummary CreateSummary()
        {
            return new SimulatorSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                Description = Description,
                DeviceConfiguration = DeviceConfiguration.Text,
                DeviceType = DeviceType.Text                
            };
        }
    }

    public class SimulatorSummary : SummaryData
    {

        public string DeviceConfiguration { get; set; }

        public string DeviceType { get; set; }
    }
}
