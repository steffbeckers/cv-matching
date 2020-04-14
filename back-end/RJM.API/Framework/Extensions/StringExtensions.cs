using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RJM.API.Framework.Extensions
{
    public static class StringExtensions
    {
        public static string ToSlug(this string text)
        {
            // Validation
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            // Convert to lower case
            string slug = text.ToLower();

            // Remove invalid chars        
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Convert multiple spaces into one space   
            slug = Regex.Replace(slug, @"\s+", " ").Trim();

            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s", "-");

            return slug;
        }
    }
}
