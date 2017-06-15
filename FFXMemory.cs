using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

/*====================================================================*
 *      Memory Locations                                              *
 * ===================================================================*
 * 
 * Current Area (static):
 * FFX.exe+8CB990
 * FFX.exe+EFBBF8
 * FFX.exe+F30804
 * 
 * Loadingscreen: 
 * [0x8CC898 + 0x123A4]
 * 
 * Button inputs: 
 * FFX.exe+8CB170
 * 
 * Inputcounter (???): 
 * FFX.exe+EF4E3C
 * 
 * Cursor position: 
 * FFX.exe+146780A
 * 
 * Current Battle Over (static):
 * FFX.exe+F3F73C (4B, Includes Flee, resets on battle start, too late on Yu Yevon)
 * 
 * Story Progression:
 * FFX.exe+84949C
 * 
 * */

namespace LiveSplit.FFX
{
    class FFXData : MemoryWatcherList
    {
        public MemoryWatcher<int> CurrentLevel { get; }
        public MemoryWatcher<int> LastLevel { get; }
        public MemoryWatcher<int> IsLoading { get; }
        public MemoryWatcher<int> CursorPosition { get; }
        public MemoryWatcher<int> Input { get; }
        public MemoryWatcher<int> StoryProgression { get; }
        public MemoryWatcher<int> SelectScreen { get; }
        public MemoryWatcher<int> BattleState { get; }

