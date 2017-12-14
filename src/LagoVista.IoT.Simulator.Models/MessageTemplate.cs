using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Models.Resources;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    public enum PaylodTypes
    {
        [EnumLabel(MessageTemplate.PayloadTypes_Text, SimulatorResources.Names.Message_PayloadType_Text, typeof(SimulatorResources))]
        String,
        [EnumLabel(MessageTemplate.PayloadTypes_Binary, SimulatorResources.Names.Message_PayloadType_Binary, typeof(SimulatorResources))]
        Binary
    }

    public enum VerbTypes
    {
        [EnumLabel(MessageTemplate.HttpVerb_GET, SimulatorResources.Names.HttpVerb_GET, typeof(SimulatorResources))]
        GET,
        [EnumLabel(MessageTemplate.HttpVerb_POST, SimulatorResources.Names.HttpVerb_POST, typeof(SimulatorResources))]
        POST,
        [EnumLabel(MessageTemplate.HttpVerb_PUT, SimulatorResources.Names.HttpVerb_PUT, typeof(SimulatorResources))]
        PUT,
        [EnumLabel(MessageTemplate.HttpVerb_DELETE, SimulatorResources.Names.HttpVerb_DELETE, typeof(SimulatorResources))]
        DELETE,
    }

    public enum QualityOfServiceLevels
    {
        [EnumLabel(MessageTemplate.QOS0, SimulatorResources.Names.MessageTemplate_QOS0, typeof(SimulatorResources))]
        QOS0,
        [EnumLabel(MessageTemplate.QOS1, SimulatorResources.Names.MessageTemplate_QOS1, typeof(SimulatorResources))]
        QOS1,
        [EnumLabel(MessageTemplate.QOS1, SimulatorResources.Names.MessageTemplate_QOS2, typeof(SimulatorResources))]
        QOS2,
    }

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageTemplate_Title, SimulatorResources.Names.MessageTemplate_Help, SimulatorResources.Names.MessageTemplate_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]
    public class MessageTemplate : IIDEntity, INamedEntity, IKeyedEntity, IEntityHeaderEntity, IValidateable
    {
        public const string PayloadTypes_Text = "text";
        public const string PayloadTypes_Binary = "binary";

        public const string HttpVerb_GET = "GET";
        public const string HttpVerb_POST = "POST";
        public const string HttpVerb_PUT = "PUT";
        public const string HttpVerb_DELETE = "DELETE";

        public const string QOS0 = "qos0";
        public const string QOS1 = "qos1";
        public const string QOS2 = "qos2";

        public MessageTemplate()
        {
            MessageHeaders = new List<MessageHeader>();
            Properties = new List<KeyValuePair<string, string>>();
            DynamicAttributes = new List<MessageDynamicAttribute>();
            Id = Guid.NewGuid().ToId();
        }

        public String Id { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }        

        [FormField(LabelResource: SimulatorResources.Names.Message_PayloadType, HelpResource: SimulatorResources.Names.Message_PayloadType_Help, FieldType: FieldTypes.Picker, EnumType:typeof(PaylodTypes), ResourceType: typeof(SimulatorResources), WaterMark:SimulatorResources.Names.Message_SelectPayloadType, IsRequired: true)]
        public EntityHeader<PaylodTypes> PayloadType { get; set; } 

        [FormField(LabelResource: SimulatorResources.Names.Common_Key, HelpResource: SimulatorResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Key { get; set; }   
        

        [FormField(LabelResource: SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string Description { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Message_MessageHeaders, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<MessageHeader> MessageHeaders { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Message_MessageHeaders, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<KeyValuePair<string,string>> Properties { get; set; }


        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_DynamicAttributes, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<MessageDynamicAttribute> DynamicAttributes { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Message_PathAndQueryString, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public String PathAndQueryString { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Message_PayloadType_Text, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string TextPayload { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Message_PayloadType_Binary, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string BinaryPayload { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_QueueName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string QueueName { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_HttpVerb, FieldType: FieldTypes.Picker, EnumType: typeof(VerbTypes), WaterMark: SimulatorResources.Names.MessageTemplate_HttpVerb_Select, ResourceType: typeof(SimulatorResources))]
        public String HttpVerb { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_To, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources))]
        public String To { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_MessageId, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources))]
        public String MessageId { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_ContentType, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources))]
        public String ContentType { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_Topic, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources))]
        public String Topic { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_QOSLevel, FieldType: FieldTypes.Picker, EnumType: typeof(QualityOfServiceLevels), ResourceType: typeof(SimulatorResources), WaterMark: SimulatorResources.Names.MessageTemplate_QOS_Select)]
        public EntityHeader<QualityOfServiceLevels> QualityOfServiceLevel { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_RetainFlag, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources))]
        public bool RetainFlag { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_AppendCR, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources))]
        public bool AppendCR { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_AppendLF, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources))]
        public bool AppendLF { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_Transport, FieldType: FieldTypes.Picker, EnumType: typeof(TransportTypes), ResourceType: typeof(SimulatorResources), WaterMark: SimulatorResources.Names.Transport_SelectTransportType, IsRequired: true)]
        public EntityHeader<TransportTypes> Transport { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_EndPoint, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string EndPoint { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTemplate_Port, FieldType: FieldTypes.Integer, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public int Port { get; set; }

        public IEntityHeader ToEntityHeader()
        {
            return new EntityHeader()
            {
                Id = Id,
                Text = Name
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult result)
        {
            if(EntityHeader.IsNullOrEmpty(Transport))
            {
                result.AddUserError("Transport is a Required Field.");
                return;
            }

            switch(Transport.Value)
            {
                case TransportTypes.MQTT:
                    if (String.IsNullOrEmpty(Topic)) result.AddUserError("Topic is a Required Field.");
                    if (EntityHeader.IsNullOrEmpty(QualityOfServiceLevel)) result.AddUserError("QOS Must Be Defined");
                    break;
                case TransportTypes.AzureServiceBus:
                    if (String.IsNullOrEmpty(QueueName)) result.AddUserError("Queue Name a Required Field.");
                    break;

                case TransportTypes.RestHttp:
                case TransportTypes.RestHttps:
                    if (String.IsNullOrEmpty(HttpVerb))
                    {
                        result.AddUserError("HTTP Verb is a Required Field.");
                    }
                    else
                    {
                        HttpVerb = HttpVerb.ToUpper();
                        if (HttpVerb != HttpVerb_GET &&
                            HttpVerb != HttpVerb_PUT &&
                            HttpVerb != HttpVerb_POST &&
                            HttpVerb != HttpVerb_DELETE)
                        {
                            result.AddUserError("Currently only the HTTP Verbs GET, POST, PUT and DELETE are supported.");
                        }
                    }
                    
                    break;
            }

            foreach(var hdr in MessageHeaders)
            {
                hdr.Validate(result);
            }
        }
    }
}
