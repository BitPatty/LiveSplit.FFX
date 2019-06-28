namespace LiveSplit.FFX
{
  /// <summary>
  /// Game versioning
  /// </summary>
  internal enum GameVersion
  {
    Unknown,
    v1
  }

  /// <summary>
  /// Entry points for the different game versions
  /// </summary>
  internal enum ExpectedEntryPoints : long
  {
    Unknown = 0,
    v1 = 0x5B93C7,   //Steam Initial Release (2016)
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
