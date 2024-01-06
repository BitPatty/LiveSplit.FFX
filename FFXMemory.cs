using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using StringList = System.Collections.Generic.List<string>;

namespace LiveSplit.FFX
{
  internal class FFXMemory : IDisposable
  {
    private readonly Dictionary<int, Tuple<int, SplitPair>> _LevelIDs = new Dictionary<int, Tuple<int, SplitPair>>
    {
        {0x2E, new Tuple<int, SplitPair>(0x12,  new SplitPair {SplitName = FFXComponent.KILIKA_WOODS,         SplitFlag = false})},     // FROM 18 - Kilika Woods                           TO  46 - Kilika - Residential Area
        {0xFA, new Tuple<int, SplitPair>(0xD4,  new SplitPair {SplitName = FFXComponent.BLITZBALL_COMPLETE,   SplitFlag = false})},     // FROM 212 - Stadium - Pool (Blitzball Exp Screen) TO  250 - Stadium - Stands
        {0x3A, new Tuple<int, SplitPair>(0x7F,  new SplitPair {SplitName = FFXComponent.MIIHEN_HIGHROAD,      SplitFlag = false})},     // FROM 127 - Highroad - Central                    TO  58 - Highroad - Agency, Front
        {0x4F, new Tuple<int, SplitPair>(0x3B,  new SplitPair {SplitName = FFXComponent.OLD_ROAD,             SplitFlag = false})},     // FROM 59 - Highroad - North End                   TO  79 - Mushroom Rock
        {0x4C, new Tuple<int, SplitPair>(0x5D,  new SplitPair {SplitName = FFXComponent.DJOSE_HIGHROAD,       SplitFlag = false})},     // FROM 93 - Djose Highroad                         TO  76 - Djose - Pilgrimage Road
        {0x69, new Tuple<int, SplitPair>(0x4B,  new SplitPair {SplitName = FFXComponent.MOONFLOW,             SplitFlag = false})},     // FROM 75 - Moonflow - South Bank Road             TO  105 - Moonflow - South Bank
        {0x8C, new Tuple<int, SplitPair>(0x87,  new SplitPair {SplitName = FFXComponent.GUADOSALAM,           SplitFlag = false})},     // FROM 135 - Guadosalam                            TO  140 - Thunder Plains - South
        {0x6E, new Tuple<int, SplitPair>(0xA2,  new SplitPair {SplitName = FFXComponent.THUNDER_PLAINS,       SplitFlag = false})},     // FROM 162 - Thunder Plains - North                TO  110 - Macalania Woods - South
        {0xDB, new Tuple<int, SplitPair>(0x118, new SplitPair {SplitName = FFXComponent.HOME,                 SplitFlag = false})},     // FROM 280 - Home - Main Corridor                  TO  219 - Home - Environment Controls
        {0xE2, new Tuple<int, SplitPair>(0x132, new SplitPair {SplitName = FFXComponent.BEVELLE_TRIALS,       SplitFlag = false})}      // FROM 306 - Bevelle - Trials                      TO  226 - Bevelle - Antechamber (Cutscene)
    };

