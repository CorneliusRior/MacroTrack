# MacroTrack

MacroTrack is a Desktop Application used to track Calories and Macronutrients. It contains a Food Log, Weight Log, Diary, Daily Task checklist, and graphing tools. The Core is written in C# and uses SQLite to administer the database.

## Stucture

MacroTrack consists of `MacroTrack.Core` and different applications of it. These are:

- `MarcoTrack.Puppet` A set of commands used to interact with Core, used by `MacroTrack.Cli`.
- `MacroTrack.Puppet2` A more advanced command system used to interact with Core, with scripting abilities, used by `MacroTrack.DashBoard`.
- `MacroTrack.BasicApp` A WinForms application of Core.
- `MacroTrack.AppLibrary` A WPF Library with UI Elements and logic necessary to make a WPF application of Core.
- `MacroTrack.Dashboard` An application of AppLibrary and Core.

## Core Features

## How to use
Download the latest **Dashboard** release at [https://github.com/CorneliusRior/MacroTrack/releases](https://github.com/CorneliusRior/MacroTrack/releases), extract the files in the downloaded .zip file to a folder of your choice, and run MTDashboard.exe.

The first thing to do once MacroTrack is up and running is to create a new **Goal**, which in both BasicApp and DashBoard is done by clicking the **New Goal** button at the top of the screen, and then setting it as the active goal by clicking the **Set Goal** button at the top of the screen. After that it is recommended that you go to **Food Entry > New Preset**, and add a number of food items that you will likely enter regularly. 

New food entries can be made by selecting a **Preset** from the drop-down menu, or by manually entering data in **Food Entry**.

## Project Status

This is a personal project. It is mostly complete, and it is unlikely that any alterations will be made that could cause problems with data. As of present, "DashBoard" is the only serious application, *Cli*, *Puppet* and *BasicApp* were development tools.

## Planned Features
- Analysis window.
	- Monthly report with statistics.
	- Graphing view to see long-term data.
	- Data analysis section.
- More applications of AppLibrary.
	- TabControl.
	- Light (Food entry only).
- "Manage Tasks" view.
- More themes and customisation options.

## Current Issues/Limitations
### AppLibrary:
- Custom DateTimePicker control behaves strangely and requires a rewrite.
- Custom SpinBox controls sometimes behaves strangely and requires review.
- Pressing "up" in WPF REPL before any commands have been run will crash the program.
- REPL Start-up banner, "MACROTRACK" banner crawl, appears strange perhaps due to it being designed in a different font.
- In settings, "Revert" and "Default" buttons do not work.