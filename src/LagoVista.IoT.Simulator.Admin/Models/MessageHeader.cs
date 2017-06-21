using LagoVista.Core.Attributes;
using LagoVista.IoT.Simulator.Admin.Resources;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;

namespace LagoVista.IoT.Simulator.Admin.Models
{

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageHeader_Title, SimulatorResources.Names.MessageHeader_Help,SimulatorResources.Names.MessageHeader_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]
    public class MessageHeader : IIDEntity, INamedEntity, IKeyedEntity, IEntityHeaderEntity
    {
        public MessageHeader()
        {
            Id = Guid.NewGuid().ToId();
        }

        public String Id { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }


        [FormField(LabelResource: Resources.SimulatorResources.Names.MessageHeader_Key, HelpResource:Resources.SimulatorResources.Names.MessageHeader_Key_Help,  FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired:true)]
        public string Key { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.MessageHeader_HeaderName, HelpResource: Resources.SimulatorResources.Names.MessageHeader_HeaderName_Help, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string HeaderName { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.MessageHeader_Value, HelpResource:Resources.SimulatorResources.Names.MessageHeader_Value_Help, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources), IsRequired:true)]
        public string Value { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string Description { get; set; }

        public IEntityHeader ToEntityHeader()
        {
            return new EntityHeader()
            {
                Id = Id,
                Text = Name,
            };
        }
    }
}
