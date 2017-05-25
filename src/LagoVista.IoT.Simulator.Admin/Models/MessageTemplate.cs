using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Simulator.Admin.Resources;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    public enum PaylodTypes
    {
        [EnumLabel(MessageTemplate.PayloadTypes_Text, SimulatorResources.Names.Message_PayloadType_Text, typeof(SimulatorResources))]
        String,
        [EnumLabel(MessageTemplate.PayloadTypes_Binary, SimulatorResources.Names.Message_PayloadType_Binary, typeof(SimulatorResources))]
        Binary
    }

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageTemplate_Title, SimulatorResources.Names.MessageTemplate_Help, SimulatorResources.Names.MessageTemplate_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]

    public class MessageTemplate : IIDEntity, INamedEntity, IKeyedEntity
    {
        public const string PayloadTypes_Text = "text";
        public const string PayloadTypes_Binary = "binary";

        public MessageTemplate()
        {
            MessageHeaders = new List<MessageHeader>();
            Id = Guid.NewGuid().ToId();
        }

        public String Id { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }        

        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_PayloadType, HelpResource: Resources.SimulatorResources.Names.Message_PayloadType_Help, FieldType: FieldTypes.Picker, EnumType:typeof(PaylodTypes), ResourceType: typeof(SimulatorResources), WaterMark:SimulatorResources.Names.Message_SelectPayloadType, IsRequired: true)]
        public EntityHeader<PaylodTypes> PayloadType { get; set; } 

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Key, HelpResource: Resources.SimulatorResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: Resources.SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Key { get; set; }        

        [FormField(LabelResource: Resources.SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string Description { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_MessageHeaders, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<MessageHeader> MessageHeaders { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.MessageTemplate_DynamicAttributes, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<MessageDynamicAttribute> DynamicAttributes { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_PathAndQueryString, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public String PathAndQueryString { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_PayloadType_Text, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string TextPayload { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Message_PayloadType_Binary, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public byte[] BinaryPayload { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.MessageTemplate_Transport, FieldType: FieldTypes.Picker, EnumType: typeof(TransportTypes), ResourceType: typeof(SimulatorResources), WaterMark: SimulatorResources.Names.Transport_SelectTransportType, IsRequired: true)]
        public EntityHeader<TransportTypes> Transport { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.MessageTemplate_EndPoint, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string EndPoint { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.MessageTemplate_Port, FieldType: FieldTypes.Integer, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public int Port { get; set; }
    }
}
