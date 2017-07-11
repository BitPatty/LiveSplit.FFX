using LiveSplit.Model;
using LiveSplit.UI.Components;
using LiveSplit.UI;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Windows.Forms;

namespace LiveSplit.FFX
{
    public class FFXComponent : LogicComponent
    {
        public override string ComponentName => "Final Fantasy X Autosplitter";

        public FFXSettings Settings { get; set; }

        private TimerModel _timer;
        private FFXMemory _gameMemory;
        private Timer _updateTimer;

        public FFXComponent(LiveSplitState state)
        {

#if DEBUG
            Debug.Listeners.Clear();
            Debug.Listeners.Add(TimedTraceListener.Instance);
#endif

            this.Settings = new FFX.FFXSettings();

            _timer = new TimerModel { CurrentState = state };
            _timer.CurrentState.OnStart += timer_OnStart;
            _timer.CurrentState.OnReset += timer_OnReset;

            _updateTimer = new Timer() { Interval = 15, Enabled = true };
            _updateTimer.Tick += updateTimer_Tick;

            _gameMemory = new FFXMemory();
            _gameMemory.OnAreaCompleted += gameMemory_OnAreaCompleted;
            _gameMemory.OnLoadFinished += gameMemory_OnLoadFinished;
            _gameMemory.OnLoadStarted += gameMemory_OnLoadStarted;
            _gameMemory.OnMusicSelect += gameMemory_OnMusicSelect;
            _gameMemory.OnMusicConfirm += gameMemory_OnMusicConfirm;
            _gameMemory.OnBossDefeated += gameMemory_OnBossDefeated;
        }

        public override void Dispose()
        {
            _timer.CurrentState.OnStart -= timer_OnStart;
            _updateTimer?.Dispose();
        }

        void updateTimer_Tick(object sender, EventArgs eventArgs)
        {
            try
            {
                _gameMemory.Update(this.Settings);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Initializes the timer as in game time.
        /// </summary>
        void timer_OnStart(object sender, EventArgs e)
        {
            _timer.InitializeGameTime();
        }

        void timer_OnReset(object sender, TimerPhase t)
        {
            onTimerReset();
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
        }

        void onTimerReset()
        {
            _timer.Reset();
            _gameMemory = null;             // Not sure about this either, definitely seemed the easiest to do to get all the flags to reset, but might be more elegant to manually make them false
            _gameMemory = new FFXMemory();  //Generate new FFXMemory so all flags reset back to false
            _gameMemory.OnAreaCompleted += gameMemory_OnAreaCompleted;
            _gameMemory.OnLoadFinished += gameMemory_OnLoadFinished;
            _gameMemory.OnLoadStarted += gameMemory_OnLoadStarted;
            _gameMemory.OnMusicSelect += gameMemory_OnMusicSelect;
            _gameMemory.OnMusicConfirm += gameMemory_OnMusicConfirm;
            _gameMemory.OnBossDefeated += gameMemory_OnBossDefeated;
        }

        void gameMemory_OnMusicSelect(object sender, EventArgs e)
        {
            if (this.Settings.Reset) onTimerReset();
        }

        void gameMemory_OnMusicConfirm(object sender, EventArgs e)
        {
            if (this.Settings.Start) _timer.Start();
            this.Settings.hasChanged = true;
        }

        void gameMemory_OnLoadStarted(object sender, EventArgs e)
        {
            if (this.Settings.RemoveLoads) _timer.CurrentState.IsGameTimePaused = true;
        }

        void gameMemory_OnLoadFinished(object sender, EventArgs e)
        {
            if (this.Settings.RemoveLoads) _timer.CurrentState.IsGameTimePaused = false;
        }

        void gameMemory_OnAreaCompleted(object sender, EventArgs e)
        {
            if (this.Settings.Split) _timer.Split();
        }

        void gameMemory_OnBossDefeated(object sender, EventArgs e)
        {
            if (this.Settings.Split) _timer.Split();
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            return this.Settings.GetSettings(document);
        }

        public override Control GetSettingsControl(LayoutMode mode)
        {
            return this.Settings;
        }

        public override void SetSettings(XmlNode settings)
        {
            this.Settings.SetSettings(settings);
        }

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
    }

    // Debug
    public class TimedTraceListener : DefaultTraceListener
    {
        private static TimedTraceListener _instance;
        public static TimedTraceListener Instance => _instance ?? (_instance = new TimedTraceListener());

        private TimedTraceListener() { }

        public int UpdateCount
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        public override void WriteLine(string message)
        {
            base.WriteLine("FFX: " + this.UpdateCount + " " + message);
        }
    }
}
