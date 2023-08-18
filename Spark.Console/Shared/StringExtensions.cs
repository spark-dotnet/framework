using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spark.Console.Shared
{
    public static class StringExtensions
    {
        public static string PascalToKebabCase(this string value)
        {
            return Regex.Replace(value, @"(?<=[a-z])(?=[A-Z])", "-").ToLower();
        }

        /// <summary>
        /// Returns the string with the first letter set to uppercase.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUpperFirst(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.Length == 1)
            {
                return value.ToUpper();
            }

            char firstChar = char.ToUpper(value[0]);
            string restOfString = value[1..];
            return firstChar + restOfString;
        }
    }
}
