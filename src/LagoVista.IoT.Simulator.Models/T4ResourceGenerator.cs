using System.Globalization;
using System.Reflection;

//Resources:SimulatorResources:Common_Description
namespace LagoVista.IoT.Simulator.Models.Resources
{
	public class SimulatorResources
	{
        private static global::System.Resources.ResourceManager _resourceManager;
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static global::System.Resources.ResourceManager ResourceManager 
		{
            get 
			{
                if (object.ReferenceEquals(_resourceManager, null)) 
				{
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LagoVista.IoT.Simulator.Models.Resources.SimulatorResources", typeof(SimulatorResources).GetTypeInfo().Assembly);
                    _resourceManager = temp;
                }
                return _resourceManager;
            }
        }
        
        /// <summary>
        ///   Returns the formatted resource string.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static string GetResourceString(string key, params string[] tokens)
		{
			var culture = CultureInfo.CurrentCulture;;
            var str = ResourceManager.GetString(key, culture);

			for(int i = 0; i < tokens.Length; i += 2)
				str = str.Replace(tokens[i], tokens[i+1]);
										
            return str;
        }
        
        /// <summary>
        ///   Returns the formatted resource string.
        /// </summary>
		/*
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static HtmlString GetResourceHtmlString(string key, params string[] tokens)
		{
			var str = GetResourceString(key, tokens);
							
			if(str.StartsWith("HTML:"))
				str = str.Substring(5);

			return new HtmlString(str);
        }*/
		
		public static string Common_Description { get { return GetResourceString("Common_Description"); } }
//Resources:SimulatorResources:Common_IsPublic

		public static string Common_IsPublic { get { return GetResourceString("Common_IsPublic"); } }
//Resources:SimulatorResources:Common_Key

		public static string Common_Key { get { return GetResourceString("Common_Key"); } }
//Resources:SimulatorResources:Common_Key_Help

		public static string Common_Key_Help { get { return GetResourceString("Common_Key_Help"); } }
//Resources:SimulatorResources:Common_Key_Validation

		public static string Common_Key_Validation { get { return GetResourceString("Common_Key_Validation"); } }
//Resources:SimulatorResources:Common_Name

		public static string Common_Name { get { return GetResourceString("Common_Name"); } }
//Resources:SimulatorResources:Common_None

		public static string Common_None { get { return GetResourceString("Common_None"); } }
//Resources:SimulatorResources:Common_Script

		public static string Common_Script { get { return GetResourceString("Common_Script"); } }
//Resources:SimulatorResources:Connection_Select_Type

		public static string Connection_Select_Type { get { return GetResourceString("Connection_Select_Type"); } }
//Resources:SimulatorResources:Connection_Type_AMQP

		public static string Connection_Type_AMQP { get { return GetResourceString("Connection_Type_AMQP"); } }
//Resources:SimulatorResources:Connection_Type_AzureEventHub

		public static string Connection_Type_AzureEventHub { get { return GetResourceString("Connection_Type_AzureEventHub"); } }
//Resources:SimulatorResources:Connection_Type_AzureIoTHub

		public static string Connection_Type_AzureIoTHub { get { return GetResourceString("Connection_Type_AzureIoTHub"); } }
//Resources:SimulatorResources:Connection_Type_AzureServiceBus

		public static string Connection_Type_AzureServiceBus { get { return GetResourceString("Connection_Type_AzureServiceBus"); } }
//Resources:SimulatorResources:Connection_Type_Custom

		public static string Connection_Type_Custom { get { return GetResourceString("Connection_Type_Custom"); } }
//Resources:SimulatorResources:Connection_Type_MQTT

		public static string Connection_Type_MQTT { get { return GetResourceString("Connection_Type_MQTT"); } }
//Resources:SimulatorResources:Connection_Type_Rest

		public static string Connection_Type_Rest { get { return GetResourceString("Connection_Type_Rest"); } }
//Resources:SimulatorResources:Connection_Type_Soap

		public static string Connection_Type_Soap { get { return GetResourceString("Connection_Type_Soap"); } }
//Resources:SimulatorResources:Connection_Type_TCP

		public static string Connection_Type_TCP { get { return GetResourceString("Connection_Type_TCP"); } }
//Resources:SimulatorResources:Connection_Type_UDP

