// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Gefco.CipQuai.ApiClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class IdentityUserRole
    {
        /// <summary>
        /// Initializes a new instance of the IdentityUserRole class.
        /// </summary>
        public IdentityUserRole()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IdentityUserRole class.
        /// </summary>
        public IdentityUserRole(string userId = default(string), string roleId = default(string))
        {
            UserId = userId;
            RoleId = roleId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "UserId")]
        public string UserId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RoleId")]
        public string RoleId { get; set; }

    }
}
