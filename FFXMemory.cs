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
        //Levels + Story Progression
        private static readonly int[] _LevelIDs = new int[] { 0x1, 0x2 };
        private bool[] _LevelSplitFlag = new bool[_LevelIDs.Length];
        private int[] _PreviousLevelIDs = new int[] { 0x1, 0x2 };

        //BossIDs + HP
        private static readonly int[] _BossIDs = new int[] { 0x1, 0x2 };

        //Eventhandlers
        public event EventHandler OnLoadStarted;
        public event EventHandler OnLoadFinished;
        public event EventHandler OnMusicSelect;
        public event EventHandler OnMusicConfirm;
        public event EventHandler OnAreaCompleted;

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
                //Split when changing Area if desired
                if (_LevelIDs.Contains<int>(_data.CurrentLevel.Current) && !_LevelSplitFlag[Array.IndexOf(_LevelIDs, _data.CurrentLevel.Current)] && _PreviousLevelIDs[Array.IndexOf(_LevelIDs, _data.CurrentLevel.Current)] == _data.CurrentLevel.Old)
                {
                    //Split
                    this.OnAreaCompleted?.Invoke(this, EventArgs.Empty);
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