    private readonly Dictionary<int, SplitPair> _ProgressionIDs = new Dictionary<int, SplitPair>
    {
        {0xF,   new SplitPair {SplitName = FFXComponent.SINSPAWN_AMMES,       SplitFlag = false}},     // 15 - Start Sinspawn Ammes battle
        {0x37,  new SplitPair {SplitName = FFXComponent.KLIKK,                SplitFlag = false}},     // 55 - Enter Klikk cutscene
        {0x4C,  new SplitPair {SplitName = FFXComponent.TROS,                 SplitFlag = false}},     // 76 - Enter Tros area after opening the door
        {0x77,  new SplitPair {SplitName = FFXComponent.PIRANHAS,             SplitFlag = false}},     // 119 - Wakka pushes Tidus into Lagoon
        {0xD6,  new SplitPair {SplitName = FFXComponent.KIMAHRI,              SplitFlag = false}},     // 214 - Gain control on Besaid Promontory before Kimahri
        {0x118, new SplitPair {SplitName = FFXComponent.SINSPAWN_ECHUILLES,   SplitFlag = false}},     // 280 - Enter underwater cutscene before Sinspawn Echuilles
        {0x142, new SplitPair {SplitName = FFXComponent.SINSPAWN_GENEAUX,     SplitFlag = false}},     // 322 - Gain control on Pilgrimage Road before Sinspawn Geneaux
        {0x1F6, new SplitPair {SplitName = FFXComponent.OBLITZERATOR,         SplitFlag = false}},     // 502 - Enter Cutscene after Blitzball cutscene ("Still in there!") before Oblitzerator
        {0x258, new SplitPair {SplitName = FFXComponent.GARUDA,               SplitFlag = false}},     // 600 - Auron FMV
        {0x343, new SplitPair {SplitName = FFXComponent.MUSHROOM_ROCK_ROAD,   SplitFlag = false}},     // 835 - Gain control on Ridge
        {0x361, new SplitPair {SplitName = FFXComponent.SINSPAWN_GUI,         SplitFlag = false}},     // 865 - Enter Sin destruction FMV before Sinspawn Gui 2
        {0x424, new SplitPair {SplitName = FFXComponent.EXTRACTOR,            SplitFlag = false}},     // 1060 - Enter Extractor cutscene
        {0x58C, new SplitPair {SplitName = FFXComponent.SPHERIMORPH,          SplitFlag = false}},     // 1420 - Gain control in Spring before Spherimorph
        {0x5CD, new SplitPair {SplitName = FFXComponent.CRAWLER,              SplitFlag = false}},     // 1485 - Enter Al Bhed cutscene on lake before Crawler
        {0x604, new SplitPair {SplitName = FFXComponent.SEYMOUR,              SplitFlag = false}},     // 1540 - Start Seymour battle
        {0x622, new SplitPair {SplitName = FFXComponent.WENDIGO,              SplitFlag = false}},     // 1570 - Gain control on Crevasse before Wendigo
        {0x7F8, new SplitPair {SplitName = FFXComponent.EVRAE,                SplitFlag = false}},     // 2040 - Gain control after Evrae/Auron cutscene before Evrae
        {0x820, new SplitPair {SplitName = FFXComponent.BEVELLE_GUARDS,       SplitFlag = false}},     // 2080 - Enter Guardian cutscene after defeating all Bevelle Guards
        {0x8AC, new SplitPair {SplitName = FFXComponent.ISAARU,               SplitFlag = false}},     // 2220 - Gain control in Via Purifico before Isaaru
        {0x8E8, new SplitPair {SplitName = FFXComponent.SEYMOUR_NATUS,        SplitFlag = false}},     // 2280 - Enter Seymour Natus Battle cutscene
        {0x9CE, new SplitPair {SplitName = FFXComponent.BIRAN_YENKE,          SplitFlag = false}},     // 2510 - Gain control after Kelk cutscene before Biran and Yenke
        {0x9E2, new SplitPair {SplitName = FFXComponent.SEYMOUR_FLUX,         SplitFlag = false}},     // 2530 - Gain control after singing before Seymour Flux
        {0xA19, new SplitPair {SplitName = FFXComponent.SANCTUARY_KEEPER,     SplitFlag = false}},     // 2585 - Gain control on Fayth cluster before Sanctuary Keeper
        {0xAFF, new SplitPair {SplitName = FFXComponent.YUNALESCA,            SplitFlag = false}},     // 2815 - Gain control in Great Hall before Yunalesca
        {0xC21, new SplitPair {SplitName = FFXComponent.SIN_CORE,             SplitFlag = false}},     // 3015 - Land on Sin cutscene before Sin Core
        {0xC3F, new SplitPair {SplitName = FFXComponent.OVERDRIVE_SIN,        SplitFlag = false}},     // 3135 - Gain control in corridor before Overdrive Sin
        {0xC85, new SplitPair {SplitName = FFXComponent.SEYMOUR_OMNIS,        SplitFlag = false}},     // 3205 - Gain control inside Sin before Seymour Omnis
        {0xCE4, new SplitPair {SplitName = FFXComponent.BRASKAS_FINAL_AEON,   SplitFlag = false}},     // 3300 - Start Braska's Final Aeon fight
        {0xD34, new SplitPair {SplitName = FFXComponent.YU_YEVON,             SplitFlag = false}}      // 3380 - Start Yu Yevon fight
    };