		public static string Connection_Type_UDP { get { return GetResourceString("Connection_Type_UDP"); } }
//Resources:SimulatorResources:HttpVerb_DELETE

		public static string HttpVerb_DELETE { get { return GetResourceString("HttpVerb_DELETE"); } }
//Resources:SimulatorResources:HttpVerb_GET

		public static string HttpVerb_GET { get { return GetResourceString("HttpVerb_GET"); } }
//Resources:SimulatorResources:HttpVerb_POST

		public static string HttpVerb_POST { get { return GetResourceString("HttpVerb_POST"); } }
//Resources:SimulatorResources:HttpVerb_PUT

		public static string HttpVerb_PUT { get { return GetResourceString("HttpVerb_PUT"); } }
//Resources:SimulatorResources:Message_MessageHeaders

		public static string Message_MessageHeaders { get { return GetResourceString("Message_MessageHeaders"); } }
//Resources:SimulatorResources:Message_MessageToken_Help

		public static string Message_MessageToken_Help { get { return GetResourceString("Message_MessageToken_Help"); } }
//Resources:SimulatorResources:Message_MessageTokens

		public static string Message_MessageTokens { get { return GetResourceString("Message_MessageTokens"); } }
//Resources:SimulatorResources:Message_PathAndQueryString

		public static string Message_PathAndQueryString { get { return GetResourceString("Message_PathAndQueryString"); } }
//Resources:SimulatorResources:Message_PayloadType

		public static string Message_PayloadType { get { return GetResourceString("Message_PayloadType"); } }
//Resources:SimulatorResources:Message_PayloadType_Binary

		public static string Message_PayloadType_Binary { get { return GetResourceString("Message_PayloadType_Binary"); } }
//Resources:SimulatorResources:Message_PayloadType_GeoPath

		public static string Message_PayloadType_GeoPath { get { return GetResourceString("Message_PayloadType_GeoPath"); } }
//Resources:SimulatorResources:Message_PayloadType_Help

		public static string Message_PayloadType_Help { get { return GetResourceString("Message_PayloadType_Help"); } }
//Resources:SimulatorResources:Message_PayloadType_Text

		public static string Message_PayloadType_Text { get { return GetResourceString("Message_PayloadType_Text"); } }
//Resources:SimulatorResources:Message_SelectPayloadType

		public static string Message_SelectPayloadType { get { return GetResourceString("Message_SelectPayloadType"); } }
//Resources:SimulatorResources:MessageDynamicAttribute_DefaultValue

		public static string MessageDynamicAttribute_DefaultValue { get { return GetResourceString("MessageDynamicAttribute_DefaultValue"); } }
//Resources:SimulatorResources:MessageDynamicAttribute_Description

		public static string MessageDynamicAttribute_Description { get { return GetResourceString("MessageDynamicAttribute_Description"); } }
//Resources:SimulatorResources:MessageDynamicAttribute_Help

		public static string MessageDynamicAttribute_Help { get { return GetResourceString("MessageDynamicAttribute_Help"); } }
//Resources:SimulatorResources:MessageDynamicAttribute_ParameterType

		public static string MessageDynamicAttribute_ParameterType { get { return GetResourceString("MessageDynamicAttribute_ParameterType"); } }
//Resources:SimulatorResources:MessageDynamicAttribute_Title

		public static string MessageDynamicAttribute_Title { get { return GetResourceString("MessageDynamicAttribute_Title"); } }
//Resources:SimulatorResources:MessageHeader_Description

		public static string MessageHeader_Description { get { return GetResourceString("MessageHeader_Description"); } }
//Resources:SimulatorResources:MessageHeader_HeaderName

		public static string MessageHeader_HeaderName { get { return GetResourceString("MessageHeader_HeaderName"); } }
//Resources:SimulatorResources:MessageHeader_HeaderName_Help

		public static string MessageHeader_HeaderName_Help { get { return GetResourceString("MessageHeader_HeaderName_Help"); } }
//Resources:SimulatorResources:MessageHeader_Help

		public static string MessageHeader_Help { get { return GetResourceString("MessageHeader_Help"); } }
//Resources:SimulatorResources:MessageHeader_Key

