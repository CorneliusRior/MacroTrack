using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2
{
    /// <summary>
    /// Some extensions to a list of strings for Command Arguments. Format for naming them: if the type starts with lowercase, start with uppercase, if it starts with uppercase (like DateTime), start with lower case. You can change this later if you don't like it.
    /// </summary>
    public static class CommandArgumentExtensions
    {
        /// <summary>
        /// Gets the specified string, throws an exception if not present.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        public static string String(this IReadOnlyList<string> args, int index, string name)
        {
            if (index >= args.Count) throw new PuppetUserException($"Not enough arguments, missing string '{name}'");
            return args[index];
        }

        /// <summary>
        /// Gets the specified string, or returns the default if not present. "Fallback" is not nullable, use StringOrNull if you want nullable return.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        /// <param name="fallBack">Default return value</param>
        public static string StringOr(this IReadOnlyList<string> args, int index, string name, string fallBack)
        {
            if (index >= args.Count) return fallBack;
            return args[index];
        }

        /// <summary>
        /// Gets the specified string, or returns null.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        public static string? StringOrNull(this IReadOnlyList<string> args, int index, string name)
        {
            if (index >= args.Count) return null;
            if (string.IsNullOrWhiteSpace(args[index])) return null;
            return args[index];
        }

        /// <summary>
        /// Gets the specified double from a list of arguments, throws an exception if not present or cannot parse.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        public static double Double(this IReadOnlyList<string> args, int index, string name)
        {
            if (index >= args.Count) throw new PuppetUserException($"Not enough arguments, missing double '{name}'");
            if (!double.TryParse(args[index], out double v)) throw new PuppetUserException($"Cannot parse double '{name}': '{args[index]}'.");
            return v;
        }

        /// <summary>
        /// Gets the specified double from a list of arguments, returns fallback if not present, throws exception if cannot parse.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        /// <param name="fallBack">Default return value</param>
        public static double DoubleOr(this IReadOnlyList<string> args, int index, string name, double fallBack)
        {
            if (index >= args.Count) return fallBack;
            if (!double.TryParse(args[index], out double v)) throw new PuppetUserException($"Cannot parse double '{name}': '{args[index]}'.");
            return v;
        }

        /// <summary>
        /// Gets the specified double from a list of arguments, returns null if not present, throws exception if cannot parse.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        /// <returns></returns>
        /// <exception cref="PuppetUserException"></exception>
        public static double? DoubleOrNull(this IReadOnlyList<string> args, int index, string name)
        {
            if (index >= args.Count) return null;
            if (!double.TryParse(args[index], out double v)) throw new PuppetUserException($"Cannot parse double '{name}': '{args[index]}'.");
            return v;
        }

        /// <summary>
        /// Gets the specified integer, throws exception if not present or cannot parse.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        public static int Int(this IReadOnlyList<string> args, int index, string name)
        {
            if (index >= args.Count) throw new PuppetUserException($"Not enough arguments, missing int '{name}'");
            if (!int.TryParse(args[index], out int v)) throw new PuppetUserException($"Cannot parse int '{name}': .{args[index]}'.");
            return v;
        }

        /// <summary>
        /// Gets the specified integer, returns fallback if not oresent, throws exception if cannot parse.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        /// <param name="fallBack">Default return value</param>
        public static int IntOr(this IReadOnlyList<string> args, int index, string name, int fallBack)
        {
            if (index >= args.Count) return fallBack;
            if (!int.TryParse(args[index], out int v)) throw new PuppetUserException($"Cannot parse int '{name}': .{args[index]}'.");
            return v;
        }

        /// <summary>
        /// Gets the specified integer, returns null if not present, throws exception if cannot parse.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        public static int? IntOrNull(this IReadOnlyList<string> args, int index, string name)
        {
            if (index >= args.Count) return null;
            if (!int.TryParse(args[index], out int v)) throw new PuppetUserException($"Cannot parse int '{name}': .{args[index]}'.");
            return v;
        }

        /// <summary>
        /// Returns specified DateTime from a list of arguments, throws an exception if not present or cannot parse.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        public static DateTime dateTime(this IReadOnlyList<string> args, int index, string name)
        {
            if (index >= args.Count) throw new PuppetUserException($"Not enough arguments, missing DateTme '{name}'");
            if (!DateTime.TryParse(args[index], out DateTime v)) throw new PuppetUserException($"Cannot Parse DateTIme '{name}': '{args[index]}'.");
            return v;
        }

        /// <summary>
        /// Returns specified DateTime from a list of arguments, returns fallabck if not present and exception if cannot parse.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        /// <param name="fallBack">Default return value</param>
        public static DateTime dateTimeOr(this IReadOnlyList<string> args, int index, string name, DateTime fallback)
        {
            if (index > args.Count) return fallback;
            if (!DateTime.TryParse(args[index], out DateTime v)) throw new PuppetUserException($"Cannot parse DateTime '{name}': '{args[index]}'.");
            return v;
        }

        /// <summary>
        /// Returns specified DateTime from a list of arguments, returns null if not present and exception if cannot parse.
        /// </summary>
        /// <param name="index">Position of desired argument in "args"</param>
        /// <param name="name">Assigned name for string, included in exception message</param>
        public static DateTime? dateTimeOrNull(this IReadOnlyList<string> args, int index, string name)
        {
            if (index > args.Count) return null;
            if (!DateTime.TryParse(args[index], out DateTime v)) throw new PuppetUserException($"Cannot parse DateTime '{name}': '{args[index]}'.");
            return v;
        }
    }
}
