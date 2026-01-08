LiveSplit.FFX 
==============

LiveSplit.FFX is a [LiveSplit](http://livesplit.org/) component for Final Fantasy X HD-Remaster. The plugin is based off [LiveSplit.Dishonored](https://github.com/fatalis/LiveSplit.Dishonored).

Current Features
----------------
  * Removes Loading times
  * Auto starts the timer according to the [leaderboard rules](http://speedrun.com/ffx)
  * Auto resets the timer on the music selection screen
  * Auto Splitting/End
  * Encounter counter, accessible via the Layout Editor -> Information -> FFX Display

Requirements
------------
* LiveSplit 1.6.3 or newer
* PC Category only

Install
-------
Right click on your splits and click 'Edit Splits'. Click on the 'Activate' button and check any options/splits you desire (see below for details of each option). Hit OK to apply these settings.

To activate the Encounter Counter, right click on your splits and click 'Edit Layout'. Hit the add '+' button, and click on Information->FFX Display. This will add a basic component to your splits, which will keep track of all the encounters in a run.

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

#### Encounter Count
Available as a separate LiveSplit component. Automatically tracks your encounter count throughout a run. Includes all bosses and compulsory encounters. The text color and background color can be changed via the components layout settings.
