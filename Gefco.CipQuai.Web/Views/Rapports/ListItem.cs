using System.Collections.Generic;

namespace Gefco.CipQuai.Web.Views
{
    public class ListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public static List<ListItem> Months => new List<ListItem>()
        {
            new ListItem("1", "Janvier"),
            new ListItem("2", "Février"),
            new ListItem("3", "Mars"),
            new ListItem("4", "Avril"),
            new ListItem("5", "Mai"),
            new ListItem("6", "Juin"),
            new ListItem("7", "Juillet"),
            new ListItem("8", "Août"),
            new ListItem("9", "Septembre"),
            new ListItem("10", "Octobre"),
            new ListItem("11", "Novembre"),
            new ListItem("12", "Décembre"),
        };

        public ListItem(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}