using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Puppet2
{
    public sealed record PuppetResult(bool Success, string Output)
    {
        public static PuppetResult Ok(string output) => new(true, output);
        public static PuppetResult Fail(string output) => new(true, output);
    }
}