		public static string MessageHeader_Key { get { return GetResourceString("MessageHeader_Key"); } }
//Resources:SimulatorResources:MessageHeader_Key_Help

		public static string MessageHeader_Key_Help { get { return GetResourceString("MessageHeader_Key_Help"); } }
//Resources:SimulatorResources:MessageHeader_Title

		public static string MessageHeader_Title { get { return GetResourceString("MessageHeader_Title"); } }
//Resources:SimulatorResources:MessageHeader_Value

		public static string MessageHeader_Value { get { return GetResourceString("MessageHeader_Value"); } }
//Resources:SimulatorResources:MessageHeader_Value_Help

		public static string MessageHeader_Value_Help( string epoch, string deviceid, string jsonutcdate) { return GetResourceString("MessageHeader_Value_Help", "{epoch}", epoch, "{deviceid}", deviceid, "{jsonutcdate}", jsonutcdate); }
//Resources:SimulatorResources:MessageTemplate_AppendCR

		public static string MessageTemplate_AppendCR { get { return GetResourceString("MessageTemplate_AppendCR"); } }
//Resources:SimulatorResources:MessageTemplate_AppendLF

		public static string MessageTemplate_AppendLF { get { return GetResourceString("MessageTemplate_AppendLF"); } }
//Resources:SimulatorResources:MessageTemplate_BinaryPayload

		public static string MessageTemplate_BinaryPayload { get { return GetResourceString("MessageTemplate_BinaryPayload"); } }
//Resources:SimulatorResources:MessageTemplate_ContentType

		public static string MessageTemplate_ContentType { get { return GetResourceString("MessageTemplate_ContentType"); } }
//Resources:SimulatorResources:MessageTemplate_Description

		public static string MessageTemplate_Description { get { return GetResourceString("MessageTemplate_Description"); } }
//Resources:SimulatorResources:MessageTemplate_DynamicAttributes

		public static string MessageTemplate_DynamicAttributes { get { return GetResourceString("MessageTemplate_DynamicAttributes"); } }
//Resources:SimulatorResources:MessageTemplate_EndPoint

		public static string MessageTemplate_EndPoint { get { return GetResourceString("MessageTemplate_EndPoint"); } }
//Resources:SimulatorResources:MessageTemplate_Help

		public static string MessageTemplate_Help { get { return GetResourceString("MessageTemplate_Help"); } }
//Resources:SimulatorResources:MessageTemplate_HttpVerb

		public static string MessageTemplate_HttpVerb { get { return GetResourceString("MessageTemplate_HttpVerb"); } }
//Resources:SimulatorResources:MessageTemplate_HttpVerb_Select

		public static string MessageTemplate_HttpVerb_Select { get { return GetResourceString("MessageTemplate_HttpVerb_Select"); } }
//Resources:SimulatorResources:MessageTemplate_MessageId

		public static string MessageTemplate_MessageId { get { return GetResourceString("MessageTemplate_MessageId"); } }
//Resources:SimulatorResources:MessageTemplate_Port

		public static string MessageTemplate_Port { get { return GetResourceString("MessageTemplate_Port"); } }
//Resources:SimulatorResources:MessageTemplate_Properties

		public static string MessageTemplate_Properties { get { return GetResourceString("MessageTemplate_Properties"); } }
//Resources:SimulatorResources:MessageTemplate_QOS_Select

		public static string MessageTemplate_QOS_Select { get { return GetResourceString("MessageTemplate_QOS_Select"); } }
//Resources:SimulatorResources:MessageTemplate_QOS0

		public static string MessageTemplate_QOS0 { get { return GetResourceString("MessageTemplate_QOS0"); } }
//Resources:SimulatorResources:MessageTemplate_QOS1

		public static string MessageTemplate_QOS1 { get { return GetResourceString("MessageTemplate_QOS1"); } }
//Resources:SimulatorResources:MessageTemplate_QOS2

		public static string MessageTemplate_QOS2 { get { return GetResourceString("MessageTemplate_QOS2"); } }
//Resources:SimulatorResources:MessageTemplate_QOSLevel

		public static string MessageTemplate_QOSLevel { get { return GetResourceString("MessageTemplate_QOSLevel"); } }
//Resources:SimulatorResources:MessageTemplate_RetainFlag

