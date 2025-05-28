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
    public class DetailSPViewModel
    {
        public DetailSPViewModel(DeclarationSimplePlancher declaration, List<TempPicture> tempPictures)
        {
            CompletionDate = declaration.CreationDate;
            Origine = declaration.CreatedBy.FirstName + " " + declaration.CreatedBy.LastName;
            Destination = declaration.AutreAgenceArrivee;
            //Description = item.Description;
            if (declaration.Pictures == null)
                declaration.Pictures = new List<Picture>();
            foreach (var tempPicture in tempPictures)
                declaration.Pictures.Add(new Picture(tempPicture));
            if (declaration.Pictures != null)
            {
                var pictures = declaration.Pictures.OrderByDescending(p => p.CreationDate).DistinctBy(p => p.PictureType).ToList();
                var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var pic = pictures.SingleOrDefault(p => p.PictureType == PictureType.Picture1);
                if (pic != null)
                    Picture1 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
                pic = pictures.SingleOrDefault(p => p.PictureType == PictureType.Picture2);
                if (pic != null)
                    Picture2 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
                pic = pictures.SingleOrDefault(p => p.PictureType == PictureType.Picture3);
                if (pic != null)
                    Picture3 = url.Action("GetPicture", "Pictures", new { id = pic.Id });
            }
            Id = declaration.Id;
        }

        public DateTime? CompletionDate { get; set; }
        public string Origine { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }

        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }

        public string Id { get; set; }
    }
}