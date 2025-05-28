using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

namespace Gefco.CipQuai.Web.Controllers.Api
{
    public class PicturesController : Controller
    {
        public ActionResult GetPicture(string id)
        {
            using (var dal = new Dal())
            {
                var picture = dal.FindPicture(id);
                if (picture == null)
                    picture = dal.FindTempPicture(id);
                if (System.IO.File.Exists(picture.PicturePath))
                {
                    System.IO.FileStream fileStream = new System.IO.FileStream(picture.PicturePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    Image img = new Bitmap(Image.FromStream(fileStream));
                    fileStream.Close();
                    if (Array.IndexOf(img.PropertyIdList, 274) > -1)
                    {
                        var orientation = (int) img.GetPropertyItem(274).Value[0];
                        switch (orientation)
                        {
                            case 1:
                                // No rotation required.
                                break;
                            case 2:
                                img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case 3:
                                img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case 4:
                                img.RotateFlip(RotateFlipType.Rotate180FlipX);
                                break;
                            case 5:
                                img.RotateFlip(RotateFlipType.Rotate90FlipX);
                                break;
                            case 6:
                                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                            case 7:
                                img.RotateFlip(RotateFlipType.Rotate270FlipX);
                                break;
                            case 8:
                                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                        }
                        // This EXIF data is now invalid and should be removed.
                        img.RemovePropertyItem(274);
                        img.Save(picture.PicturePath);
                    }
                    img.Dispose();
                    Stream s = new MemoryStream(System.IO.File.ReadAllBytes(picture.PicturePath));
                    return new FileStreamResult(s, "image/jpeg");
                }
            }
            return null;
        }
    }
}
