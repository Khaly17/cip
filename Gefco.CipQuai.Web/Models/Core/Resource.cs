using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class Resource : BaseModel
    {
        [JsonIgnore]
        public bool IsForAll { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        [JsonIgnore]
        public DateTime LastUpdate { get; set; }
        public ResourceType Type { get; set; }
        public byte[] ValueAsBytes { get; set; }
        [JsonIgnore]
        public List<ResourceRole> Roles { get; set; }
        [JsonIgnore]
        public List<ApplicationUser> Users { get; set; }
        public override BaseModel Clone()
        {
            var obj = new Resource
            {
                CreationDate = CreationDate,
                Id = Id,
                Key = Key,
                Value = Value,
                LastUpdate = LastUpdate,
                Type = Type,
                ValueAsBytes = ValueAsBytes
            };
            return obj;
        }
    }

    public class ResourceRole : BaseModel
    {
        public virtual IdentityRole Role { get; set; }
        public virtual Resource Resource { get; set; }

        public override BaseModel Clone()
        {
            var item = new ResourceRole()
            {
                CreationDate = CreationDate,
                Id = Id,
                Resource = Resource,
                Role = Role
            };
            return item;
        }
    }

    public enum ResourceType
    {
        Text = 0,
        ImageUrl = 1,
        BinaryImage = 2,
        PageUrl = 3,
        Boolean = 4
    }
}