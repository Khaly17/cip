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
    public class DetailDPViewModel
    {
        public DeclarationDoublePlancher DP { get; }

        public DetailDPViewModel(DeclarationDoublePlancher declaration, List<TempPicture> tempPictures)
        {
            DP = declaration;
            CompletionDate = declaration.CompletionDate;
            DueDate = declaration.Traction.DueDate;
            TractionName = declaration.Traction.Name;
            CreatedBy = declaration.CreatedBy.FirstName + " " + declaration.CreatedBy.LastName;
            Motifs = declaration.MotifDps.ToList();
            IsDPUsed = declaration.IsDPUsed;
            AutreMotifDP = declaration.AutreMotifDP;
            if (declaration.Pictures == null)
                declaration.Pictures = new List<Picture>();
            foreach (var tempPicture in tempPictures)
                declaration.Pictures.Add(new Picture(tempPicture));
            if (declaration.Pictures != null)
            {
                var pictures = declaration.Pictures.OrderByDescending(p => p.CreationDate).DistinctBy(p => p.PictureType).ToList();
                var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var pic = pictures.SingleOrDefault(p => p.PictureType == PictureType.HalfLoadPicture);
                if (pic != null)
                    Picture1 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
                pic = pictures.SingleOrDefault(p => p.PictureType == PictureType.FullLoadPicture);
                if (pic != null)
                    Picture2 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
                pic = pictures.SingleOrDefault(p => p.PictureType == PictureType.ErrorPicture1);
                if (pic != null)
                    ErrorPicture1 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
                pic = pictures.SingleOrDefault(p => p.PictureType == PictureType.ErrorPicture2);
                if (pic != null)
                    ErrorPicture2 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
                pic = pictures.SingleOrDefault(p => p.PictureType == PictureType.ErrorPicture3);
                if (pic != null)
                    ErrorPicture3 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
            }
            Id = declaration.Id;
        }

        public string AutreMotifDP { get; set; }

        public bool IsDPUsed { get; set; }

        public List<MotifDP> Motifs { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string TractionName { get; set; }
        public string CreatedBy { get; set; }

        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string ErrorPicture1 { get; set; }
        public string ErrorPicture2 { get; set; }
        public string ErrorPicture3 { get; set; }

        public string Id { get; set; }
    }
}