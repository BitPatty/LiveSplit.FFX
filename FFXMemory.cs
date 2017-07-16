using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using StringList = System.Collections.Generic.List<string>;

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
        public MemoryWatcher<int> HP_Enemy_A { get; }

        public MemoryWatcher<int> Cutscene { get; }
        public MemoryWatcher<int> YuYevon { get; }
        public MemoryWatcher<int> EncounterCounter { get; }

        public FFXData(GameVersion version, int baseOffset)
        {
            if (version == GameVersion.v10)
            {
                this.CurrentLevel = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x8CB990));              // Current area ID, == 23 if main menu, 4B
                this.IsLoading = new MemoryWatcher<int>(new DeepPointer(0x8CC898, 0x123A4));                // == 2 if loading screen, 4B
                this.CursorPosition = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x1467808));           // == 0 if cursor on yes, == 1 if cursor on no, 1B, 0x00FF0000
                this.Input = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x8CB170));                     // Button Input, == 32 if A pressed, 4B
                this.StoryProgression = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x84949C));          // Storyline progress
                this.SelectScreen = new MemoryWatcher<int>(new IntPtr(baseOffset + 0xF25B30));              // == 7 || == 8 on confirm sound screen; == 6 on sound selection screen, 4B
                this.BattleState = new MemoryWatcher<int>(new DeepPointer(0x390D90, 0x4));                  // 10 = In Battle, 522 = Boss Defeated, 778 = Flee/Escape, 66058 = Victory Fanfare, 4B
                this.Cutscene = new MemoryWatcher<int>(new IntPtr(baseOffset + 0xD27C88));                  // Cutscene type
                this.YuYevon = new MemoryWatcher<int>(new IntPtr(baseOffset + 0xD2A8E8));                   // Yu Yevon screen transition = 1, back up - 0xD381AC = 3
                this.HP_Enemy_A = new MemoryWatcher<int>(new DeepPointer(0xD34460, 0x5D0));                 // Current HP of Enemy A
                this.EncounterCounter = new MemoryWatcher<int>(new IntPtr(baseOffset + 0xD307A4));          // Encounter counter
            }

            this.CurrentLevel.FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull;

            this.AddRange(this.GetType().GetProperties()
                .Where(p => !p.GetIndexParameters().Any())
                .Select(p => p.GetValue(this, null) as MemoryWatcher)
                .Where(p => p != null));
        }
    }

    struct SplitPair
    {
        public string SplitName;
        public bool SplitFlag;
    }

    class FFXMemory
    {
        private Dictionary<int, Tuple<int, SplitPair>> _LevelIDs = new Dictionary<int, Tuple<int, SplitPair>>
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

        private Dictionary<int, SplitPair> _ProgressionIDs = new Dictionary<int, SplitPair>
        {
            {0xF,   new SplitPair {SplitName = FFXComponent.SINSPAWN_AMMES,       SplitFlag = false}},     // 15 - Start Sinspawn Ammes battle
            {0x37,  new SplitPair {SplitName = FFXComponent.KLIKK,                SplitFlag = false}},     // 55 - Enter Klikk cutscene
            {0x4C,  new SplitPair {SplitName = FFXComponent.TROS,                 SplitFlag = false}},     // 76 - Enter Tros area after opening the door
            {0x77,  new SplitPair {SplitName = FFXComponent.PIRANHAS,             SplitFlag = false}},     // 119 - Enter Besaid Crossroads cutscene
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
            {0x825, new SplitPair {SplitName = FFXComponent.BEVELLE_GUARDS,       SplitFlag = false}},     // 2085 - Enter Guardian cutscene after defeating all Bevelle Guards
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

        private Dictionary<int, string> _MiscellaneousIDs = new Dictionary<int, string>
        {
            {0x49,  FFXComponent.PIRANHAS},                // 73 - Post Piranha Cutscene
            {0x16,  FFXComponent.GARUDA},                  // 22 - Garuda battle
            {0x3AC, FFXComponent.MUSHROOM_ROCK_ROAD},      // 940 - Kinoc Introduction
            {0x50,  FFXComponent.ISAARU},                  // 80 - Shiva summon
            {0x7,   FFXComponent.YU_YEVON},                // 7 - Yu Yevon battle
            {0x1C,  FFXComponent.YU_YEVON}                 // 28 - Yu Yevon battle
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
        private List<int> _ignorePIDs;      // PIDs to ignore if necessary
        private FFXData _data;              // Memory Information
        private Process _process;           // Process Information
        private bool _loadingStarted;       // true if loading screen active
        private int _isaaruCounter = 0;     // Boss counter for Isaaru split
        public StringList activatedSplits;

        // DLL Sizes to verify game version
        private enum ExpectedDllSizes
        {
            FFXExe = 37212160   //Steam release executable v1.0.0
        }

        // Add PIDs to ignore if necessary
        public FFXMemory()
        {
            _ignorePIDs = new List<int>();
            activatedSplits = new StringList();
        }

        public void Update(FFXSettings Settings)
        {
            // Try to rehook process if lost
            if (_process == null || _process.HasExited)
            {
                if (!this.TryGetGameProcess())
                    return;
                Settings.hasChanged = true;
            }

            if(Settings.hasChanged)
            {
                activatedSplits.Clear();
                activatedSplits.AddRange(Settings.GetSplits());
            }

            TimedTraceListener.Instance.UpdateCount++;

            _data.UpdateAll(_process);

            if (_data.EncounterCounter.Changed)
            {
                this.OnEncounter?.Invoke(this, _data.EncounterCounter.Current);
            }

            // Area splits
            if (_data.CurrentLevel.Changed && _LevelIDs.ContainsKey(_data.CurrentLevel.Current))
            {
                Tuple<int, SplitPair> tuple = _LevelIDs[_data.CurrentLevel.Current];
                int previousLevelID = tuple.Item1;
                SplitPair splitPair = tuple.Item2;

                if (activatedSplits.Contains(splitPair.SplitName) && !splitPair.SplitFlag && previousLevelID == _data.CurrentLevel.Old)
                {
                    this.OnAreaCompleted?.Invoke(this, EventArgs.Empty); //Split
                    splitPair.SplitFlag = true;
                    _LevelIDs[_data.CurrentLevel.Current] = new Tuple<int, SplitPair>(previousLevelID, splitPair);
                }
            }

            // Boss / Enemy splits
            if ((_data.BattleState.Changed || (_data.StoryProgression.Changed && _data.StoryProgression.Current == 2085)) && _ProgressionIDs.ContainsKey(_data.StoryProgression.Current))
            {
                // When battle state changes or for special case Bevelle Guards, when story progression changes as that is after the battle state has changed
                bool canSplit = false;
                SplitPair splitPair = _ProgressionIDs[_data.StoryProgression.Current];
                int battleState = _data.BattleState.Current;
                string splitName = splitPair.SplitName;

                if (activatedSplits.Contains(splitName) && !splitPair.SplitFlag && battleState == 522)
                {
                    canSplit = true;
                }
                else if (activatedSplits.Contains(splitName) && !splitPair.SplitFlag && battleState == 66058)
                {
                    //Special case for 'Klikk' and 'BiranYenke' splits; these battles end with the 'victory fanfare' which sets the battleState to 66058
                    if (_data.StoryProgression.Current == 55 || _data.StoryProgression.Current == 2510)
                        canSplit = true;
                }

                if (_MiscellaneousIDs.ContainsValue(splitName))
                    canSplit = false; // These splits are checked in _MiscellaneousIds;

                if (canSplit)
                {
                    this.OnBossDefeated?.Invoke(this, EventArgs.Empty);
                    splitPair.SplitFlag = true;
                    _ProgressionIDs[_data.StoryProgression.Current] = splitPair;
                }
            }

            // Misc splits
            if (_MiscellaneousIDs.ContainsKey(_data.Cutscene.Current) && _ProgressionIDs.ContainsKey(_data.StoryProgression.Current))
            {
                bool canSplit = false;
                SplitPair splitPair = _ProgressionIDs[_data.StoryProgression.Current];
                if (activatedSplits.Contains(splitPair.SplitName) && !splitPair.SplitFlag)
                {
                    if (_data.Cutscene.Current == 73 && _data.StoryProgression.Current == 119)
                    {
                        canSplit = true;    // Piranhas
                    }
                    else if (_data.Cutscene.Current == 22 && _data.StoryProgression.Current == 600 && _data.BattleState.Current == 522 && _data.BattleState.Old == 10 )
                    {
                        canSplit = true;    // Garuda
                    }
                    else if (_data.Cutscene.Current == 940 && _data.StoryProgression.Current == 835)
                    {
                        canSplit = true;    // Mushroom Rock Road
                    }
                    else if (_data.HP_Enemy_A.Current == 0 && _data.StoryProgression.Current == 3380 && _data.YuYevon.Changed && _data.YuYevon.Current == 1)
                    {
                        canSplit = true;    // Yu Yevon
                    }
                }

                // Split
                if (canSplit)
                {
                    this.OnAreaCompleted?.Invoke(this, EventArgs.Empty);
                    splitPair.SplitFlag = true;
                    _ProgressionIDs[_data.StoryProgression.Current] = splitPair;
                }
            }

            // Special Isaaru split
            if (_data.StoryProgression.Current == 2220 && _data.BattleState.Changed && _data.BattleState.Current == 522)
            {
                SplitPair splitPair = _ProgressionIDs[_data.StoryProgression.Current];
                ++_isaaruCounter;

                if (_isaaruCounter == 3 && !splitPair.SplitFlag)
                {
                    this.OnAreaCompleted?.Invoke(this, EventArgs.Empty);
                    splitPair.SplitFlag = true;
                    _ProgressionIDs[_data.StoryProgression.Current] = splitPair;
                }
            }

            // Main Menu
            if (_data.CurrentLevel.Current == 23)
            {
                // Reset _isaaruCounter if the main menu is entered (in case of GO on Isaaru or game crash)
                _isaaruCounter = 0;

                // Reset timer before starting a new game (on music selection screen)
                if (_data.SelectScreen.Current == 6)
                    this.OnMusicSelect?.Invoke(this, EventArgs.Empty);

                // Start timer when starting a new game (on a press if on sound confirmation screen and cursor on "Yes")
                if ((_data.SelectScreen.Current == 7 || _data.SelectScreen.Current == 8) && ((_data.CursorPosition.Current >> 16) & 0xFF) == 0 && _data.Input.Current == 32)
                    this.OnMusicConfirm?.Invoke(this, EventArgs.Empty);
            }

            // Loading screen in/out
            if (_data.IsLoading.Changed)
            {
                // Pause Timer if Loading Screen active
                if (_data.IsLoading.Current == 2)
                {
                    _loadingStarted = true;
                    this.OnLoadStarted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    // Resume timer if loading screen no longer active
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
            // Find process
            Process game = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.ToLower() == "ffx" && !p.HasExited && !_ignorePIDs.Contains(p.Id));

            if (game == null) return false;

            GameVersion version;

            // Verify .exe size
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

    // Game versioning for possible updates, currently there's only the 1.0.0 .exe available on steam
    enum GameVersion
    {
        v10
    }
}
