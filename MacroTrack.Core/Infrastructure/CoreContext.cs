using MacroTrack.Core.Logging;
using MacroTrack.Core.Repositories;
using MacroTrack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroTrack.Core.Infrastructure
{
    public sealed record CoreContext(
        IMTLogger Logger    
    );  
}
