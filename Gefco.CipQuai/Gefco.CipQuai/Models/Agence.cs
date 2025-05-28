namespace Gefco.CipQuai.ApiClient.Models
{
    public partial class Agence
    {
        public bool IsGefcoFrance => AgenceType?.Value == "Gefco France";
        public bool IsInternational => AgenceType?.Value == "International";
        public bool IsClient => AgenceType?.Value == "Confrères";
    }
}