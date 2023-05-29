using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorSpark.Console.Shared
{
    public static class StringExtensions
    {
        public static string PascalToKebabCase(this string value)
        {
            return Regex.Replace(value, @"(?<=[a-z])(?=[A-Z])", "-").ToLower();
        }
    }
}
