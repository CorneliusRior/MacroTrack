using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT5toMTScript
{
    public sealed record DiaryAddPayload(string Body, DateTime? Time);
    
    
    public sealed record GoalActivationPayload(int Id, DateTime? Date);
    
    
    
    public sealed record CheatSetPayload(DateTime Date, bool IsCheatDay);
    public sealed record WeightAddPayload(double Weight, DateTime? Time);

    public sealed record ScriptMetaData(
        string Format,
        string Name,
        string Author,
        DateTime Created)
    {
        public string PrintInfo() => $"{Name}.\nAuthor: {Author}, Created: {Created.ToString("G")}\nFormat: '{Format}'.";
    }
}
