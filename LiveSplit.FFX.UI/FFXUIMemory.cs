using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LiveSplit.FFX.UI
{
    class FFXData : MemoryWatcherList
    {
        public Counter[] _counterData { get; set; }

        public FFXData(GameVersion version, int baseOffset, Counter[] counterData)
        {
            _counterData = new Counter[counterData.Length];
            counterData.CopyTo(_counterData, 0);

            if (version == GameVersion.v10)
            {
                for (int i = 0; i < _counterData.Length; i++)
                {
                    _counterData[i].Watcher = new MemoryWatcher<int>(new IntPtr(baseOffset + _counterData[i].Offset));
                }
            }
        }

        public event MemoryWatcherDataChangedEventHandler OnCounterWatcherDataChanged;

        public void Update(Process process)
        {
            if (OnCounterWatcherDataChanged != null)
            {
                var changedList = new List<MemoryWatcher>();

                for (int i = 0; i < _counterData.Length; i++)
                {
                    if (_counterData[i].Watcher.Update(process))
                        changedList.Add(_counterData[i].Watcher);
                }

                foreach (var watcher in changedList)
                    OnCounterWatcherDataChanged(watcher);
            }
            else
            {
                for (int i = 0; i < _counterData.Length; i++)
                {
                    _counterData[i].Watcher.Update(process);
                }
            }
        }
    }

    class FFXUIMemory
    {
        // Eventhandlers
        public event EventHandler<Tuple<int, int>> OnValueChanged;

        // Vars
        private List<int> _ignorePIDs;      // PIDs to ignore if necessary
        private FFXData _data;              // Memory Information
        private Process _process;           // Process Information

        private Counter[] _counterData { get; set; }

        // DLL Sizes to verify game version
        private enum ExpectedDllSizes
        {
            FFXExe = 37212160   //Steam release executable v1.0.0
        }

        // Add PIDs to ignore if necessary
        public FFXUIMemory(Counter[] counterData)
        {
            _ignorePIDs = new List<int>();
            this._counterData = counterData;
        }

        public void Update(FFXUISettings Settings)
        {
            // Try to rehook process if lost
            if (_process == null || _process.HasExited)
            {
                if (!this.TryGetGameProcess())
                    return;
            }

            _data.Update(_process);

            for (int i = 0; i < _data._counterData.Length; i++)
            {
                if (_data._counterData[i].Watcher.Changed)
                {
                    int counterValue = (int)_data._counterData[i].Watcher.Current & _data._counterData[i].Size;
                    Tuple<int, int> values = new Tuple<int, int>(i, counterValue);
                    this.OnValueChanged?.Invoke(this, values);
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
                //_ignorePIDs.Add(game.Id);
                //MessageBox.Show("Unexpected game version. Final Fantasy X 1.0.0 is required", "LiveSplit.FFX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            _data = new FFXData(version, game.MainModule.BaseAddress.ToInt32(), _counterData);
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
