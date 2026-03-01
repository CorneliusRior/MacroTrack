namespace MacroTrack.Puppet2
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes new paragrams, truncates the string down to a single line of a given length, finished off with a truncateString, by default "…".
        /// </summary>
        /// <param name="length">Length you want your string to be cut down to.</param>
        /// <param name="truncateString">String appended to the end of the string if it exceeds Length. "…" by default.</param>
        public static string Truncate(this string input, int length, string truncateString = "…")
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            input = input.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
            if (length <= truncateString.Length) return truncateString[..(Math.Max(0, length))];
            if (input.Length <= length) return input;
            return input[..(length - truncateString.Length)] + truncateString;
        }
    }
}
