namespace MacroTrack.Core.Models
{
    /// <summary>
    /// Originally done in Puppet2, but is useful enough to put into Core as well. Will need it for "Print" functions in the models, hence it being in "Models" namespace. If we make more of this sort of thing it can have its own namespace but that's probably unlikely.
    /// </summary>
    public static class TruncationHelper
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
            length = Math.Abs(length);
            if (length <= truncateString.Length) return truncateString[..(Math.Max(0, length))];
            if (input.Length <= length) return input;
            return input[..(length - truncateString.Length)] + truncateString;
        }

        public static string? TruncateNullable(this string? input, int length, string truncateString = "…")
        {
            if (input is null) return null;
            return input.Truncate(length, truncateString);
        }

        public static string ToStringTruncate(this double input, int length, string format = "0.#", string prefix = "", string suffix = "", string truncateString = "…")
        {
            string output = prefix + input.ToString(format) + suffix;
            output = output.Truncate(length, truncateString);
            return output;
        }
    }
}
