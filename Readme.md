LiveSplit.FFX 
==============

LiveSplit.FFX is a [LiveSplit](http://livesplit.org/) component for Final Fantasy X HD-Remaster. The plugin is based off [LiveSplit.Dishonored](https://github.com/fatalis/LiveSplit.Dishonored).

Current Features
----------------
  * Removes Loading times
  * Auto starts the timer according to the [leaderboard rules](http://speedrun.com/ffx)
  * Auto resets the timer on the music selection screen
  * Auto Splitting/End
  * FFX Display, accessible via the Layout Editor

Requirements
------------
* LiveSplit 1.6.3 or newer
* PC Category only

Install
-------
Right click on your splits and click 'Edit Splits'. Click on the 'Activate' button and check any options/splits you desire (see below for details of each option). Hit OK to apply these settings.

To activate the FFX Display, right click on your splits and click 'Edit Layout'. Hit the add '+' button, and click on Information->FFX Display. This will add a basic component to your splits, which will keep track of the specified game value. Open the "Layout Settings" to select the value you want it to show.

Full instructions here - [Autosplitter/Load Remover/Encounter Count Guide](http://www.speedrun.com/ffx/guide/vnxps)

The following settings are currently available:

#### Start
Automatically starts the timer when confirming the music selection in the new game menu.

#### Split(s)
Automatically splits the selected splits.

#### Reset
Automatically resets the timer in the new game menu.

#### Remove Loads 
Removes loading times from the ingame timer comparison.

#### FFX Display
Available as a separate LiveSplit component. Automatically tracks the selected value, such as the number of encounters, Speed Spheres in inventory and more.
