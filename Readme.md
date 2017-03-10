LiveSplit.FFX v 0.3
===================

LiveSplit.FFX is a WIP [LiveSplit](http://livesplit.org/) component for Final Fantasy X HD-Remaster. The plugin is based off [LiveSplit.Dishonored](https://github.com/fatalis/LiveSplit.Dishonored).

Current Features
----------------
  * Removes Loading times
  * Auto starts the timer according to the [leaderboard rules](http://speedrun.com/ffx)
  * Auto resets the timer on the music selection screen
  
TODO List
---------
  * Auto Splitting
  * Custom Splits

Requirements
------------
* LiveSplit 1.6.3 or newer

Install
-------
Build the LiveSplit.FFX.dll component and place it in your LiveSplit Components directory. Add the autosplitter via the layout editor, where you can also find the settings.

The following settings are currently available:

#### Auto Start
Enabled by default, automatically starts the timer when confirming the music selection in the new game menu.

#### Legacy Splits (WIP) 
(Locked) Not yet implemented feature to automatically split for each commonly used split.

#### Auto Reset
Disabled by default, automatically resets the timer if you enter the music selection screen in the new game menu.

#### Remove Loads 
Disabled by default, removes loading times from the ingame timer comparison.