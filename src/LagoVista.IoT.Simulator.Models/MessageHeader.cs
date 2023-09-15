using LagoVista.Core.Attributes;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Models.Resources;

namespace LagoVista.IoT.Simulator.Admin.Models
{

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageHeader_Title, SimulatorResources.Names.MessageHeader_Help,SimulatorResources.Names.MessageHeader_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]
    public class MessageHeader : IIDEntity, INamedEntity, IKeyedEntity, IEntityHeaderEntity, IValidateable, IFormDescriptor
    {
        public MessageHeader()
        {
            Id = Guid.NewGuid().ToId();
        }

        public String Id { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageHeader_Key, HelpResource:SimulatorResources.Names.MessageHeader_Key_Help,  FieldType: FieldTypes.Key, RegExValidationMessageResource: SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired:true)]
        public string Key { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageHeader_HeaderName, HelpResource: SimulatorResources.Names.MessageHeader_HeaderName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string HeaderName { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageHeader_Value, HelpResource:SimulatorResources.Names.MessageHeader_Value_Help, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources), IsRequired:true)]
        public string Value { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string Description { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(HeaderName),
                nameof(Value),
                nameof(Description)
            };
        }

        public IEntityHeader ToEntityHeader()
        {
            return new EntityHeader()
            {
                Id = Id,
                Text = Name,
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult result)
        {
            if (String.IsNullOrEmpty(HeaderName)) result.AddSystemError("Header Name is a Required Field.");
            if (String.IsNullOrEmpty(Value)) result.AddSystemError("Header Name is a Required Field.");
        }
    }
}