		public static string MessageTemplate_RetainFlag { get { return GetResourceString("MessageTemplate_RetainFlag"); } }
//Resources:SimulatorResources:MessageTemplate_TextPayload

		public static string MessageTemplate_TextPayload { get { return GetResourceString("MessageTemplate_TextPayload"); } }
//Resources:SimulatorResources:MessageTemplate_Title

		public static string MessageTemplate_Title { get { return GetResourceString("MessageTemplate_Title"); } }
//Resources:SimulatorResources:MessageTemplate_To

		public static string MessageTemplate_To { get { return GetResourceString("MessageTemplate_To"); } }
//Resources:SimulatorResources:MessageTemplate_Topic

		public static string MessageTemplate_Topic { get { return GetResourceString("MessageTemplate_Topic"); } }
//Resources:SimulatorResources:MessageTemplate_Transport

		public static string MessageTemplate_Transport { get { return GetResourceString("MessageTemplate_Transport"); } }
//Resources:SimulatorResources:Simulator_AccessKey

		public static string Simulator_AccessKey { get { return GetResourceString("Simulator_AccessKey"); } }
//Resources:SimulatorResources:Simulator_AccessKeyName

		public static string Simulator_AccessKeyName { get { return GetResourceString("Simulator_AccessKeyName"); } }
//Resources:SimulatorResources:Simulator_AnonymousConnection

		public static string Simulator_AnonymousConnection { get { return GetResourceString("Simulator_AnonymousConnection"); } }
//Resources:SimulatorResources:Simulator_BasicAuth

		public static string Simulator_BasicAuth { get { return GetResourceString("Simulator_BasicAuth"); } }
//Resources:SimulatorResources:Simulator_ConnectionString

		public static string Simulator_ConnectionString { get { return GetResourceString("Simulator_ConnectionString"); } }
//Resources:SimulatorResources:Simulator_CredentialsStorage

		public static string Simulator_CredentialsStorage { get { return GetResourceString("Simulator_CredentialsStorage"); } }
//Resources:SimulatorResources:Simulator_CredentialsStorage_Help

		public static string Simulator_CredentialsStorage_Help { get { return GetResourceString("Simulator_CredentialsStorage_Help"); } }
//Resources:SimulatorResources:Simulator_CredentialsStorage_InCloud

		public static string Simulator_CredentialsStorage_InCloud { get { return GetResourceString("Simulator_CredentialsStorage_InCloud"); } }
//Resources:SimulatorResources:Simulator_CredentialsStorage_OnDevice

		public static string Simulator_CredentialsStorage_OnDevice { get { return GetResourceString("Simulator_CredentialsStorage_OnDevice"); } }
//Resources:SimulatorResources:Simulator_CredentialsStorage_Prompt

		public static string Simulator_CredentialsStorage_Prompt { get { return GetResourceString("Simulator_CredentialsStorage_Prompt"); } }
//Resources:SimulatorResources:Simulator_CredentialsStorage_Select

		public static string Simulator_CredentialsStorage_Select { get { return GetResourceString("Simulator_CredentialsStorage_Select"); } }
//Resources:SimulatorResources:Simulator_DefaultEndPoint

		public static string Simulator_DefaultEndPoint { get { return GetResourceString("Simulator_DefaultEndPoint"); } }
//Resources:SimulatorResources:Simulator_DefaultPayloadType

		public static string Simulator_DefaultPayloadType { get { return GetResourceString("Simulator_DefaultPayloadType"); } }
//Resources:SimulatorResources:Simulator_DefaultPort

		public static string Simulator_DefaultPort { get { return GetResourceString("Simulator_DefaultPort"); } }
//Resources:SimulatorResources:Simulator_DefaultTransport

		public static string Simulator_DefaultTransport { get { return GetResourceString("Simulator_DefaultTransport"); } }
//Resources:SimulatorResources:Simulator_Deployment_Config

		public static string Simulator_Deployment_Config { get { return GetResourceString("Simulator_Deployment_Config"); } }
//Resources:SimulatorResources:Simulator_Description

		public static string Simulator_Description { get { return GetResourceString("Simulator_Description"); } }
//Resources:SimulatorResources:Simulator_Device_Config

