using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;
using System.Web.Http.Routing;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class Picture : BaseModel
    {
        public Picture()
        {
            
        }
        public Picture(TempPicture picture)
        {
            DeclarationControleReception_Id = picture.DeclarationControleReception_Id;
            ApplicationUser_Id = picture.ApplicationUser_Id;
            DeclarationBonnePratique_Id = picture.DeclarationBonnePratique_Id;
            DeclarationNonConformite_Id = picture.DeclarationNonConformite_Id;
            DeclarationRemorque_Id = picture.DeclarationRemorque_Id;
            PicturePath = picture.PicturePath;
            PictureType = picture.PictureType;
            CreatedBy_Id = picture.CreatedBy_Id;
            CreationDate = picture.CreationDate;
            Id = picture.Id;
        }
        public Picture(Picture picture)
        {
            DeclarationControleReception_Id = picture.DeclarationControleReception_Id;
            ApplicationUser_Id = picture.ApplicationUser_Id;
            DeclarationBonnePratique_Id = picture.DeclarationBonnePratique_Id;
            DeclarationNonConformite_Id = picture.DeclarationNonConformite_Id;
            DeclarationRemorque_Id = picture.DeclarationRemorque_Id;
            PicturePath = picture.PicturePath;
            PictureType = picture.PictureType;
            CreatedBy_Id = picture.CreatedBy_Id;
            CreationDate = picture.CreationDate;
            Id = picture.Id;
        }
        public string PicturePath { get; set; }
        public PictureType PictureType { get; set; }

        [JsonIgnore]
        public string ApplicationUser_Id { get; set; }

        [ForeignKey("ApplicationUser_Id")]
        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [JsonIgnore]
        public string DeclarationRemorque_Id { get; set; }

        [ForeignKey("DeclarationRemorque_Id")]
        [JsonIgnore]
        public virtual DeclarationSimplePlancher DeclarationRemorque { get; set; }

        [JsonIgnore]
        public string DeclarationControleReception_Id { get; set; }

        [ForeignKey("DeclarationControleReception_Id")]
        [JsonIgnore]
        public virtual DeclarationControleReception DeclarationControleReception { get; set; }

        [JsonIgnore]
        public string DeclarationBonnePratique_Id { get; set; }

        [JsonIgnore]
        [ForeignKey("DeclarationBonnePratique_Id")]
        public virtual DeclarationBonnePratique DeclarationBonnePratique { get; set; }
        [JsonIgnore]
        public string DeclarationNonConformite_Id { get; set; }

        [JsonIgnore]
        [ForeignKey("DeclarationNonConformite_Id")]
        public virtual DeclarationNonConformite DeclarationNonConformite { get; set; }
        public override BaseModel Clone()
        {
            var item = new Picture()
            {
                CreationDate = CreationDate,
                Id = Id,
                PictureType = PictureType,
                PicturePath = PicturePath
            };
            return item;
        }
    }
    public class TempPicture : BaseModel
    {
        public TempPicture()
        {
            
        }
        public TempPicture(Picture picture)
        {
            DeclarationControleReception_Id = picture.DeclarationControleReception_Id;
            ApplicationUser_Id = picture.ApplicationUser_Id;
            DeclarationBonnePratique_Id = picture.DeclarationBonnePratique_Id;
            DeclarationNonConformite_Id = picture.DeclarationNonConformite_Id;
            DeclarationRemorque_Id = picture.DeclarationRemorque_Id;
            PicturePath = picture.PicturePath;
            PictureType = picture.PictureType;
            CreatedBy_Id = picture.CreatedBy_Id;
            CreationDate = picture.CreationDate;
            Id = picture.Id;
        }
        public string PicturePath { get; set; }
        public PictureType PictureType { get; set; }

        public string ApplicationUser_Id { get; set; }
        public string DeclarationRemorque_Id { get; set; }
        public string DeclarationControleReception_Id { get; set; }
        public string DeclarationBonnePratique_Id { get; set; }
        public string DeclarationNonConformite_Id { get; set; }

        public override BaseModel Clone()
        {
            return null;
        }
    }
}