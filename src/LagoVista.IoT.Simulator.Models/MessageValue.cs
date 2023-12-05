using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Models.Resources;
using System.Collections.Generic;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageValue_Title, SimulatorResources.Names.MessageValue_Help, SimulatorResources.Names.MessageValue_Description,
    EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]
    public class MessageValue : IEntityHeaderEntity, IValidateable, IFormDescriptor
    {
        [FormField(LabelResource: SimulatorResources.Names.MessageValue_Attribute, FieldType: FieldTypes.EntityHeaderPicker, WaterMark:SimulatorResources.Names.MessageValue_SelectAttribute, ResourceType: typeof(SimulatorResources))]
        public EntityHeader<MessageDynamicAttribute> Attribute { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageValue_Value, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Value { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                 nameof(Attribute),
                 nameof(Value)
            };
        }

        public EntityHeader ToEntityHeader()
        {
            return Attribute;
        }
    }
}
