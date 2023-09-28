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
    EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources),
        FactoryUrl: "/api/simulator/instance/factory")]
    public class SimulatorInstance : IEntityHeaderEntity, IValidateable, IFormDescriptor
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

        [FormField(LabelResource: SimulatorResources.Names.SimulatorInstance_TransmissionPlan, FieldType: FieldTypes.ChildListInline, ResourceType: typeof(SimulatorResources))]
        public List<MessageTransmissionPlan> TransmissionPlans { get; set; }



        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Simulator),
                nameof(DeviceId),
                nameof(Key),
                nameof(Description),
                nameof(TransmissionPlans)
            };
        }

        public IEntityHeader ToEntityHeader()
        {
            return EntityHeader.Create(Id, Name);
        }
    }
}
