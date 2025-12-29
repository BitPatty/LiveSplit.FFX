using LiveSplit.FFX.UI;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.FFX
{
  public class FFXComponent : LogicComponent
  {
    public override string ComponentName => "Final Fantasy X Autosplitter";

    public FFXSettings Settings { get; set; }

    private readonly TimerModel _timer;
    private readonly LiveSplitState _state;
    private FFXMemory _gameMemory;
    private readonly Timer _updateTimer;
    private FFXUIComponent UI => _state.Layout.Components.FirstOrDefault(c => c.GetType() == typeof(FFXUIComponent)) as FFXUIComponent;

    public FFXComponent(LiveSplitState state)
    {
      Settings = new FFXSettings();

      _state = state;
      _timer = new TimerModel { CurrentState = state };
      _timer.CurrentState.OnStart += Timer_OnStart;
      _timer.CurrentState.OnReset += Timer_OnReset;

      _gameMemory = new FFXMemory();

      _gameMemory.OnAreaCompleted += GameMemory_OnAreaCompleted;
      _gameMemory.OnLoadFinished += GameMemory_OnLoadFinished;
      _gameMemory.OnLoadStarted += GameMemory_OnLoadStarted;
      _gameMemory.OnMusicSelect += GameMemory_OnMusicSelect;
      _gameMemory.OnMusicConfirm += GameMemory_OnMusicConfirm;
      _gameMemory.OnBossDefeated += GameMemory_OnBossDefeated;
      _gameMemory.OnEncounter += GameMemory_OnEncounter;

      _updateTimer = new Timer() { Interval = 15, Enabled = true };
      _updateTimer.Tick += UpdateTimer_Tick;
    }

    private void UpdateTimer_Tick(object sender, EventArgs eventArgs)
    {
      try
      {
        _gameMemory.Update(Settings);
      }
      catch { }
    }

    /// <summary>
    /// Initializes the timer as in game time.
    /// </summary>
    private void Timer_OnStart(object sender, EventArgs e) => _timer.InitializeGameTime();

    private void Timer_OnReset(object sender, TimerPhase t) => ResetAutoSplit();

    public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }

    private void ResetAutoSplit()
    {
      _gameMemory.OnAreaCompleted -= GameMemory_OnAreaCompleted;
      _gameMemory.OnLoadFinished -= GameMemory_OnLoadFinished;
      _gameMemory.OnLoadStarted -= GameMemory_OnLoadStarted;
      _gameMemory.OnMusicSelect -= GameMemory_OnMusicSelect;
      _gameMemory.OnMusicConfirm -= GameMemory_OnMusicConfirm;
      _gameMemory.OnBossDefeated -= GameMemory_OnBossDefeated;
      _gameMemory.OnEncounter -= GameMemory_OnEncounter;
      _gameMemory?.Dispose();
      _gameMemory = new FFXMemory();
      _gameMemory.OnAreaCompleted += GameMemory_OnAreaCompleted;
      _gameMemory.OnLoadFinished += GameMemory_OnLoadFinished;
      _gameMemory.OnLoadStarted += GameMemory_OnLoadStarted;
      _gameMemory.OnMusicSelect += GameMemory_OnMusicSelect;
      _gameMemory.OnMusicConfirm += GameMemory_OnMusicConfirm;
      _gameMemory.OnBossDefeated += GameMemory_OnBossDefeated;
      _gameMemory.OnEncounter += GameMemory_OnEncounter;
    }

    private void GameMemory_OnMusicSelect(object sender, EventArgs e)
    {
      if (Settings.Reset) ResetAutoSplit();
    }

    private void GameMemory_OnMusicConfirm(object sender, EventArgs e)
    {
      Settings.HasChanged = true;
      if (Settings.Start) _timer.Start();
    }

    private void GameMemory_OnLoadStarted(object sender, EventArgs e)
    {
      if (Settings.RemoveLoads) _state.IsGameTimePaused = true;
    }

    private void GameMemory_OnLoadFinished(object sender, EventArgs e)
    {
      if (Settings.RemoveLoads) _state.IsGameTimePaused = false;
    }

    private void GameMemory_OnAreaCompleted(object sender, EventArgs e)
    {
      if (Settings.Split) _timer.Split();
    }

    private void GameMemory_OnBossDefeated(object sender, EventArgs e)
    {
      if (Settings.Split) _timer.Split();
    }

    private void GameMemory_OnEncounter(object sender, int count) => UI?.SetEncounters(count);
    public override XmlNode GetSettings(XmlDocument document) => Settings.GetSettings(document);
    public override Control GetSettingsControl(LayoutMode mode) => Settings;
    public override void SetSettings(XmlNode settings) => Settings.SetSettings(settings);

    public static string SINSPAWN_AMMES = "SinspawnAmmes";
    public static string KLIKK = "Klikk";
    public static string TROS = "Tros";
    public static string PIRANHAS = "Piranhas";
    public static string KIMAHRI = "Kimahri";
    public static string SINSPAWN_ECHUILLES = "SinspawnEchuilles";
    public static string SINSPAWN_GENEAUX = "SinspawnGeneaux";
    public static string KILIKA_WOODS = "KilikaWoods";
    public static string OBLITZERATOR = "Oblitzerator";
    public static string BLITZBALL_COMPLETE = "BlitzballComplete";
    public static string GARUDA = "Garuda";
    public static string MIIHEN_HIGHROAD = "MiihenHighroad";
    public static string OLD_ROAD = "OldRoad";
    public static string MUSHROOM_ROCK_ROAD = "MushroomRockRoad";
    public static string SINSPAWN_GUI = "SinspawnGui";
    public static string MRR_SKIP = "MrrSkip";
    public static string DJOSE_HIGHROAD = "DjoseHighroad";
    public static string MOONFLOW = "Moonflow";
    public static string EXTRACTOR = "Extractor";
    public static string GUADOSALAM = "Guadosalam";
    public static string THUNDER_PLAINS = "ThunderPlains";
    public static string SPHERIMORPH = "Spherimorph";
    public static string CRAWLER = "Crawler";
    public static string SEYMOUR = "Seymour";
    public static string WENDIGO = "Wendigo";
    public static string HOME = "Home";
    public static string EVRAE = "Evrae";
    public static string BEVELLE_GUARDS = "BevelleGuards";
    public static string BEVELLE_TRIALS = "BevelleTrials";
    public static string ISAARU = "Isaaru";
    public static string SEYMOUR_NATUS = "SeymourNatus";
    public static string BIRAN_YENKE = "BiranYenke";
    public static string SEYMOUR_FLUX = "SeymourFlux";
    public static string SANCTUARY_KEEPER = "SanctuaryKeeper";
    public static string YUNALESCA = "Yunalesca";
    public static string SIN_CORE = "SinCore";
    public static string OVERDRIVE_SIN = "OverdriveSin";
    public static string SEYMOUR_OMNIS = "SeymourOmnis";
    public static string BRASKAS_FINAL_AEON = "BraskasFinalAeon";
    public static string YU_YEVON = "YuYevon";

    public override void Dispose()
    {
      _timer.CurrentState.OnStart -= Timer_OnStart;
      _updateTimer?.Dispose();
    }
  }
}
