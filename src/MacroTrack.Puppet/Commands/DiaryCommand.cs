// we wil rewrite this this using dictionary of delegates.

using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public class DiaryCommand : ICommand
{
    private readonly DiaryService _service;

    public string Name => "diary";
    public IReadOnlyList<string> Aliases => new[] { "d", "journal" };
    public string Description => "Commands for interfacing with Diary.";
    public string Usage => "doary [add/delete/edit/list] (other arguments)";
    public string LongHelp => @"Command set for interfacing with Diary. Subcommands are:
     - diary add [body]                      - Adds new diary entry.
     - diary delete [id]                     - Deletes the selected entry. No (Y/N) confirmation. diary delete last deletes the last entry (one with highest id).
     - diary edit [id] (body) (edit notes)   - Edits the selected entry. Indicate start and end of each with double quotations. If you want to make no edits to the body, leave the first argument blank (one space, doesn't work without the space), otherwise, the body of that entry will be replaced with the first argument. Edit notes are appended after the timestamp.    
     - list (start time) (end time)          - Lists out every Diary entry, or every entry since start date if that is specified, or every entry between start date and end date if both arguments present. Use format 'YYYY/MM/DD HH:MM:DD' in doble quotes by default.";

    public DiaryCommand(DiaryService service)
    {
        _service = service;
    }

    public string Execute(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return ("Not enough arguments, usage: " + Usage);
        switch (args[0])
        {
            case "add": return Add(args.Skip(1).ToList());
            case "delete": return Delete(args.Skip(1).ToList());
            case "edit": return Edit(args.Skip(1).ToList());
            case "list": return List(args.Skip(1).ToList());
            case "help": return LongHelp;
            default: return $"Unknown argument 'diary {args[0]}', type 'help diary'.";
        }
    }

    private string Add(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return "Not enough arguments, usage: diary add [body].";
        try {return $"Entry added: {PrintEntry(_service.AddEntry(args[0]))}";}
        catch (Exception ex) {return $"Puppet.Commands.DiaryCommand.Add(): Error, could not find new entry after adding it: {ex.Message}";}
    }

    private string Delete(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return ("Not enough arugments, usage: diary delete [ID]/\"last\".");
        if (args[0] == "last")
        {
            try {return $"Deleted last entry: {PrintEntry(_service.DeleteLast())}";}
            catch (Exception ex) {return "Puppet.Commands.DiaryCommand.Delete(): Error deleting last entry: " + ex.Message;}
        }
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer";
        try {return $"Deleted entry: {PrintEntry(_service.DeleteEntry(id))}";}
        catch (Exception ex) {return $"Puppet.Commands.DiaryCommand.Delete(): Error deleting entry {id}: {ex.Message}\nThere's probably no entry with ID [{id}]. Try 'diary list' to see list of entries & IDs.";}
    }

    private string Edit(IReadOnlyList<string> args)
    {
        if (args.Count < 2) return ("Not enough arguments, usage: diary edit [ID] \"(body)\" \"(edit note)\"");
        if (!int.TryParse(args[0], out int id)) return $"Invalid ID '{args[0]}', must be an integer.";
        if (_service.GetEntry(id) == null) return $"Invalid ID '{args[0]}', entry not found, type 'diary list' to see IDs";
        if (args.Count == 2)
        {
            if (string.IsNullOrWhiteSpace(args[1])) return "Body empty, no edit notes: No edits made.";
            try
            {
                var oldEntry = _service.GetEntry(id);
                var newEntry = _service.EditEntry(id, args[2], "No notes");
                return $"Edited entry: \n{PrintEntry(oldEntry)}\n\nTo:\n\n{PrintEntry(newEntry)}";
            }
            catch (Exception ex) {return $"Puppet.Commands.DiaryCommand.Edit(): Error editing entry [{id}]: {ex.Message}\nThere's probably no entry with ID[{id}]. Try 'diary list' to see list of all entries & IDs.";}
        }
        try 
        {
            var oldEntry = _service.GetEntry(id);
            var newEntry = _service.EditEntry(id, string.IsNullOrWhiteSpace(args[1])? oldEntry.Body : args[1], string.IsNullOrWhiteSpace(args[2])? "No notes" : args[2]);
            return $"Edited entry: \n{PrintEntry(oldEntry)}\n\nTo:\n\n{PrintEntry(newEntry)}";
        }
        catch (Exception ex) {return $"Puppet.Commands.DiaryCommand.Edit(): Error editing entry [{id}]: {ex.Message}\nThere's probably no entry with ID[{id}]. Try 'diary list' to see list of all entries & IDs.";}
    }

    private string List(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return PrintList(_service.GetAll());
        if (!DateTime.TryParse(args[0], out DateTime start)) return $"Cannot parse Date/Time '{args[0]}'.\nTry formatting as:\n - \"YYYY/MM/DD HH:MM:SS\"\n - \"YYYY/MM/DD\"\n - \"HH:MM:SS\"";
        if (args.Count == 1) return PrintList(_service.FromTimes(start, DateTime.Now));
        if (!DateTime.TryParse(args[1], out DateTime end)) return $"Cannot parse Date/Time '{args[1]}'.\nTry formatting as:\n - \"YYYY/MM/DD HH:MM:SS\"\n - \"YYYY/MM/DD\"\n - \"HH:MM:SS\"";
        return PrintList(_service.FromTimes(start, end));
    }

    private string PrintEntry(DiaryEntry entry)
    {
        return $"[{entry.Id}] {entry.Time:yyyy/MM/dd HH:mm:ss}\n{entry.Body}";
    }

    private string PrintList(List<DiaryEntry> DiaryList)
    {
        string listString = "";
        foreach (var entry in DiaryList)
        {
            listString = $"{listString}{PrintEntry(entry)}\n\n";
        }
        return listString;
    }
}