    private readonly List<string> _MiscellaneousIDs = new List<string>
    {
        FFXComponent.PIRANHAS,                // 73 - Post Piranha Cutscene
        FFXComponent.GARUDA,                  // 22 - Garuda battle
        FFXComponent.MUSHROOM_ROCK_ROAD,      // 940 - Kinoc Introduction
        FFXComponent.WENDIGO,                // Bogus Value (Not Required)
        FFXComponent.BEVELLE_GUARDS,         // Bogus Value (Not Required)
        FFXComponent.ISAARU,                  // 80 - Shiva summon
        FFXComponent.YU_YEVON,                // 7 - Yu Yevon battle
        FFXComponent.YU_YEVON                 // 28 - Yu Yevon battle
    };

    // Eventhandlers
    public event EventHandler OnLoadStarted;
    public event EventHandler OnLoadFinished;
    public event EventHandler OnMusicSelect;
    public event EventHandler OnMusicConfirm;
    public event EventHandler OnAreaCompleted;
    public event EventHandler OnBossDefeated;
    public event EventHandler<int> OnEncounter;

    // Vars
    private List<int> _ignorePIDs = new List<int> { } ;     // PIDs to ignore if necessary
    private FFXData _data;                                  // Memory Information
    private Process _process;                               // Process Information
    private bool _loadingStarted;                           // true if loading screen active
    private int _isaaruCounter = 0;                         // Boss counter for Isaaru split
    public StringList activatedSplits;

    // Add PIDs to ignore if necessary
    public FFXMemory()
    {
      activatedSplits = new StringList();
    }

