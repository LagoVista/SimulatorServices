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
        [EnumLabel(Simulator.Transport_RestHttp, SimulatorResources.Names.Transport_REST_Http, typeof(SimulatorResources))]
        RestHttp,
        [EnumLabel(Simulator.Transport_RestHttps, SimulatorResources.Names.Transport_REST_Https, typeof(SimulatorResources))]
        RestHttps,
        [EnumLabel(Simulator.Transport_MQTT, SimulatorResources.Names.Transport_MQTT, typeof(SimulatorResources))]
        MQTT,
        [EnumLabel(Simulator.Transport_AMQP, SimulatorResources.Names.Transport_AMQP, typeof(SimulatorResources))]
        AMQP,
        [EnumLabel(Simulator.Transport_Azure_EventHub, SimulatorResources.Names.Transport_AzureEventHub, typeof(SimulatorResources))]
        AzureEventHub,
        [EnumLabel(Simulator.Transport_AzureServiceBus, SimulatorResources.Names.Transport_AzureServiceBus, typeof(SimulatorResources))]
        AzureServiceBus,
        [EnumLabel(Simulator.Transport_IOT_HUB, SimulatorResources.Names.Transport_AzureIoTHub, typeof(SimulatorResources))]
        AzureIoTHub,
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


        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_AccessKeyName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String AccessKeyName { get; set; }

        [FormField(LabelResource: Resources.SimulatorResources.Names.Simulator_AccessKey, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String AccessKey { get; set; }


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
