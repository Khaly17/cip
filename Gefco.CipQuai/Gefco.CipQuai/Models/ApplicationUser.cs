namespace Gefco.CipQuai.ApiClient.Models
{
    public partial class ApplicationUser
    {
        public string DisplayName => FirstName + " " + LastName;
        partial void CustomInit()
        {
            if (ProfilePicture == null)
                ProfilePicture = new Picture(){ PicturePath = "User.svg" };
        }
    }
}