    public void Update(FFXSettings Settings)
    {
      _process?.Refresh();

      // Try to rehook process if lost
      if (_process?.HasExited != false)
      {
        if (!TryGetGameProcess()) return;
        Settings.HasChanged = true;
      }

      if (Settings.HasChanged)
      {
        activatedSplits.Clear();
        activatedSplits.AddRange(Settings.GetSplits());
      }

      try
      {
        _data.UpdateAll(_process);
      }
      catch (Win32Exception)
      {
        return;
      }

      // Encounter Count update
      if (_data.EncounterCounter.Changed) OnEncounter?.Invoke(this, _data.EncounterCounter.Current);

      #region SPLITS

      // Area splits
      if (_data.CurrentLevel.Changed && _LevelIDs.ContainsKey(_data.CurrentLevel.Current))
      {
        Tuple<int, SplitPair> tuple = _LevelIDs[_data.CurrentLevel.Current];
        int previousLevelID = tuple.Item1;
        SplitPair splitPair = tuple.Item2;

        if (activatedSplits.Contains(splitPair.SplitName) && !splitPair.SplitFlag && previousLevelID == _data.CurrentLevel.Old)
        {
          OnAreaCompleted?.Invoke(this, EventArgs.Empty);
          splitPair.SplitFlag = true;
          _LevelIDs[_data.CurrentLevel.Current] = new Tuple<int, SplitPair>(previousLevelID, splitPair);
        }
      }

      // Boss / Enemy splits
      if (_data.BattleState.Changed && _ProgressionIDs.ContainsKey(_data.StoryProgression.Current))
      {
        // When battle state changes or for special case Bevelle Guards, when story progression changes as that is after the battle state has changed
        bool canSplit = false;
        SplitPair splitPair = _ProgressionIDs[_data.StoryProgression.Current];
        int battleState = _data.BattleState.Current;
        string splitName = splitPair.SplitName;

        //Normal Split
        if (activatedSplits.Contains(splitName) && !splitPair.SplitFlag && battleState == (int)BattleState.Over)
        {
          canSplit = true;
        }
        else if (activatedSplits.Contains(splitName) && !splitPair.SplitFlag && battleState == (int)BattleState.Fanfare)
        {
          //Special case for 'Klikk' and 'BiranYenke' splits; these battles end with the 'victory fanfare' which sets the battleState to 66058
          if (_data.StoryProgression.Current == 55 || _data.StoryProgression.Current == 2510)
            canSplit = true;
        }

        // These splits are checked in _MiscellaneousIds;
        if (_MiscellaneousIDs.Contains(splitName)) canSplit = false;

        if (canSplit)
        {
          OnBossDefeated?.Invoke(this, EventArgs.Empty);
          splitPair.SplitFlag = true;
          _ProgressionIDs[_data.StoryProgression.Current] = splitPair;
        }
      }

      // Misc splits
      if (_ProgressionIDs.ContainsKey(_data.StoryProgression.Current))
      {
        bool canSplit = false;
        SplitPair splitPair = _ProgressionIDs[_data.StoryProgression.Current];
        if (activatedSplits.Contains(splitPair.SplitName) && !splitPair.SplitFlag)
        {
          // Piranhas
          if
          (
            (_data.StoryProgression.Current == 119)
            && (_data.CutsceneType.Current == 73)
          )
          {
            canSplit = true;
          }
          // Garuda
          else if
          (
            (_data.StoryProgression.Current == 600)
            && (_data.EncounterMapID.Current == 17)
            && (_data.EncounterFormationID1.Current == 0)
            && (_data.EncounterFormationID2.Current == 1)
            && (_data.BattleState.Current == 522)
            && (_data.BattleState.Old == 10)
          )
          {
            canSplit = true;
          }
          // Mushroom Rock Road
          else if
          (
            (_data.StoryProgression.Current == 835)
            && (_data.CutsceneType.Current == 940)
          )
          {
            canSplit = true;
          }
          // Wendigo
          else if
          (
            (_data.StoryProgression.Current == 1570)
            && (_data.EncounterMapID.Current == 44)
            && (_data.EncounterFormationID1.Current == 0)
            && (_data.EncounterFormationID2.Current == 1)
            && (_data.BattleState.Current == 522)
            && (_data.BattleState.Old == 10)
          )
          {
            canSplit = true;
          }
          // Bevelle Guards
          else if
          (
            (_data.StoryProgression.Current == 2080)
            && (_data.EncounterMapID.Current == 53)
            && (_data.EncounterFormationID1.Current == 0)
            && (_data.EncounterFormationID2.Current == 2)
            && (_data.BattleState.Current == 522)
            && (_data.BattleState.Old == 10)
          )
          {
            canSplit = true;
          }
        }

        // Split
        if (canSplit)
        {
          OnAreaCompleted?.Invoke(this, EventArgs.Empty);
          splitPair.SplitFlag = true;
          _ProgressionIDs[_data.StoryProgression.Current] = splitPair;
        }
      }

      // Special Isaaru split
      if (_data.StoryProgression.Current == 2220 && _data.BattleState.Changed && _data.BattleState.Current == (int)BattleState.Over)
      {
        SplitPair splitPair = _ProgressionIDs[_data.StoryProgression.Current];
        ++_isaaruCounter;

        if (activatedSplits.Contains(splitPair.SplitName) && !splitPair.SplitFlag && _isaaruCounter == 3 && !splitPair.SplitFlag)
        {
          OnAreaCompleted?.Invoke(this, EventArgs.Empty);
          splitPair.SplitFlag = true;
          _ProgressionIDs[_data.StoryProgression.Current] = splitPair;
        }
      }

      // Special Yu Yevon Split
      if (_data.StoryProgression.Current == 3380 && _data.YuYevon.Changed && _data.YuYevon.Current == 1)
      {
        SplitPair splitPair = _ProgressionIDs[_data.StoryProgression.Current];
        bool canSplit = false;

        if (activatedSplits.Contains(splitPair.SplitName) && !splitPair.SplitFlag)
        {
          try
          {
            if (_data.HPEnemyA.Current == 0) canSplit = true;    // Yu Yevon
          }
          catch (Exception)
          {
            canSplit = false;
          }

          // Split
          if (canSplit)
          {
            OnAreaCompleted?.Invoke(this, EventArgs.Empty);
            splitPair.SplitFlag = true;
            _ProgressionIDs[_data.StoryProgression.Current] = splitPair;
          }
        }
      }

      // Main Menu
      if (_data.CurrentLevel.Current == 23)
      {
        // Reset _isaaruCounter if the main menu is entered (in case of GO on Isaaru or game crash)
        _isaaruCounter = 0;

        // Reset timer before starting a new game (on music selection screen)
        if (_data.SelectScreen.Current == 6) OnMusicSelect?.Invoke(this, EventArgs.Empty);

        // Start timer when starting a new game (on a press if on sound confirmation screen and cursor on "Yes")
        if ((_data.SelectScreen.Current == 7
            || _data.SelectScreen.Current == 8)
            && ((_data.CursorPosition.Current >> 16) & 0xFF) == 0
            && _data.Input.Current == 32
           )
        {
          OnMusicConfirm?.Invoke(this, EventArgs.Empty);
        }
      }

      #endregion SPLITS

      #region LOADREMOVER

      // Loading screen in/out
      if (_data.IsLoading.Changed)
      {
        // Pause Timer if Loading Screen active
        if (_data.IsLoading.Current == 2 && !_loadingStarted)
        {
          _loadingStarted = true;
          OnLoadStarted?.Invoke(this, EventArgs.Empty);
        }
        else
        {
          // Resume timer if loading screen no longer active
          if (_loadingStarted)
          {
            _loadingStarted = false;
            OnLoadFinished?.Invoke(this, EventArgs.Empty);
          }
        }
      }
    }