		public static string Simulator_Device_Config { get { return GetResourceString("Simulator_Device_Config"); } }
//Resources:SimulatorResources:Simulator_DeviceId

		public static string Simulator_DeviceId { get { return GetResourceString("Simulator_DeviceId"); } }
//Resources:SimulatorResources:Simulator_DeviceType

		public static string Simulator_DeviceType { get { return GetResourceString("Simulator_DeviceType"); } }
//Resources:SimulatorResources:Simulator_Help

		public static string Simulator_Help { get { return GetResourceString("Simulator_Help"); } }
//Resources:SimulatorResources:Simulator_HubName

		public static string Simulator_HubName { get { return GetResourceString("Simulator_HubName"); } }
//Resources:SimulatorResources:Simulator_MessageTemplates

		public static string Simulator_MessageTemplates { get { return GetResourceString("Simulator_MessageTemplates"); } }
//Resources:SimulatorResources:Simulator_Password

		public static string Simulator_Password { get { return GetResourceString("Simulator_Password"); } }
//Resources:SimulatorResources:Simulator_PipelineModule_Config

		public static string Simulator_PipelineModule_Config { get { return GetResourceString("Simulator_PipelineModule_Config"); } }
//Resources:SimulatorResources:Simulator_QueueName

		public static string Simulator_QueueName { get { return GetResourceString("Simulator_QueueName"); } }
//Resources:SimulatorResources:Simulator_Subscription

		public static string Simulator_Subscription { get { return GetResourceString("Simulator_Subscription"); } }
//Resources:SimulatorResources:Simulator_Subscription_Help

		public static string Simulator_Subscription_Help( string deviceid) { return GetResourceString("Simulator_Subscription_Help", "{deviceid}", deviceid); }
//Resources:SimulatorResources:Simulator_Title

		public static string Simulator_Title { get { return GetResourceString("Simulator_Title"); } }
//Resources:SimulatorResources:Simulator_TLSSSL

		public static string Simulator_TLSSSL { get { return GetResourceString("Simulator_TLSSSL"); } }
//Resources:SimulatorResources:Simulator_Topic

		public static string Simulator_Topic { get { return GetResourceString("Simulator_Topic"); } }
//Resources:SimulatorResources:Simulator_UserName

		public static string Simulator_UserName { get { return GetResourceString("Simulator_UserName"); } }
//Resources:SimulatorResources:Transport_AMQP

		public static string Transport_AMQP { get { return GetResourceString("Transport_AMQP"); } }
//Resources:SimulatorResources:Transport_AzureEventHub

		public static string Transport_AzureEventHub { get { return GetResourceString("Transport_AzureEventHub"); } }
//Resources:SimulatorResources:Transport_AzureIoTHub

		public static string Transport_AzureIoTHub { get { return GetResourceString("Transport_AzureIoTHub"); } }
//Resources:SimulatorResources:Transport_AzureServiceBus

		public static string Transport_AzureServiceBus { get { return GetResourceString("Transport_AzureServiceBus"); } }
//Resources:SimulatorResources:Transport_MQTT

		public static string Transport_MQTT { get { return GetResourceString("Transport_MQTT"); } }
//Resources:SimulatorResources:Transport_RabbitMQ

		public static string Transport_RabbitMQ { get { return GetResourceString("Transport_RabbitMQ"); } }
//Resources:SimulatorResources:Transport_REST_Http

		public static string Transport_REST_Http { get { return GetResourceString("Transport_REST_Http"); } }
//Resources:SimulatorResources:Transport_REST_Https

		public static string Transport_REST_Https { get { return GetResourceString("Transport_REST_Https"); } }
//Resources:SimulatorResources:Transport_SelectTransportType

		public static string Transport_SelectTransportType { get { return GetResourceString("Transport_SelectTransportType"); } }
//Resources:SimulatorResources:Transport_TCP

		public static string Transport_TCP { get { return GetResourceString("Transport_TCP"); } }
//Resources:SimulatorResources:Transport_UDP

		public static string Transport_UDP { get { return GetResourceString("Transport_UDP"); } }

