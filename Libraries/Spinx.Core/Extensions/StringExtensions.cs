using System;
using System.Text.RegularExpressions;

namespace Spinx.Core.Extensions
{
    public static class StringExtensions
    {
        public static string NullSafeToLower(this string s)
        {
            if (s == null)
                s = string.Empty;

            return s.ToLower();
        }

        public static string GenerateSlug(this string phrase)
        {
            var str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string TrimEnd(this string source, string value)
        {
            return !source.EndsWith(value)
                ? source
                : source.Remove(source.LastIndexOf(value, StringComparison.Ordinal));
        }

        public static string StripHtml(this string txt)
        {
            return !string.IsNullOrEmpty(txt) ? Regex.Replace(txt, "<(.|\\n)*?>", string.Empty) : string.Empty;
        }    
    }
}