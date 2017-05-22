using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Saferide.Extensions
{
    public static class StringHelper
    {
        public static string ToUpperFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }


        public static string[] Spintax(this string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            string pattern = @"\[[^\[\]]*\]";
            Match m = Regex.Match(str, pattern);
            while (m.Success)
            {
                string seg = str.Substring(m.Index + 1, m.Length - 2);
                var choices = seg.Split('|');
                return choices;
            }
            return null;
        }
    }
}
