// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: dcdf58a12c95811d3171c7ebb7ff4e879d18f90be05b90434114e0d6c8f10c9d
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using Newtonsoft.Json;
using LagoVista.IoT.Simulator.Models.Resources;
using LagoVista.Core.Models.UIMetaData;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    public enum TransportTypes
    {
        [EnumLabel(Simulator.Transport_AMQP, SimulatorResources.Names.Transport_AMQP, typeof(SimulatorResources))]
        AMQP,
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

    public enum CredentialsStorage
    {
        [EnumLabel(Simulator.CredentialsStorage_InCloud, SimulatorResources.Names.Simulator_CredentialsStorage_InCloud, typeof(SimulatorResources))]
        InCloud,
        [EnumLabel(Simulator.CredentialsStorage_OnDevice, SimulatorResources.Names.Simulator_CredentialsStorage_OnDevice, typeof(SimulatorResources))]
        OnDevice,
        [EnumLabel(Simulator.CredentialsStorage_Prompt, SimulatorResources.Names.Simulator_CredentialsStorage_Prompt, typeof(SimulatorResources))]
        Prompt
    }

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.Simulator_Title, SimulatorResources.Names.Simulator_Description, 
        SimulatorResources.Names.Simulator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources), Icon: "icon-fo-information-computer",
        SaveUrl: "/api/simulator", GetUrl: "/api/simulator/{id}", GetListUrl: "/api/org/simulators", FactoryUrl: "/api/simulator/factory", DeleteUrl: "/api/simulator/{id}")]
    public class Simulator : EntityBase,  IValidateable, IFormDescriptor, IFormConditionalFields, IIconEntity, ISummaryFactory, IFormDescriptorCol2, IFormDescriptorBottom
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

        public const string CredentialsStorage_InCloud = "incloud";
        public const string CredentialsStorage_OnDevice = "device";
        public const string CredentialsStorage_Prompt = "prompt";

        public Simulator()
        {
            SimulatorStates = new List<SimulatorState>();
            MessageTemplates = new List<MessageTemplate>();
            Id = Guid.NewGuid().ToId();
            Icon = "icon-fo-information-computer";
        }

        [FormField(LabelResource: SimulatorResources.Names.Common_Icon, FieldType: FieldTypes.Icon, ResourceType: typeof(SimulatorResources), IsRequired: false, IsUserEditable: true)]
        public string Icon { get; set; }


        [FormField(LabelResource: SimulatorResources.Names.Simulator_Deployment_Config, FieldType: FieldTypes.EntityHeaderPicker, EntityHeaderPickerUrl: "/api/deployment/instances", WaterMark:SimulatorResources.Names.Simulator_DeploymentConfiguration_Watermark, 
            ResourceType: typeof(SimulatorResources))]
        public EntityHeader DeploymentConfiguration { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_Device_Config, FieldType: FieldTypes.EntityHeaderPicker, EntityHeaderPickerUrl: "/api/deviceconfigs", WaterMark: SimulatorResources.Names.Simulator_Device_Config_Watermark, 
            ResourceType: typeof(SimulatorResources))]
        public EntityHeader DeviceConfiguration { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_Solution, FieldType: FieldTypes.EntityHeaderPicker, EntityHeaderPickerUrl: "/api/deployment/solutions", WaterMark: SimulatorResources.Names.Simulator_Device_Config_Watermark,
             ResourceType: typeof(SimulatorResources))]
        public EntityHeader Solution { get; set; }
    
        [FormField(LabelResource: SimulatorResources.Names.Simulator_DeviceType, FieldType: FieldTypes.EntityHeaderPicker, EntityHeaderPickerUrl: "/api/devicetypes", WaterMark: SimulatorResources.Names.Simulator_DeviceType_Watermark,
            ResourceType: typeof(SimulatorResources))]
        public EntityHeader DeviceType { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_PipelineModule_Config, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(SimulatorResources),
            WaterMark: SimulatorResources.Names.Simulator_PipelineModule_Config_Watermark)]
        public EntityHeader PipelineModuleConfiguration { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_MessageTemplates, FieldType: FieldTypes.ChildListInline, FactoryUrl: "/api/simulator/messagetemplate/factory", InPlaceEditing:false, ResourceType: typeof(SimulatorResources))]
        public List<MessageTemplate> MessageTemplates { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_SimulatorStates, HelpResource:SimulatorResources.Names.Simulator_States_Help, FactoryUrl: "/api/simulator/state/factory", FieldType: FieldTypes.ChildListInline, ResourceType: typeof(SimulatorResources))]
        public List<SimulatorState> SimulatorStates { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_CredentialsStorage, HelpResource: SimulatorResources.Names.Simulator_CredentialsStorage_Help, FieldType: FieldTypes.Picker, EnumType: typeof(CredentialsStorage), ResourceType: typeof(SimulatorResources), WaterMark: SimulatorResources.Names.Simulator_CredentialsStorage_Select)]
        public EntityHeader<CredentialsStorage> CredentialStorage { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_DefaultTransport, FieldType: FieldTypes.Picker, EnumType: typeof(TransportTypes), ResourceType: typeof(SimulatorResources), IsRequired: true,
                WaterMark: SimulatorResources.Names.Transport_SelectTransportType)]
        public EntityHeader<TransportTypes> DefaultTransport { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_DefaultEndPoint, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string DefaultEndPoint { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_ConnectionString, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string ConnectionString { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_HubName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string HubName { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_QueueName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string QueueName { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_Topic, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string Topic { get; set; }


        [FormField(LabelResource: SimulatorResources.Names.Simulator_Subscription, HelpResource: SimulatorResources.Names.Simulator_Subscription_Help, FieldType: FieldTypes.Text,
            ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string Subscription { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_DefaultPort, FieldType: FieldTypes.Integer, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public int DefaultPort { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_DefaultPayloadType, HelpResource: SimulatorResources.Names.Message_PayloadType_Help, FieldType: FieldTypes.Picker,
            EnumType: typeof(PaylodTypes), ResourceType: typeof(SimulatorResources), WaterMark: SimulatorResources.Names.Message_SelectPayloadType, IsRequired: true)]
        public EntityHeader<PaylodTypes> DefaultPayloadType { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_Password, FieldType: FieldTypes.Secret, SecureIdFieldName: nameof(PasswordSecureId), ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String Password { get; set; }

        public string PasswordSecureId { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_AuthHeader, FieldType: FieldTypes.Secret, SecureIdFieldName: nameof(AuthHeaderSecureId), ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String AuthHeader { get; set; }
        public String AuthHeaderSecureId { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_UserName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String UserName { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_TLSSSL, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public bool TLSSSL { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_AccessKeyName, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String AccessKeyName { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_AccessKey, FieldType: FieldTypes.Secret, SecureIdFieldName: nameof(AccessKeySecureId), ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String AccessKey { get; set; }

        public String AccessKeySecureId { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_AnonymousConnection, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public bool Anonymous { get; set; }


        [FormField(LabelResource: SimulatorResources.Names.Simulator_BasicAuth, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public bool BasicAuth { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Simulator_DeviceId, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public String DeviceId { get; set; }

        public SimulatorSummary CreateSummary()
        {
            return new SimulatorSummary()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                Icon = Icon,
                Description = Description,
                EndPoint = DefaultEndPoint,
                Port = DefaultPort,
                TransportType = DefaultTransport.Text,
                DeviceConfiguration = DeviceConfiguration == null ? SimulatorResources.Common_None : DeviceConfiguration.Text,
                DeviceType = DeviceType == null ? SimulatorResources.Common_None : DeviceType.Text
            };
        }

        [CustomValidator]
        public void Validate(ValidationResult result, Actions action)
        {
            if (EntityHeader.IsNullOrEmpty(DefaultTransport))
            {
                result.AddUserError("Transport Type is a Required Field.");
                return;
            }

            switch (DefaultTransport.Value)
            {
                case TransportTypes.AzureEventHub:
                    if (EntityHeader.IsNullOrEmpty(CredentialStorage)) result.AddUserError("Please select where you would like your credentials stored.");
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (String.IsNullOrEmpty(HubName)) result.AddUserError("Hub Name is a Required Field.");

                    if (action == Actions.Create)
                    {
                        if (EntityHeader.IsNullOrEmpty(CredentialStorage) && this.CredentialStorage.Value == CredentialsStorage.InCloud && String.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is a Required Field");
                    }
                    break;
                case TransportTypes.AzureIoTHub:
                    if (EntityHeader.IsNullOrEmpty(CredentialStorage)) result.AddUserError("Please select where you would like your credentials stored.");
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (String.IsNullOrEmpty(DeviceId)) result.AddUserError("Device Id is a Required Field");

                    if (action == Actions.Create)
                    {
                        if (!EntityHeader.IsNullOrEmpty(CredentialStorage) && this.CredentialStorage.Value == CredentialsStorage.InCloud && String.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is a Required Field");
                    }

                    break;
                case TransportTypes.AzureServiceBus:
                    if (EntityHeader.IsNullOrEmpty(CredentialStorage)) result.AddUserError("Please select where you would like your credentials stored.");
                    if (String.IsNullOrEmpty(DefaultEndPoint)) result.AddUserError("Default Endpoint is a Required Field");
                    if (String.IsNullOrEmpty(QueueName)) result.AddUserError("Queue Name is a Required Field");
                    if (String.IsNullOrEmpty(AccessKeyName)) result.AddUserError("Access Key Name is a Required Field");

                    if (action == Actions.Create)
                    {
                        if (!EntityHeader.IsNullOrEmpty(CredentialStorage) && this.CredentialStorage.Value == CredentialsStorage.InCloud && String.IsNullOrEmpty(AccessKey)) result.AddUserError("Access Key is a Required Field");
                    }
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
                        if (EntityHeader.IsNullOrEmpty(CredentialStorage)) result.AddUserError("Please select where you would like your credentials stored.");
                        if (String.IsNullOrEmpty(UserName)) result.AddUserError("User Name is required if your connection is not anonymous.");
                        if (action == Actions.Create)
                        {
                            if (!EntityHeader.IsNullOrEmpty(CredentialStorage) && this.CredentialStorage.Value == CredentialsStorage.InCloud && String.IsNullOrEmpty(Password)) result.AddUserError("Password is required if your connection is not anonymous.");
                        }
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
                        if (action == Actions.Create)
                        {
                            if (EntityHeader.IsNullOrEmpty(CredentialStorage)) result.AddUserError("Please select where you would like your credentials stored.");
                            if (BasicAuth)
                            {
                                if (String.IsNullOrEmpty(UserName)) result.AddUserError("User Name is required if your connection is not anonymous.");
                                if (!EntityHeader.IsNullOrEmpty(CredentialStorage) && this.CredentialStorage.Value == CredentialsStorage.InCloud && String.IsNullOrEmpty(Password)) result.AddUserError("Password is required if your connection is not anonymous and basic auth.");
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(UserName)) result.AddUserError("User Name is required if your connection is not anonymous.");
                                if (!EntityHeader.IsNullOrEmpty(CredentialStorage) && this.CredentialStorage.Value == CredentialsStorage.InCloud && String.IsNullOrEmpty(AuthHeader)) result.AddUserError("Authentication Header is required if your connection is not anonymous and not basic auth.");
                            }
                        }
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

            foreach (var msg in MessageTemplates)
            {
                msg.Validate(result);
            }
        }

        
        public FormConditionals GetConditionalFields()
        {
            return new FormConditionals()
            {
                ConditionalFields = new List<string>()
                 {
                     nameof(DefaultEndPoint),
                     nameof(DefaultPort),
                     nameof(ConnectionString),
                     nameof(HubName),
                     nameof(QueueName),
                     nameof(Topic),
                     nameof(Subscription),

                     nameof(AuthHeader),
                     nameof(BasicAuth),
                     nameof(Anonymous),
                     nameof(UserName),
                     nameof(Password),

                     nameof(AccessKeyName),
                     nameof(TLSSSL),

                     nameof(AccessKey)
                 },
                Conditionals = new List<FormConditional>()
                {
                    new FormConditional()
                    {
                         Field = nameof(DefaultTransport),
                         Value = Transport_Azure_EventHub,
                         VisibleFields = new List<string>()
                         {
                             nameof(AccessKey),
                             nameof(DefaultEndPoint),
                             nameof(HubName),
                         }
                    },
                    new FormConditional()
                    {
                         Field = nameof(DefaultTransport),
                         Value = Transport_IOT_HUB,
                         VisibleFields = new List<string>()
                         {
                             nameof(AccessKey),
                             nameof(DefaultEndPoint),
                             nameof(DeviceId),
                         }
                    },
                    new FormConditional()
                    {
                         Field = nameof(DefaultTransport),
                         Value = Transport_AzureServiceBus,
                         VisibleFields = new List<string>()
                         {
                             nameof(AccessKey),
                             nameof(QueueName),
                             nameof(DefaultEndPoint),
                             nameof(DeviceId),
                         }
                    },
                    new FormConditional()
                    {
                         Field = nameof(DefaultTransport),
                         Value = Transport_MQTT,
                         VisibleFields = new List<string>()
                         {
                             nameof(Anonymous),
                             nameof(DefaultPort),
                             nameof(DefaultEndPoint),
                             nameof(UserName),
                             nameof(Password),
                             nameof(Topic),
                         }
                    },
                    new FormConditional()
                    {
                         Field = nameof(DefaultTransport),
                         Value = Transport_RestHttp,
                         VisibleFields = new List<string>()
                         {
                             nameof(Anonymous),
                             nameof(DefaultPort),
                             nameof(DefaultEndPoint),
                             nameof(UserName),
                             nameof(Password),
                         }
                    },
                      new FormConditional()
                    {
                         Field = nameof(DefaultTransport),
                         Value = Transport_RestHttps,
                         VisibleFields = new List<string>()
                         {
                             nameof(Anonymous),
                             nameof(DefaultPort),
                             nameof(DefaultEndPoint),
                             nameof(UserName),
                             nameof(Password),
                         }
                    },
                    new FormConditional()
                    {
                         Field = nameof(DefaultTransport),
                         Value = Transport_TCP,
                         VisibleFields = new List<string>()
                         {
                             nameof(DefaultPort),
                             nameof(DefaultEndPoint),
                         }
                    },
                    new FormConditional()
                    {
                         Field = nameof(DefaultTransport),
                         Value = Transport_UDP,
                         VisibleFields = new List<string>()
                         {
                             nameof(DefaultPort),
                             nameof(DefaultEndPoint),
                         }
                    }
                }
            };
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Icon),
               
                nameof(DeploymentConfiguration),
                nameof(DeviceType),
                nameof(DeviceConfiguration),
                nameof(Solution),
                
                nameof(SimulatorStates),
                nameof(MessageTemplates),
            };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary();
        }

        public List<string> GetFormFieldsCol2()
        {
            return new List<string>()
            { 
                nameof(DeviceId),
                nameof(DefaultTransport),
                nameof(DefaultEndPoint),
                nameof(DefaultPort),
                nameof(DefaultPayloadType),
                nameof(ConnectionString),

                nameof(HubName),
                nameof(QueueName),
                nameof(Topic),
                nameof(Subscription),

                nameof(Anonymous),
                nameof(BasicAuth),

                nameof(UserName),
                nameof(Password),
                nameof(AuthHeader),

                nameof(TLSSSL),

                nameof(AccessKeyName),
                nameof(AccessKey),
            };
        }

        public List<string> GetFormFieldsBottom()
        {
            return new List<string>()
            {
                nameof(Description),
            };
        }
    }

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.Simulators_Title, SimulatorResources.Names.Simulator_Description,
       SimulatorResources.Names.Simulator_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources), Icon: "icon-fo-information-computer", 
       SaveUrl: "/api/simulator", GetUrl: "/api/simulator/{id}", GetListUrl: "/api/org/simulators", FactoryUrl: "/api/simulator/factory", DeleteUrl: "/api/simulator/{id}")]
    public class SimulatorSummary : SummaryData
    {
        public String EndPoint { get; set; }
        public int Port { get; set; }
        public String TransportType { get; set; }
        public string DeviceConfiguration { get; set; }
        public string DeviceType { get; set; }
    }
}
