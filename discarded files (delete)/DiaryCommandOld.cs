using MacroTrack.Core.Models;
using MacroTrack.Core.Services;

namespace MacroTrack.Puppet.Commands;

public class DiaryCommandOld : ICommand
{
    private readonly DiaryService _service;

    public string Name => "diary";
    public IReadOnlyList<string> Aliases => new[] { "d", "journal" };
    public string Description => "Commands for interfacing with Diary.";
    public string Usage => "doary [add/delete/edit/list] (other arguments)";
    public string LongHelp => @"Command set for interfacing with Diary. Subcommands are:
     - diary add [body]                      - Adds new diary entry.
     - diary delete [id]                     - Deletes the selected entry. No (Y/N) confirmation.
     - diary edit [id] (body) (edit notes)   - Edits the selected entry. Indicate start and end of each with double quotations. If you want to make no edits to the body, leave the first argument blank, otherwise, the body of that entry will be replaced with the first argument. Edit notes are appended after the timestamp.    
     - list (start time) (end time)          - Lists out every Diary entry, or every entry since start date if that is specified, or every entry between start date and end date if both arguments present. Use format 'YYYY/MM/DD HH:MM:DD' in doble quotes by default.";

    public DiaryCommandOld(DiaryService service)
    {
        _service = service;
    }

    public string Execute(IReadOnlyList<string> args)
    {
        if (args.Count == 0)
        {
            return ("Not enough arguments, usage: " + Usage);
        }

        switch(args[0])
        {
            case "add":
                if (args.Count == 1)
                {
                    return ("Not enough arguments, usage: diary add \"body\"");
                }

                try
                {
                    return $"Entry added: {PrintEntry(_service.AddEntry(args[1]))}";
                }
                catch
                {
                    return $"Error, could not find the new entry after we added it. Connection problem probably, I don't know, this is just here to make the program feel better.";
                }

            case "delete":
                if (args.Count == 1)
                {
                    return "Not enough arguments, usage: diary delete [ID]";
                }
                
                if (!int.TryParse(args[1], out int id))
                {
                    return $"Invalid ID '{id}', must be an integer.";
                }

                try
                {
                    return $"Entry deleted: {PrintEntry(_service.DeleteEntry(id))}";
                }
                catch
                {
                    return $"Error: diary entry not found, there probably isn't an entry for [{id}]";
                }
                

            case "edit":
                if (args.Count < 3)
                {
                    return "Not enough arguments, usage: diary edit [id] \"(body)\" \"(Edit Note)\"";
                }
                if (args.Count == 3)
                {
                    if (!int.TryParse(args[1], out int editId))
                    {
                        return $"Invalid ID '{args[1]}', must be an integer.";
                    }

                    if (string.IsNullOrWhiteSpace(args[2]))
                    {
                        return "Body empty, no edit notes: no edits made.";
                    }

                    try
                    {
                        var oldEntry = _service.GetEntry(editId);
                        var newEntry = _service.EditEntry(editId, args[2], "No notes");
                        return $"Edited entry:\n{PrintEntry(oldEntry)}\n\nTo:\n\n{PrintEntry(newEntry)}";
                    }
                    catch
                    {
                        return $"Error: diary entry not fond, there probably isn't an entry for [{editId}]";
                    }
                }

                if (args.Count > 3)
                {
                    if (!int.TryParse(args[1], out int editId))
                    {
                        return $"Invalid ID '{args[1]}', must be an integer.";
                    }

                    if (string.IsNullOrWhiteSpace(args[2]))
                    {
                        if (string.IsNullOrWhiteSpace(args[3]))
                        {
                            return "Body empty, Edit notes empty: No edits made";
                        }

                        try
                        {
                            var oldEntry = _service.GetEntry(editId);
                            var newEntry = _service.EditEntry(editId, oldEntry.Body, args[3]);
                            return $"Edited entry:\n{PrintEntry(oldEntry)}\n\nTo:\n\n{PrintEntry(newEntry)}";
                        }
                        catch
                        {
                            return $"Error: diary entry not fond, there probably isn't an entry for [{editId}]";
                        }
                    }

                    if (string.IsNullOrWhiteSpace(args[3]))
                    {
                        try
                        {
                            var oldEntry = _service.GetEntry(editId);
                            var newEntry = _service.EditEntry(editId, args[2], "No notes");
                            return $"Edited entry:\n{PrintEntry(oldEntry)}\n\nTo:\n\n{PrintEntry(newEntry)}";
                        }
                        catch
                        {
                            return $"Error: diary entry not fond, there probably isn't an entry for [{editId}]";
                        }
                    }
                    
                    try
                    {
                        var oldEntry = _service.GetEntry(editId);
                        var newEntry = _service.EditEntry(editId, args[2], args[3]);
                        return $"Edited entry:\n{PrintEntry(oldEntry)}\n\nTo:\n\n{PrintEntry(newEntry)}";
                    }
                    catch
                    {
                        return $"Error: diary entry not fond, there probably isn't an entry for [{editId}]";
                    }
                }

                return "";                

            case "list":
                if (args.Count == 1)
                {
                    return PrintList(_service.GetAll());
                }

                if (args.Count == 2)
                {
                    if (!DateTime.TryParse(args[1], out DateTime start))
                    {
                        return $"Cannot parse Date/Time '{args[1]}'.";
                    }
                    return PrintList(_service.FromTimes(start, DateTime.Now));
                }

                if (args.Count > 2)
                {
                    if (!DateTime.TryParse(args[1], out DateTime start))
                    {
                        return $"Cannot parse Date/Time '{args[1]}'.";
                    }

                    if (!DateTime.TryParse(args[2], out DateTime end))
                    {
                        return $"Cannot parse Date/Time '{args[2]}'.";
                    }

                    return PrintList(_service.FromTimes(start, end));
                }

                return "";

            default:
                return $"Unknown argument 'weight {args[0]}', type 'help diary'.";
        }
    }

    private string PrintEntry(DiaryEntry entry)
    {
        return $"   [{entry.Id}] {entry.Time}\n{entry.Body}";
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