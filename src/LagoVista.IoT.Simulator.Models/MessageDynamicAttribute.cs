using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Models.Resources;

namespace LagoVista.IoT.Simulator.Admin.Models
{    
    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageDynamicAttribute_Title, SimulatorResources.Names.MessageDynamicAttribute_Help, SimulatorResources.Names.MessageDynamicAttribute_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]
    public class MessageDynamicAttribute : IIDEntity, INamedEntity, IKeyedEntity, IEntityHeaderEntity, IValidateable, IFormDescriptor
    {
        public MessageDynamicAttribute()
        {
            Id = Guid.NewGuid().ToId();
        }

        public String Id { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }
        
        [FormField(LabelResource: SimulatorResources.Names.MessageDynamicAttribute_ParameterType, FieldType: FieldTypes.Picker, EnumType: typeof(ParameterTypes), ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public EntityHeader<ParameterTypes> ParameterType { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Key, HelpResource: SimulatorResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageDynamicAttribute_DefaultValue, FieldType: FieldTypes.Text, RegExValidationMessageResource: SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string DefaultValue { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string Description { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(ParameterType),
                nameof(DefaultValue),
                nameof(Description)
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
}
