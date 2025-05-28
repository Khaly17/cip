using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Extensions
{
    public static class EnumerableExtensions
    {
        public static List<T> CloneList<T>(this IEnumerable<T> list) where T : BaseModel
        {
            return list.Select(p => (T) p.Clone()).ToList();
        }
        public static List<ApplicationUser> CloneList(this IEnumerable<ApplicationUser> list)
        {
            return list.Select(p => (ApplicationUser) p.Clone()).ToList();
        }
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }
        public static DateTime StartOfMonth(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            var date = dt.AddDays(-1 * diff).Date;
            return new DateTime(date.Year, date.Month, 1);
        }
        public static int TotalWeeks(this TimeSpan dt)
        {
            return (int)(dt.TotalDays / 7);
        }
        public static int TotalMonths(this TimeSpan dt)
        {
            return (int)(dt.TotalDays / 30);
        }
        public static DateTime StartOfDay(this DateTime dt)
        {
            return dt.Date;
        }
    }

}