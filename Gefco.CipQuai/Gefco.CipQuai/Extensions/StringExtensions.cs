using System;
using System.Collections.Generic;
using System.Linq;

namespace Gefco.CipQuai.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        public static bool IsEmptyId(this string str)
        {
            if (str == null)
                return true;
            return str == Guid.Empty.ToString();
        }
    }
}