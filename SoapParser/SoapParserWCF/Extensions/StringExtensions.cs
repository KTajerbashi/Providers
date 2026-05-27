using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoapParserWCF.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Checks if a string is not null, not empty, and not whitespace
        /// </summary>
        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Checks if a string is null, empty, or whitespace
        /// </summary>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Returns null if string is empty/whitespace, otherwise returns the original string
        /// </summary>
        public static string NullIfEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        /// <summary>
        /// Returns a default value if string is empty/whitespace
        /// </summary>
        public static string DefaultIfEmpty(this string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        /// <summary>
        /// Truncates string to maximum length, with optional ellipsis
        /// </summary>
        public static string Truncate(this string value, int maxLength, bool addEllipsis = false)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
                return value;

            string truncated = value.Substring(0, maxLength);
            return addEllipsis ? truncated + "..." : truncated;
        }

        /// <summary>
        /// Removes all whitespace characters from the string
        /// </summary>
        public static string RemoveWhitespace(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return new string(value.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        /// <summary>
        /// Converts string to Title Case (First Letter Of Each Word Capitalized)
        /// </summary>
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.TextInfo.ToTitleCase(value.ToLower());
        }

        /// <summary>
        /// Reverses the string
        /// </summary>
        public static string Reverse(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Checks if string contains any of the specified values (case-insensitive by default)
        /// </summary>
        public static bool ContainsAny(this string value, StringComparison comparisonType, params string[] searchStrings)
        {
            if (string.IsNullOrWhiteSpace(value) || searchStrings == null || searchStrings.Length == 0)
                return false;

            return searchStrings.Any(s => value.IndexOf(s, comparisonType) >= 0);
        }

        /// <summary>
        /// Checks if string contains any of the specified values (case-insensitive)
        /// </summary>
        public static bool ContainsAny(this string value, params string[] searchStrings)
        {
            return ContainsAny(value, StringComparison.OrdinalIgnoreCase, searchStrings);
        }

        /// <summary>
        /// Checks if string contains all of the specified values
        /// </summary>
        public static bool ContainsAll(this string value, StringComparison comparisonType, params string[] searchStrings)
        {
            if (string.IsNullOrWhiteSpace(value) || searchStrings == null || searchStrings.Length == 0)
                return false;

            return searchStrings.All(s => value.IndexOf(s, comparisonType) >= 0);
        }

        /// <summary>
        /// Extracts substring between two markers
        /// </summary>
        public static string Between(this string value, string startMarker, string endMarker)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            int startIndex = value.IndexOf(startMarker);
            if (startIndex == -1) return null;

            startIndex += startMarker.Length;
            int endIndex = value.IndexOf(endMarker, startIndex);
            if (endIndex == -1) return null;

            return value.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Ensures string starts with specified prefix
        /// </summary>
        public static string EnsureStartsWith(this string value, string prefix, StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrWhiteSpace(value))
                return prefix;

            return value.StartsWith(prefix, comparison) ? value : prefix + value;
        }

        /// <summary>
        /// Ensures string ends with specified suffix
        /// </summary>
        public static string EnsureEndsWith(this string value, string suffix, StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrWhiteSpace(value))
                return suffix;

            return value.EndsWith(suffix, comparison) ? value : value + suffix;
        }

        /// <summary>
        /// Removes specified prefix from string if present
        /// </summary>
        public static string RemovePrefix(this string value, string prefix, StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(prefix))
                return value;

            return value.StartsWith(prefix, comparison) ? value.Substring(prefix.Length) : value;
        }

        /// <summary>
        /// Removes specified suffix from string if present
        /// </summary>
        public static string RemoveSuffix(this string value, string suffix, StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(suffix))
                return value;

            return value.EndsWith(suffix, comparison)
                ? value.Substring(0, value.Length - suffix.Length)
                : value;
        }

        /// <summary>
        /// Checks if string matches any of the specified patterns (wildcard support: *, ?)
        /// </summary>
        public static bool MatchesAnyPattern(this string value, params string[] patterns)
        {
            if (string.IsNullOrWhiteSpace(value) || patterns == null || patterns.Length == 0)
                return false;

            return patterns.Any(pattern =>
            {
                string regexPattern = "^" + System.Text.RegularExpressions.Regex.Escape(pattern)
                    .Replace("\\*", ".*")
                    .Replace("\\?", ".") + "$";
                return System.Text.RegularExpressions.Regex.IsMatch(value, regexPattern,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            });
        }

        /// <summary>
        /// Converts string to slug (URL-friendly format)
        /// </summary>
        public static string ToSlug(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            // Remove special characters and replace spaces with hyphens
            string slug = value.Trim().ToLower();
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[\s-]+", " ");
            slug = slug.Trim().Replace(' ', '-');

            return slug;
        }

        /// <summary>
        /// Returns first N characters of string
        /// </summary>
        public static string First(this string value, int count)
        {
            if (string.IsNullOrWhiteSpace(value) || count <= 0)
                return string.Empty;

            return value.Length <= count ? value : value.Substring(0, count);
        }

        /// <summary>
        /// Returns last N characters of string
        /// </summary>
        public static string Last(this string value, int count)
        {
            if (string.IsNullOrWhiteSpace(value) || count <= 0)
                return string.Empty;

            return value.Length <= count ? value : value.Substring(value.Length - count);
        }
    }
}
