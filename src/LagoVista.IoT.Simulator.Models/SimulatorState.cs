using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Simulator.Models.Resources;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LagoVista.IoT.Simulator.Admin.Models
{
    [EntityDescription(SimulatorDomain.SimulatorAdmin, SimulatorResources.Names.SimulatorState_Title, SimulatorResources.Names.SimulatorState_Help, 
        SimulatorResources.Names.SimulatorState_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(SimulatorResources),
        FactoryUrl: "/api/simulator/state/factory")]
    public class SimulatorState : IIDEntity, IKeyedEntity, INamedEntity, IDescriptionEntity, IValidateable, IEntityHeaderEntity, IFormDescriptor
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Name { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Key, HelpResource: SimulatorResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: SimulatorResources.Names.Common_Key_Validation, ResourceType: typeof(SimulatorResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_IsPublic, FieldType: FieldTypes.CheckBox, ResourceType: typeof(SimulatorResources))]
        public bool IsPublic { get; set; }

        [FormField(LabelResource: SimulatorResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(SimulatorResources))]
        public string Description { get; set; }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Description)
            };
        }

        public IEntityHeader ToEntityHeader()
        {
            return EntityHeader.Create(Id, Name);  
        }
    }
}
