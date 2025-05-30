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

    public partial class Agence
    {
        /// <summary>
        /// Initializes a new instance of the Agence class.
        /// </summary>
        public Agence()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Agence class.
        /// </summary>
        public Agence(string name, string id, System.DateTime creationDate, bool? isUnderWatch = default(bool?), AgenceType agenceType = default(AgenceType), string otherName = default(string))
        {
            IsUnderWatch = isUnderWatch;
            AgenceType = agenceType;
            OtherName = otherName;
            Name = name;
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
        [JsonProperty(PropertyName = "IsUnderWatch")]
        public bool? IsUnderWatch { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AgenceType")]
        public AgenceType AgenceType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "OtherName")]
        public string OtherName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

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
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
            if (Id == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Id");
            }
        }
    }
}
