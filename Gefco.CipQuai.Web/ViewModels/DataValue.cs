namespace Gefco.CipQuai.Web.Models
{
    public class DataValue
    {
        public DataValue(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }
        public DataValue(string name, int value, string color)
        {
            this.Name = name;
            this.Value = value;
            this.Color = color;
        }

        public string Name { get; set; }
        public string Color { get; set; }
        public int Value { get; set; }
    }
}