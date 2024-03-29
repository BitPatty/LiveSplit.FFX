﻿using LiveSplit.ComponentUtil;
using System;
using System.Linq;

namespace LiveSplit.FFX
{
  internal class FFXData : MemoryWatcherList
  {
    public MemoryWatcher<int> CurrentLevel { get; }
    public MemoryWatcher<int> LastLevel { get; }
    public MemoryWatcher<int> IsLoading { get; }
    public MemoryWatcher<int> CursorPosition { get; }
    public MemoryWatcher<int> Input { get; }
    public MemoryWatcher<int> StoryProgression { get; }
    public MemoryWatcher<int> SelectScreen { get; }
    public MemoryWatcher<int> BattleState { get; }
    public MemoryWatcher<int> HPEnemyA { get; }
    public MemoryWatcher<int> CutsceneType { get; }
    public MemoryWatcher<int> YuYevon { get; }
    public MemoryWatcher<int> EncounterCounter { get; }
    public MemoryWatcher<short> EncounterMapID { get; }
    public MemoryWatcher<byte> EncounterFormationID1 { get; }
    public MemoryWatcher<byte> EncounterFormationID2 { get; }

    public FFXData(GameVersion version, int baseOffset)
    {
      if (version == GameVersion.v1)
      {
        CurrentLevel = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x8CB990));              // Current area ID, == 23 if main menu, 4B
        IsLoading = new MemoryWatcher<int>(new DeepPointer(0x8CC898, 0x123A4));                // == 2 if loading screen, 4B
        CursorPosition = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x1467808));           // == 0 if cursor on yes, == 1 if cursor on no, 1B, 0x00FF0000
        Input = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x8CB170));                     // Button Input, == 32 if A pressed, 4B
        StoryProgression = new MemoryWatcher<int>(new IntPtr(baseOffset + 0x84949C));          // Storyline progress
        SelectScreen = new MemoryWatcher<int>(new IntPtr(baseOffset + 0xF25B30));              // == 7 || == 8 on confirm sound screen; == 6 on sound selection screen, 4B
        BattleState = new MemoryWatcher<int>(new DeepPointer(0x390D90, 0x4));                  // 10 = In Battle, 522 = Boss Defeated, 778 = Flee/Escape, 66058 = Victory Fanfare, 4B
        CutsceneType = new MemoryWatcher<int>(new IntPtr(baseOffset + 0xD27C88));              // Cutscene type
        YuYevon = new MemoryWatcher<int>(new IntPtr(baseOffset + 0xD2A8E8));                   // Yu Yevon screen transition = 1, back up - 0xD381AC = 3
        HPEnemyA = new MemoryWatcher<int>(new DeepPointer(0xD34460, 0x5D0));                   // Current HP of Enemy A
        EncounterCounter = new MemoryWatcher<int>(new IntPtr(baseOffset + 0xD307A4));          // Encounter counter
        EncounterMapID = new MemoryWatcher<short>(new IntPtr(baseOffset + 0xD2C256));            // Encounter Map ID
        EncounterFormationID1 = new MemoryWatcher<byte>(new IntPtr(baseOffset + 0xD2C258));     // Encounter Formation ID 1
        EncounterFormationID2 = new MemoryWatcher<byte>(new IntPtr(baseOffset + 0xD2C259));     // Encounter Formation ID 2
        }

      CurrentLevel.FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull;

      AddRange(GetType().GetProperties()
          .Where(p => p.GetIndexParameters().Length == 0)
          .Select(p => p.GetValue(this, null) as MemoryWatcher)
          .Where(p => p != null));
    }
  }
}
