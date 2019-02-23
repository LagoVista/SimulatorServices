using LagoVista.Core;
using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Models.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.SimulatorInstance_Title, SimulatorResources.Names.SimulatorInstance_Help, SimulatorResources.Names.SimulatorInstance_Description,
    EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]
    public class SimulatorInstance : IEntityHeaderEntity, IValidateable
    {
        public SimulatorInstance()
        {
            Id = Guid.NewGuid().ToId();
            TransmissionPlans = new List<MessageTransmissionPlan>();
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Key, HelpResource: SimulatorResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources), IsRequired: false)]
        public string Description { get; set; }


        [FormField(LabelResource: SimulatorResources.Names.SimulatorInstance_Simulator, FieldType: FieldTypes.EntityHeaderPicker, WaterMark: SimulatorResources.Names.SimulatorInstance_SelectSimulator, ResourceType: typeof(SimulatorResources))]

        public EntityHeader<Simulator> Simulator { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.SimulatorInstance_DeviceId, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string DeviceId { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.SimulatorInstance_TransmissionPlan, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<MessageTransmissionPlan> TransmissionPlans { get; set; }

        public IEntityHeader ToEntityHeader()
        {
            return EntityHeader.Create(Id, Name);
        }
    }

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageTransmissionPlan_Title, SimulatorResources.Names.MessageTransmissionPlan_Help, SimulatorResources.Names.MessageTransmissionPlan_Description,
    EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]
    public class MessageTransmissionPlan : IEntityHeaderEntity, IValidateable
    {
        public MessageTransmissionPlan()
        {
            Id = Guid.NewGuid().ToId();
            Values = new List<MessageValue>();
            PeriodMS = 1000;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Key, HelpResource: SimulatorResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTransmissionPlan_PeriodMS, FieldType: FieldTypes.Integer,  ResourceType: typeof(SimulatorResources))]
        public int PeriodMS { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTransmissionPlan_MessageTemplate, FieldType: FieldTypes.EntityHeaderPicker, WaterMark: SimulatorResources.Names.MessageTransmissionPlan_SelectMessage, ResourceType: typeof(SimulatorResources))]
        public EntityHeader<MessageTemplate> Message { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTransmissionPlan_Values, FieldType: FieldTypes.ChildList, ResourceType: typeof(SimulatorResources))]
        public List<MessageValue> Values { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTransmissionPlan_ForState, FieldType: FieldTypes.EntityHeaderPicker, ResourceType: typeof(SimulatorResources))]
        public EntityHeader<SimulatorState> ForState { get; set; }

        public IEntityHeader ToEntityHeader()
        {
            return EntityHeader.Create(Id, Name);
        }

        [CustomValidatorAttribute]
        public void Validate(ValidationResult res)
        {
            if (PeriodMS < 100) res.AddUserError("Please provide a number larger then 100ms for the period.");
        }
    }

    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageValue_Title, SimulatorResources.Names.MessageValue_Help, SimulatorResources.Names.MessageValue_Description,
    EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources))]
    public class MessageValue : IEntityHeaderEntity, IValidateable
    {
        [FormField(LabelResource: SimulatorResources.Names.MessageValue_Attribute, FieldType: FieldTypes.EntityHeaderPicker, WaterMark:SimulatorResources.Names.MessageValue_SelectAttribute, ResourceType: typeof(SimulatorResources))]
        public EntityHeader<MessageDynamicAttribute> Attribute { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageValue_Value, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Value { get; set; }

        public IEntityHeader ToEntityHeader()
        {
            return Attribute;
        }
    }
}
