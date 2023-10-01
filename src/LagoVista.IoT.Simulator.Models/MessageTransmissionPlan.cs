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
    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.MessageTransmissionPlan_Title, SimulatorResources.Names.MessageTransmissionPlan_Help, SimulatorResources.Names.MessageTransmissionPlan_Description,
    EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources),
        FactoryUrl: "/api/simulator/instance/transmissionplan/factory")]
    public class MessageTransmissionPlan : IEntityHeaderEntity, IValidateable, IFormDescriptor
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

        [FormField(LabelResource: SimulatorResources.Names.MessageTransmissionPlan_Values, FieldType: FieldTypes.ChildListInline, ResourceType: typeof(SimulatorResources))]
        public List<MessageValue> Values { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.MessageTransmissionPlan_ForState, FieldType: FieldTypes.EntityHeaderPicker, WaterMark: SimulatorResources.Names.MessageTransmissionPlan_ForState_Select, ResourceType: typeof(SimulatorResources))]
        public EntityHeader<SimulatorState> ForState { get; set; }

        public bool OneTime { get; set; } = false;

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(PeriodMS),
                nameof(Message),
                nameof(ForState),
                nameof(Values),
            };
        }

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
}
