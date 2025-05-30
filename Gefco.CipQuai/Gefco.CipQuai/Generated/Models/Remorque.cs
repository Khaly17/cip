// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Gefco.CipQuai.ApiClient.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class Remorque
    {
        /// <summary>
        /// Initializes a new instance of the Remorque class.
        /// </summary>
        public Remorque()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Remorque class.
        /// </summary>
        public Remorque(string id, System.DateTime creationDate, string numéroRemorque = default(string), bool? isDoublePlancher = default(bool?))
        {
            NuméroRemorque = numéroRemorque;
            IsDoublePlancher = isDoublePlancher;
            Id = id;
            CreationDate = creationDate;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "NuméroRemorque")]
        public string NuméroRemorque { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsDoublePlancher")]
        public bool? IsDoublePlancher { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CreationDate")]
        public System.DateTime CreationDate { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Id == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Id");
            }
        }
    }
}