    #endregion LOADREMOVER

    #region PROCESS

    /// <summary>
    /// Tries to hook the games process
    /// </summary>
    /// <returns>Returns true if the hooking succeeds</returns>
    private bool TryGetGameProcess()
    {
      // Dispose previous process if necessary
      _process?.Dispose();
      _process = null;

      try
      {
        Process game = GetProcess("ffx");
        GameVersion version;

        if (game == null)
        {
          return false;
        }

        if (_ignorePIDs.Contains(game.Id))
        {
          return false;
        }  

        long baseAddress = game.MainModuleWow64Safe().BaseAddress.ToInt64();
        long entryPointAddress = game.MainModuleWow64Safe().EntryPointAddress.ToInt64();
        long relativeEntryPointAddress = entryPointAddress - baseAddress;

        if (relativeEntryPointAddress == (long)ExpectedEntryPoints.v1)
        {
          version = GameVersion.v1;
        }
        else
        {
          _ignorePIDs.Add(game.Id);
          MessageBox.Show("Unexpected game version. Final Fantasy X 1.0.0 is required. Try to restart the game.", "LiveSplit.FFX", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }

        version = GameVersion.v1;

        _data = new FFXData(version, game.MainModuleWow64Safe().BaseAddress.ToInt32());
        _process = game;
        game = null;

        return true;
      }
      catch (Win32Exception)
      {
        _process?.Dispose();
        _process = null;
        return false;
      }
      catch (NullReferenceException)
      {
        _process?.Dispose();
        _process = null;
        return false;
      }
    }

    /// <summary>
    /// Finds the specified process by name (case-insensitive)
    /// </summary>
    /// <param name="processName">The process name</param>
    /// <returns>Returns the process or null</returns>
    private Process GetProcess(string processName)
      => Process.GetProcesses().FirstOrDefault(p => p.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase) && !p.HasExited);

    public void Dispose() => _process?.Dispose();
  }

  #endregion PROCESS
}
