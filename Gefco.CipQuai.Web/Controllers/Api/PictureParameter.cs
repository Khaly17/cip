using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Controllers.Api
{
    public class PictureParameter
    {
        public string AppVersion { get; set; }
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FileContent { get; set; }
        public string DeclarationId { get; set; }
        public PictureType PictureType { get; set; }
    }
}
