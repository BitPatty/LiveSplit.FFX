namespace LiveSplit.FFX
{
  /// <summary>
  /// Game versioning
  /// </summary>
  internal enum GameVersion
  {
    Unknown,
    v1,
    v2
  }

  /// <summary>
  /// Entry points for the different game versions
  /// </summary>
  internal enum ExpectedEntryPoints : long
  {
    Unknown = 0,
    v1 = 0x5B93C7,   //Steam Initial Release (2016)
    v2 = 0x10D9571   //Steam Update (March 2019)
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
