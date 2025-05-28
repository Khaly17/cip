using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Results
{
    public class PictureServiceResult : ServiceResult<Picture>
    {
        public PictureServiceResult()
        {
            
        }
        public PictureServiceResult(Picture picture)
        {
            Value = picture;
            IsSuccess = true;
        }
    }
}