		public static class Names
		{
			public const string Common_Description = "Common_Description";
			public const string Common_IsPublic = "Common_IsPublic";
			public const string Common_Key = "Common_Key";
			public const string Common_Key_Help = "Common_Key_Help";
			public const string Common_Key_Validation = "Common_Key_Validation";
			public const string Common_Name = "Common_Name";
			public const string Common_None = "Common_None";
			public const string Common_Script = "Common_Script";
			public const string Connection_Select_Type = "Connection_Select_Type";
			public const string Connection_Type_AMQP = "Connection_Type_AMQP";
			public const string Connection_Type_AzureEventHub = "Connection_Type_AzureEventHub";
			public const string Connection_Type_AzureIoTHub = "Connection_Type_AzureIoTHub";
			public const string Connection_Type_AzureServiceBus = "Connection_Type_AzureServiceBus";
			public const string Connection_Type_Custom = "Connection_Type_Custom";
			public const string Connection_Type_MQTT = "Connection_Type_MQTT";
			public const string Connection_Type_Rest = "Connection_Type_Rest";
			public const string Connection_Type_Soap = "Connection_Type_Soap";
			public const string Connection_Type_TCP = "Connection_Type_TCP";
			public const string Connection_Type_UDP = "Connection_Type_UDP";
			public const string HttpVerb_DELETE = "HttpVerb_DELETE";
			public const string HttpVerb_GET = "HttpVerb_GET";
			public const string HttpVerb_POST = "HttpVerb_POST";
			public const string HttpVerb_PUT = "HttpVerb_PUT";
			public const string Message_MessageHeaders = "Message_MessageHeaders";
			public const string Message_MessageToken_Help = "Message_MessageToken_Help";
			public const string Message_MessageTokens = "Message_MessageTokens";
			public const string Message_PathAndQueryString = "Message_PathAndQueryString";
			public const string Message_PayloadType = "Message_PayloadType";
			public const string Message_PayloadType_Binary = "Message_PayloadType_Binary";
			public const string Message_PayloadType_GeoPath = "Message_PayloadType_GeoPath";
			public const string Message_PayloadType_Help = "Message_PayloadType_Help";
			public const string Message_PayloadType_Text = "Message_PayloadType_Text";
			public const string Message_SelectPayloadType = "Message_SelectPayloadType";
			public const string MessageDynamicAttribute_DefaultValue = "MessageDynamicAttribute_DefaultValue";
			public const string MessageDynamicAttribute_Description = "MessageDynamicAttribute_Description";
			public const string MessageDynamicAttribute_Help = "MessageDynamicAttribute_Help";
			public const string MessageDynamicAttribute_ParameterType = "MessageDynamicAttribute_ParameterType";
			public const string MessageDynamicAttribute_Title = "MessageDynamicAttribute_Title";
			public const string MessageHeader_Description = "MessageHeader_Description";
			public const string MessageHeader_HeaderName = "MessageHeader_HeaderName";
			public const string MessageHeader_HeaderName_Help = "MessageHeader_HeaderName_Help";
			public const string MessageHeader_Help = "MessageHeader_Help";
			public const string MessageHeader_Key = "MessageHeader_Key";
			public const string MessageHeader_Key_Help = "MessageHeader_Key_Help";
			public const string MessageHeader_Title = "MessageHeader_Title";
			public const string MessageHeader_Value = "MessageHeader_Value";
			public const string MessageHeader_Value_Help = "MessageHeader_Value_Help";
			public const string MessageTemplate_AppendCR = "MessageTemplate_AppendCR";
			public const string MessageTemplate_AppendLF = "MessageTemplate_AppendLF";
			public const string MessageTemplate_BinaryPayload = "MessageTemplate_BinaryPayload";
			public const string MessageTemplate_ContentType = "MessageTemplate_ContentType";
			public const string MessageTemplate_Description = "MessageTemplate_Description";
			public const string MessageTemplate_DynamicAttributes = "MessageTemplate_DynamicAttributes";
			public const string MessageTemplate_EndPoint = "MessageTemplate_EndPoint";
			public const string MessageTemplate_Help = "MessageTemplate_Help";
			public const string MessageTemplate_HttpVerb = "MessageTemplate_HttpVerb";
			public const string MessageTemplate_HttpVerb_Select = "MessageTemplate_HttpVerb_Select";
			public const string MessageTemplate_MessageId = "MessageTemplate_MessageId";
			public const string MessageTemplate_Port = "MessageTemplate_Port";
			public const string MessageTemplate_Properties = "MessageTemplate_Properties";
			public const string MessageTemplate_QOS_Select = "MessageTemplate_QOS_Select";
			public const string MessageTemplate_QOS0 = "MessageTemplate_QOS0";
			public const string MessageTemplate_QOS1 = "MessageTemplate_QOS1";
			public const string MessageTemplate_QOS2 = "MessageTemplate_QOS2";
			public const string MessageTemplate_QOSLevel = "MessageTemplate_QOSLevel";
			public const string MessageTemplate_RetainFlag = "MessageTemplate_RetainFlag";
			public const string MessageTemplate_TextPayload = "MessageTemplate_TextPayload";
			public const string MessageTemplate_Title = "MessageTemplate_Title";
			public const string MessageTemplate_To = "MessageTemplate_To";
			public const string MessageTemplate_Topic = "MessageTemplate_Topic";
			public const string MessageTemplate_Transport = "MessageTemplate_Transport";
			public const string Simulator_AccessKey = "Simulator_AccessKey";
			public const string Simulator_AccessKeyName = "Simulator_AccessKeyName";
			public const string Simulator_AnonymousConnection = "Simulator_AnonymousConnection";
			public const string Simulator_BasicAuth = "Simulator_BasicAuth";
			public const string Simulator_ConnectionString = "Simulator_ConnectionString";
			public const string Simulator_CredentialsStorage = "Simulator_CredentialsStorage";
			public const string Simulator_CredentialsStorage_Help = "Simulator_CredentialsStorage_Help";
			public const string Simulator_CredentialsStorage_InCloud = "Simulator_CredentialsStorage_InCloud";
			public const string Simulator_CredentialsStorage_OnDevice = "Simulator_CredentialsStorage_OnDevice";
			public const string Simulator_CredentialsStorage_Prompt = "Simulator_CredentialsStorage_Prompt";
			public const string Simulator_CredentialsStorage_Select = "Simulator_CredentialsStorage_Select";
			public const string Simulator_DefaultEndPoint = "Simulator_DefaultEndPoint";
			public const string Simulator_DefaultPayloadType = "Simulator_DefaultPayloadType";
			public const string Simulator_DefaultPort = "Simulator_DefaultPort";
			public const string Simulator_DefaultTransport = "Simulator_DefaultTransport";
			public const string Simulator_Deployment_Config = "Simulator_Deployment_Config";
			public const string Simulator_Description = "Simulator_Description";
			public const string Simulator_Device_Config = "Simulator_Device_Config";
			public const string Simulator_DeviceId = "Simulator_DeviceId";
			public const string Simulator_DeviceType = "Simulator_DeviceType";
			public const string Simulator_Help = "Simulator_Help";
			public const string Simulator_HubName = "Simulator_HubName";
			public const string Simulator_MessageTemplates = "Simulator_MessageTemplates";
			public const string Simulator_Password = "Simulator_Password";
			public const string Simulator_PipelineModule_Config = "Simulator_PipelineModule_Config";
			public const string Simulator_QueueName = "Simulator_QueueName";
			public const string Simulator_Subscription = "Simulator_Subscription";
			public const string Simulator_Subscription_Help = "Simulator_Subscription_Help";
			public const string Simulator_Title = "Simulator_Title";
			public const string Simulator_TLSSSL = "Simulator_TLSSSL";
			public const string Simulator_Topic = "Simulator_Topic";
			public const string Simulator_UserName = "Simulator_UserName";
			public const string Transport_AMQP = "Transport_AMQP";
			public const string Transport_AzureEventHub = "Transport_AzureEventHub";
			public const string Transport_AzureIoTHub = "Transport_AzureIoTHub";
			public const string Transport_AzureServiceBus = "Transport_AzureServiceBus";
			public const string Transport_MQTT = "Transport_MQTT";
			public const string Transport_RabbitMQ = "Transport_RabbitMQ";
			public const string Transport_REST_Http = "Transport_REST_Http";
			public const string Transport_REST_Https = "Transport_REST_Https";
			public const string Transport_SelectTransportType = "Transport_SelectTransportType";
			public const string Transport_TCP = "Transport_TCP";
			public const string Transport_UDP = "Transport_UDP";
		}
	}
}
