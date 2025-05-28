using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public abstract class BaseModel
    {
        [Required]
        [Key]
        public virtual string Id { get; set; }

        [JsonIgnore]
        public virtual bool IsDeleted { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(CreatedBy_Id))]
        public virtual ApplicationUser CreatedBy { get; set; }
        
        [JsonIgnore]
        public string CreatedBy_Id  { get; set; }
        
        
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime CreationDate { get; set; }
        
        public abstract BaseModel Clone();

    }
}