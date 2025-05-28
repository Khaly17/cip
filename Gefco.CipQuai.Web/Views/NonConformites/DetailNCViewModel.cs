using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Extensions;
using Gefco.CipQuai.Web.Models;
using Microsoft.Ajax.Utilities;

namespace Gefco.CipQuai.Web.Views
{
    public class DetailNCViewModel
    {
        public string AgenceConcernee_Id { get; set; }

        public DetailNCViewModel(DeclarationNonConformite declaration, List<TempPicture> tempPictures)
        {
            CompletionDate = declaration.CreationDate;
            CreatedBy = declaration.CreatedBy.FirstName + " " + declaration.CreatedBy.LastName;
            Origine = declaration.Agence?.Name;
            Destination = declaration.AgenceConcernée.Name;
            NumVoyage = declaration.NumVoyage;
            Motifs = declaration.MotifNCs.ToList();
            AutreMotifNC = declaration.AutreMotifNC;
            if (declaration.Pictures == null)
                declaration.Pictures = new List<Picture>();
            foreach (var tempPicture in tempPictures)
                declaration.Pictures.Add(new Picture(tempPicture));
            if (declaration.Pictures != null)
            {
                var pictures = declaration.Pictures.OrderByDescending(p => p.CreationDate).DistinctBy(p => p.PictureType).ToList();
                var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var pic = pictures.FirstOrDefault(p => p.PictureType == PictureType.Picture1);
                if (pic != null)
                    Picture1 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
                pic = pictures.FirstOrDefault(p => p.PictureType == PictureType.Picture2);
                if (pic != null)
                    Picture2 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
                pic = pictures.FirstOrDefault(p => p.PictureType == PictureType.Picture3);
                if (pic != null)
                    Picture3 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
            }
            Id = declaration.Id;
            Status = declaration.IsDeleted ? "Supprimée" : declaration.CurrentStatus?.Description ?? "A valider";
            AgenceConcernee_Id = declaration.AgenceConcernée_Id;
        }

        public string AutreMotifNC { get; set; }

        public List<MotifNC> Motifs { get; set; }

        public DateTime? CompletionDate { get; set; }
        public string CreatedBy { get; set; }
        public string Origine { get; set; }
        public string Destination { get; set; }
        public string NumVoyage { get; set; }

        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }

        public string Id { get; set; }
        public string Status { get; set; }
    }
}