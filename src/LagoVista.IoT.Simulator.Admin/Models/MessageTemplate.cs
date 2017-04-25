using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.IoT.Simulator.Admin.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageTemplate_Title, SimulatorResources.Names.MessageTemplate_Help, SimulatorResources.Names.MessageTemplate_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]

    public class MessageTemplate
    {
        public MessageTemplate()
        {
            MessageHeaders = new List<MessageHeader>();
            Tokens = new List<string>();
        }


        public enum PaylodTypes
        {
            [EnumLabel("text", SimulatorResources.Names.Message_PayloadType_Text, typeof(SimulatorResources))]
            String,
            [EnumLabel("binary", SimulatorResources.Names.Message_PayloadType_Binary, typeof(SimulatorResources))]
            Binary
        }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_PayloadType, HelpResource: Resources.SimulatorResources.Names.Message_PayloadType_Help, FieldType: FieldTypes.Picker, EnumType:typeof(PaylodTypes), ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public EntityHeader<PaylodTypes> PayloadType { get; set; } 

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Key, HelpResource: Resources.SimulatorResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Key { get; set; }
        

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string Description { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_MessageHeaders, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<MessageHeader> MessageHeaders { get; set; }


        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_MessageTokens, HelpResource: Resources.SimulatorResources.Names.Message_MessageToken_Help, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public List<String> Tokens { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_PayloadType_Text, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string TextPayload { get; set; }


        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_PayloadType_Binary, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public byte[] BinaryPayload { get; set; }
    }
}