        public FFXData(GameVersion version, int baseOffset)
        {
            if (version == GameVersion.v10)
            {
                this.CurrentLevel = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x8CB990));              //Current area ID, == 23 if main manu, 4B
                this.IsLoading = new MemoryWatcher<int>(new DeepPointer(0x8CC898, 0x123A4));                // == 2 if loading screen, 4B
                this.CursorPosition = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x1467808));           //== 0 if cursor on yes, == 1 if cursor on no, 1B, 0x00FF0000
                this.Input = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x8CB170));                     //Button Input, == 32 if A pressed, 4B
                this.StoryProgression = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x84949C));          //Storyline progress
                this.SelectScreen = new MemoryWatcher<int>(new IntPtr(baseOffset + 0xF25B30));              //== 7 || == 8 on confirm sound screen; == 6 on sound selection screen, 4B
                this.BattleState = new MemoryWatcher<int>(new DeepPointer(0x390D90, 0x4));         //10 = In Battle, 522 = Boss Defeated, 778 = Flee/Escape, 66058 = Victory Fanfare
            }

            this.CurrentLevel.FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull;

            this.AddRange(this.GetType().GetProperties()
                .Where(p => !p.GetIndexParameters().Any())
                .Select(p => p.GetValue(this, null) as MemoryWatcher)
                .Where(p => p != null));
        }
    }

    class FFXMemory
    {
        //Levels IDs and Previous Level IDs
        //IMPORTANT - If adding/removing from this array, make sure you add/remove the corresponding Level ID
        //in the _PreviousLevelIDs array at the correct index or area autosplits will no longer work!
        private static readonly int[] _LevelIDs = new int[]
        {
            0x2E,  // 46 - Kilika - Residential Area
            0x7D,  // 125 - Stadium - Pool (Post Blitzball Cutscene)
            0x3A,  // 58 - Highroad - Agency, Front
            0x4F,  // 79 - Mushroom Rock
            0x4C,  // 76 - Djose - Pilgrimage Road
            0x69,  // 105 - Moonflow - South Bank
            0x8C,  // 140 - Thunder Plains - South
            0x6E,  // 110 - Macalania Woods - South
            0xDB,  // 219 - Home - Environment Controls
            0xE2   // 226 - Bevelle - Antechamber (Cutscene)
        };

        private static readonly int[] _PreviousLevelIDs = new int[]
        {
            0x12,  // 18 - Kilika Woods
            0xD4,  // 212 - Stadium - Pool (Blitzball Exp Screen)
            0x7F,  // 127 - Highroad - Central
            0x3B,  // 59 - Highroad - North End
            0x5D,  // 93 - Djose Highroad
            0x4B,  // 75 - Moonflow - South Bank Road
            0x87,  // 135 - Guadosalam
            0xA2,  // 162 - Thunder Plains - North
            0x118, // 280 - Home - Main Corridor
            0x132  // 306 - Bevelle - Trials
        };

        private static readonly int[] _ProgressionIDs = new int[]
        {
            0xF,   // 15 - Start Sinspawn Ammes battle
            0x37,  // 55 - Enter Klikk cutscene
            0x4C,  // 76 - Enter Tros area after opening the door
            0xD6,  // 214 - Gain control on Besaid Promontory before Kimahri
            0x118, // 280 - Enter underwater cutscene before Sinpsawn Echuilles
            0x142, // 322 - Gain control on Pilgrimage Road before Sinspawn Geneaux
            0x1F4, // 502 - Enter Cutscene after Blitzball cutscene ("Still in there!") before Oblitzerator
            0x258, // 600 - Auron FMV before Garuda                                                                  =========[BROKEN - It currently splits on the Dinosaur battle before Garuda]
            0x361, // 865 - Enter Sin destruction FMV before Sinspawn Gui 2
            0x424, // 1060 - Enter Extractor cutscene
            0x58C, // 1420 - Gain control in Spring before Spherimorph
            0x5CD, // 1485 - Enter Al Bhed cutscene on lake before Crawler
            0x604, // 1540 - Start Seymour battle
            0x622, // 1570 - Gain control on Crevasse before Wendigo
            0x7F8, // 2040 - Gain control after Evrae/Auron cutscene before Evrae
            0x825, // 2085 - Enter Guardian cutscene after defeating all Bevelle Guards
            0x8AC, // 2220 - Gain control in Via Purifico before Isaaru
            0x8E8, // 2280 - Enter Seymour Natus Battle cutscene
            0x9CE, // 2510 - Gain control after Kelk cutscene before Biran and Yenke
            0x9E2, // 2530 - Gain control after singing before Seymour Flux
            0xA19, // 2585 - Gain control on Fayth cluster before Sanctuary Keeper
            0xAFF, // 2815 - Gain control in Great Hall before Yunalesca
            0xC21, // 3015 - Land on Sin cutscene before Sin Core
            0xC3F, // 3135 - Gain control in corridor before Overdrive Sin
            0xC85, // 3205 - Gain control inside Sin before Seymour Omnis
            0xCE4, // 3300 - Start Braska's Final Aeon fight
            0xD34  // 3380 - Start Yu Yevon fight
        };

        private bool[] _LevelSplitFlag = new bool[_LevelIDs.Length];
        private bool[] _ProgressionSplitFlag = new bool[_ProgressionIDs.Length];

        //BossIDs + HP
        //private static readonly int[] _BossIDs = new int[] { 0x1, 0x2 };

        //Eventhandlers
        public event EventHandler OnLoadStarted;
        public event EventHandler OnLoadFinished;
        public event EventHandler OnMusicSelect;
        public event EventHandler OnMusicConfirm;
        public event EventHandler OnAreaCompleted;
        public event EventHandler OnBossDefeated;

        //PIDs to ignore if necessary
        private List<int> _ignorePIDs;

        private FFXData _data;
        private Process _process;
        private bool _loadingStarted;

        //DLL Sizes to verify game version
        private enum ExpectedDllSizes
        {
            FFXExe = 37212160   //Steam release executable v1.0.0
        }

        //Add PIDs to ignore if necessary
        public FFXMemory()
        {
            _ignorePIDs = new List<int>();
        }

        //Read memory
        public void Update()
        {
            //Try to rehook process
            if (_process == null || _process.HasExited)
            {
                if (!this.TryGetGameProcess())
                    return;
            }

            TimedTraceListener.Instance.UpdateCount++;

            //Update all memory data
            _data.UpdateAll(_process);

            if (_data.CurrentLevel.Changed)
            {
                int levelIDIndex = Array.IndexOf(_LevelIDs, _data.CurrentLevel.Current);

                //Split when changing Area if desired
                if (_LevelIDs.Contains<int>(_data.CurrentLevel.Current) && !_LevelSplitFlag[levelIDIndex] && _PreviousLevelIDs[levelIDIndex] == _data.CurrentLevel.Old)
                {
                    //Split
                    this.OnAreaCompleted?.Invoke(this, EventArgs.Empty);
                    _LevelSplitFlag[levelIDIndex] = true;
                }
            }

            if (_data.BattleState.Changed)
            {
                bool canSplit = false;
                int progressionIDIndex = Array.IndexOf(_ProgressionIDs, _data.StoryProgression.Current);
                int battleState = _data.BattleState.Current;

                //Split when Boss Defeated
                if (_ProgressionIDs.Contains<int>(_data.StoryProgression.Current) && !_ProgressionSplitFlag[progressionIDIndex] && battleState == 522)
                {
                    canSplit = true;
                }
                else if (_ProgressionIDs.Contains<int>(_data.StoryProgression.Current) && !_ProgressionSplitFlag[progressionIDIndex] && battleState == 66058)
                {
                    //Special case for 'Klikk' and 'Biran and Yenke' splits; these battles end with the 'victory fanfare' which sets the battleState to 66058
                    if (_data.StoryProgression.Current == 55 || _data.StoryProgression.Current == 2510)
                        canSplit = true;
                }

                if (canSplit)
                {
                    //Split
                    this.OnBossDefeated?.Invoke(this, EventArgs.Empty);
                    _ProgressionSplitFlag[progressionIDIndex] = true;
                }
            }

            if (_data.CurrentLevel.Current == 23)
            {
                //Reset timer before starting a new game (on music selection screen)
                if (_data.SelectScreen.Current == 6) this.OnMusicSelect?.Invoke(this, EventArgs.Empty);

                //Start timer when starting a new game (on a press if on sound confirmation screen and cursor on "Yes")
                if ((_data.SelectScreen.Current == 7 || _data.SelectScreen.Current == 8) && ((_data.CursorPosition.Current >> 16) & 0xFF) == 0 && _data.Input.Current == 32)
                {
                    this.OnMusicConfirm?.Invoke(this, EventArgs.Empty);
                }
            }


            if (_data.IsLoading.Changed)
            {
                //Pause Timer if Loading Screen active
                if (_data.IsLoading.Current == 2)
                {
                    _loadingStarted = true;
                    this.OnLoadStarted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    //Resume timer if loading screen no longer active
                    if (_loadingStarted)
                    {
                        _loadingStarted = false;
                        this.OnLoadFinished?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }



        bool TryGetGameProcess()
        {
            //Find process
            Process game = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.ToLower() == "ffx" && !p.HasExited && !_ignorePIDs.Contains(p.Id));

            if (game == null) return false;

            GameVersion version;

            //Verify .exe size
            if (game.MainModuleWow64Safe().ModuleMemorySize == (int)ExpectedDllSizes.FFXExe)
            {
                version = GameVersion.v10;
            }
            else
            {
                _ignorePIDs.Add(game.Id);
                MessageBox.Show("Unexpected game version. Final Fantasy X 1.0.0 is required", "LiveSplit.FFX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            _data = new FFXData(version, game.MainModule.BaseAddress.ToInt32());
            _process = game;

            return true;
        }
    }

    //Game versioning for possible updates, currently there's only 1.0.0
    enum GameVersion
    {
        v10
    }
}
