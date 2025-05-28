using System.Collections.Generic;

namespace Gefco.CipQuai.Web.Models
{
    public class CategoryDataValue
    {
        public CategoryDataValue(string name, List<DataValue> values)
        {
            CategoryName = name;
            Values = values;
        }

        public string CategoryName { get; set; }
        public List<DataValue> Values { get; set; }
    }
}