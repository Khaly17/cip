namespace Gefco.CipQuai.Web.Models
{
    public class Region : NameBaseModel
    {
        //public ApplicationUser Directeur { get; set; }
        //public ApplicationUser RQCO { get; set; }
        //public ApplicationUser CPO1 { get; set; }
        //public ApplicationUser CPO2 { get; set; }
        //public ApplicationUser RQHSE { get; set; }
        //public ApplicationUser AQHSE { get; set; }
        //public ApplicationUser AlternateQHSE { get; set; }
        public override BaseModel Clone()
        {
            var obj = new Region
            {
                CreationDate = CreationDate,
                Id = Id,
                Name = Name,
                AutoValidateNC = AutoValidateNC,
                AutoCloseDP = AutoCloseDP,
                //LimitDP = LimitDP,
                //LimitDPCount = LimitDPCount
                //Directeur = Directeur,
                //RQCO = RQCO,
                //CPO1 = CPO1,
                //CPO2 = CPO2,
                //RQHSE = RQHSE,
                //AQHSE = AQHSE,
                //AlternateQHSE = AlternateQHSE,
            };
            return obj;
        }

        public bool AutoValidateNC { get; set; }
        public bool AutoCloseDP { get; set; }
        //public bool LimitDP { get; set; }
        //public int LimitDPCount { get; set; }
    }
}