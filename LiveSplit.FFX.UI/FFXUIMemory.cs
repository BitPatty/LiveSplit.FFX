using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace LiveSplit.FFX.UI
{
    class FFXData : MemoryWatcherList
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
        public Counter[] _counterData { get; set; }


        /// <summary>
        /// Tries to find the specified key in the specified memory range.
        /// </summary>
        /// <param name="process">Process</param>
        /// <param name="counterIndex">Index of Counter to modify</param>
        /// <param name="key">Key</param>
        /// <param name="offset">Offset to area to search</param>
        /// <param name="range">Size of area to search</param>
        /// <returns>True if key could be found</returns>
        public bool SetIndex(Process process, int counterIndex, int key, int offset, int range)
        {
            byte[] buffer = new byte[range];

            int bytesRead;

            try
            {
                ReadProcessMemory((int)process.Handle, new IntPtr(process.MainModule.BaseAddress.ToInt32() + offset), buffer, range, out bytesRead);
            }
            catch (Exception)
            {
                return false;
            }

            for (int i = 0; i < range; i += 2)
            {
                if (i < buffer.Length && buffer[i] == key)
                {
                    i /= 2;
                    _counterData[counterIndex].Index = i;
                    return true;
                }
            }

            return false;
        }

        public FFXData(GameVersion version, Process process, Counter[] counterData)
        {
            int baseOffset = process.MainModule.BaseAddress.ToInt32();
            _counterData = new Counter[counterData.Length];
            counterData.CopyTo(_counterData, 0);

            if (version == GameVersion.v10)
            {
                for (int i = 0; i < _counterData.Length; i++)
                {
                    if (_counterData[i].IndexOffset == 0)
                    {
                        _counterData[i].Watcher = new MemoryWatcher<int>(new IntPtr(baseOffset + _counterData[i].Offset));
                    }
                    else
                    {
                        SetIndexWatcher(process, i);
                    }
                }
            }
        }

        public void Update(Process process)
        {
            for (int i = 0; i < _counterData.Length; i++)
            {
                if (_counterData[i].IndexOffset != 0)
                    SetIndexWatcher(process, i);
                else
                    _counterData[i].Watcher.Update(process);
            }
        }


        public void SetIndexWatcher(Process process, int index)
        {
            if (SetIndex(process, index, _counterData[index].IndexKey, _counterData[index].IndexOffset, _counterData[index].IndexBufferSize))
            {
                _counterData[index].Watcher = new MemoryWatcher<int>(new IntPtr(process.MainModule.BaseAddress.ToInt32() + _counterData[index].Offset + _counterData[index].Index));
                _counterData[index].Watcher.Update(process);
            }
            else
            {
                _counterData[index].Watcher = null;
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

        // DLL Sizes to verify process version
        private enum ExpectedDllSizes
        {
            FFXExe = 37212160   //Steam release executable v1.0.0
        }

        // Add PIDs to ignore if necessary
        public FFXUIMemory(Counter[] counterData)
        {
            _ignorePIDs = new List<int>();
            _counterData = counterData;
        }

        public void Update(FFXUISettings Settings)
        {
            // Try to rehook process if lost
            if (_process == null || _process.HasExited)
            {
                if (!TryGetGameProcess())
                    throw new ProcessLostException();
            }

            _data.Update(_process);

            for (int i = 0; i < _data._counterData.Length; i++)
            {
                int counterValue;
                try
                {
                    counterValue = (int)_data._counterData[i].Watcher.Current & _data._counterData[i].Size;
                }
                catch (NullReferenceException)
                {
                    // Watcher or Watcher.Current == null
                    counterValue = 0;
                }

                Tuple<int, int> values = new Tuple<int, int>(i, counterValue);
                OnValueChanged?.Invoke(this, values);
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
                //_ignorePIDs.Add(process.Id);
                //MessageBox.Show("Unexpected process version. Final Fantasy X 1.0.0 is required", "LiveSplit.FFX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            _data = new FFXData(version, game, _counterData);
            _process = game;

            return true;
        }
    }

    // Game versioning for possible updates, currently there's only the 1.0.0 .exe available on steam
    enum GameVersion
    {
        v10
    }


    public class ProcessLostException : System.Exception
    {
        public ProcessLostException() : base() { }
        public ProcessLostException(string message) : base(message) { }
        public ProcessLostException(string message, System.Exception inner) : base(message, inner) { }

        protected ProcessLostException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
