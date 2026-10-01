namespace LiveSplit.FFX
{
  /// <summary>
  /// Game versioning
  /// </summary>
  internal enum GameVersion
  {
    Unknown,
    v1,
    v2,
  }

  /// <summary>
  /// Entry points (relative to the base address) for the different game versions
  /// </summary>
  internal enum ExpectedEntryPoints : long
  {
    Unknown = 0,
    v1 = 0x5493C7,   //Steam Initial Release (2016)
    v2 = 0x5493C1,   //"Minor Bugfix" update (2026)
  }

  /// <summary>
  /// Battle State values
  /// </summary>
  internal enum BattleState : uint
  {
    Over = 522,
    Fanfare = 66058
  }
}
