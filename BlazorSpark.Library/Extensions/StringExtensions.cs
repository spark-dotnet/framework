using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Extensions
{
	public static class StringExtensions
	{
		public static string ToSlug(this string input)
		{
			string str = input.RemoveDiacritics().ToLower();
			// invalid chars
			str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
			// convert multiple spaces into one space
			str = Regex.Replace(str, @"\s+", " ").Trim();
			// cut and trim
			str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
			// hyphens
			str = Regex.Replace(str, @"\s", "-");
			return str;
		}

		private static string RemoveDiacritics(this string str)
		{
			var normalizedString = str.Normalize(NormalizationForm.FormD);
			var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

			for (int i = 0; i < normalizedString.Length; i++)
			{
				char c = normalizedString[i];
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}

			return stringBuilder
				.ToString()
				.Normalize(NormalizationForm.FormC);
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
