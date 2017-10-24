using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using System;
using LagoVista.Core;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.Simulator.Admin.Resources;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    public enum TransportTypes
    {
        /*[EnumLabel(Simulator.Transport_AMQP, SimulatorResources.Names.Transport_AMQP, typeof(SimulatorResources))]
        AMQP,*/
        [EnumLabel(Simulator.Transport_Azure_EventHub, SimulatorResources.Names.Transport_AzureEventHub, typeof(SimulatorResources))]
        AzureEventHub,
        [EnumLabel(Simulator.Transport_IOT_HUB, SimulatorResources.Names.Transport_AzureIoTHub, typeof(SimulatorResources))]
        AzureIoTHub,
        [EnumLabel(Simulator.Transport_AzureServiceBus, SimulatorResources.Names.Transport_AzureServiceBus, typeof(SimulatorResources))]
        AzureServiceBus,
        [EnumLabel(Simulator.Transport_MQTT, SimulatorResources.Names.Transport_MQTT, typeof(SimulatorResources))]
        MQTT,
        /*[EnumLabel(Simulator.Transport_RabbitMQ, SimulatorResources.Names.Transport_RabbitMQ, typeof(SimulatorResources))]
        RabbitMQ,*/
        [EnumLabel(Simulator.Transport_RestHttp, SimulatorResources.Names.Transport_REST_Http, typeof(SimulatorResources))]
        RestHttp,
        [EnumLabel(Simulator.Transport_RestHttps, SimulatorResources.Names.Transport_REST_Https, typeof(SimulatorResources))]
        RestHttps,
        [EnumLabel(Simulator.Transport_UDP, SimulatorResources.Names.Transport_UDP, typeof(SimulatorResources))]
        UDP,
        [EnumLabel(Simulator.Transport_TCP, SimulatorResources.Names.Transport_TCP, typeof(SimulatorResources))]
        TCP
    }

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.Simulator_Title, SimulatorResources.Names.Simulator_Description, SimulatorResources.Names.Simulator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]

    public class Simulator : ModelBase,  IKeyedEntity, IIDEntity, INamedEntity, IOwnedEntity, IAuditableEntity, IValidateable, INoSQLEntity, IEntityHeaderEntity
    {
        public const string Transport_RestHttp = "resthttp";
        public const string Transport_RestHttps = "resthttps";
        public const string Transport_RabbitMQ = "rabbitmq";
        public const string Transport_MQTT = "mqtt";
        public const string Transport_IOT_HUB = "azureiothub";
        public const string Transport_AzureServiceBus = "azureservicebus";
        public const string Transport_Azure_EventHub = "azureeventhub";
        public const string Transport_AMQP = "amqp";
        public const string Transport_UDP = "udp";
        public const string Transport_TCP = "tcp";
       
        public Simulator()
        {
            MessageTemplates = new List<MessageTemplate>();
            Id = Guid.NewGuid().ToId();
        }

        public string DatabaseName { get; set; }
        public string EntityType { get; set; }

        [JsonProperty("id")]
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


        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_DefaultTransport,FieldType:FieldTypes.Picker, EnumType:typeof(TransportTypes), ResourceType: typeof(SimulatorResources), IsRequired:true, WaterMark: SimulatorResources.Names.Transport_SelectTransportType)]
        public EntityHeader<TransportTypes> DefaultTransport { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_DefaultEndPoint, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired:false)]
        public string DefaultEndPoint { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_ConnectionString, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string ConnectionString { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_HubName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string HubName { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_QueueName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string QueueName { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_Topic, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string Topic { get; set; }


        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_Subscription, HelpResource:Resources.SimulatorResources.Names.Simulator_Subscription_Help, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string Subscription { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_DefaultPort, FieldType: FieldTypes.Integer, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public int DefaultPort { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_DefaultPayloadType, HelpResource: Resources.SimulatorResources.Names.Message_PayloadType_Help, FieldType: FieldTypes.Picker, EnumType: typeof(PaylodTypes), ResourceType: typeof(SimulatorResources), WaterMark: SimulatorResources.Names.Message_SelectPayloadType, IsRequired: true)]
        public EntityHeader<PaylodTypes> DefaultPayloadType { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_Password, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String Password { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_UserName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String UserName { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_TLSSSL, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public bool TLSSSL { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_AccessKeyName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String AccessKeyName { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_AccessKey, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String AccessKey { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_AnonymousConnection, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public bool Anonymous { get; set; }


        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_BasicAuth, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public bool BasicAuth { get; set; }
         
        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_DeviceId, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String DeviceId { get; set; }


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
                EndPoint = DefaultEndPoint,
                Port = DefaultPort,
                TransportType = DefaultTransport.Text,
                DeviceConfiguration = DeviceConfiguration == null ? SimulatorResources.Common_None : DeviceConfiguration.Text,
                DeviceType = DeviceType == null ? SimulatorResources.Common_None : DeviceType.Text
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult result)
        {
            if(EntityHeader.IsNullOrEmpty(DefaultTransport))
            {
                result.AddUserError("Transport Type is a Required Field.");
                return;
            }

            switch(DefaultTransport.Value)
            {
                case TransportTypes.AzureEventHub:
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (String.IsNullOrEmpty(HubName)) result.AddUserError("Hub Name is a Required Field.");
                    if (String.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is a Required Field");
                    if (String.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is a Required Field");
                    break;
                case TransportTypes.AzureIoTHub:
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (String.IsNullOrEmpty(DeviceId)) result.AddUserError("Device Id is a Required Field");
                    if (String.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is a Required Field");

                    break;
                case TransportTypes.AzureServiceBus:
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (String.IsNullOrEmpty(QueueName)) result.AddUserError("Queue Name is a Required Field");
                    if (String.IsNullOrEmpty(AccessKeyName)) result.AddUserError("Access Key Name is a Required Field"); 
                    if (String.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is a Required Field"); 
                    break;
                case TransportTypes.MQTT:
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (DefaultPort == 0) result.AddUserError("Port should not be zero");
                    if (Anonymous)
                    {
                        UserName = null;
                        Password = null;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(UserName)) result.AddUserError("User Name is required if your connection is not anonymous.");
                        if (String.IsNullOrEmpty(Password)) result.AddUserError("Password is required if your connection is not anonymous.");
                    }

                    break;
                case TransportTypes.RestHttp:
                case TransportTypes.RestHttps:
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (DefaultPort == 0) result.AddUserError("Port should not be zero");
                    if (Anonymous)
                    {
                        UserName = null;
                        Password = null;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(UserName)) result.AddUserError("User Name is required if your connection is not anonymous.");
                        if (String.IsNullOrEmpty(Password)) result.AddUserError("Password is required if your connection is not anonymous.");
                    }
                    break;
                case TransportTypes.TCP:
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (DefaultPort == 0) result.AddUserError("Port should not be zero");
                    break;
                case TransportTypes.UDP:
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (DefaultPort == 0) result.AddUserError("Port should not be zero");
                    break;
            }

            foreach(var msg in MessageTemplates)
            {
                msg.Validate(result);
            }
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

    public class SimulatorSummary : SummaryData
    {
        public String EndPoint { get; set; }
        public int Port { get; set; }
        public String TransportType { get; set; }

        public string DeviceConfiguration { get; set; }

        public string DeviceType { get; set; }
    }
}
