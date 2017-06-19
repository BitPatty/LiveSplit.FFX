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

        //Free ressources
        public override void Dispose()
        {
            _timer.CurrentState.OnStart -= timer_OnStart;
            _updateTimer?.Dispose();
        }

        //Check memory changes
        void updateTimer_Tick(object sender, EventArgs eventArgs)
        {
            try
            {
                _gameMemory.Update(this.Settings.GetSplits());
                // Not sure about this, a list of the splits get sent to FFXMemory so it knows which splits the user has selected to be split
                // But this method is called rapidly so might be expensive to pass this list of strings in continually?
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                //MessageBox.Show("Couldn't update Game Memory");
            }
}

        //Initialize as IGT
        void timer_OnStart(object sender, EventArgs e)
        {
            _timer.InitializeGameTime();
        }

        //User has reset timer
        void timer_OnReset(object sender, TimerPhase t)
        {
            _timer.Reset();
            _gameMemory = null; // Not sure about this either, definitely seemed the easiest to do to get all the flags to reset, but might be more elegant to manually make them false
            _gameMemory = new FFXMemory(); //Generate new FFXMemory so all flags reset back to false
            _gameMemory.OnAreaCompleted += gameMemory_OnAreaCompleted;
            _gameMemory.OnLoadFinished += gameMemory_OnLoadFinished;
            _gameMemory.OnLoadStarted += gameMemory_OnLoadStarted;
            _gameMemory.OnMusicSelect += gameMemory_OnMusicSelect;
            _gameMemory.OnMusicConfirm += gameMemory_OnMusicConfirm;
            _gameMemory.OnBossDefeated += gameMemory_OnBossDefeated;
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {

        }

        //Reset timer on music selection screen
        void gameMemory_OnMusicSelect(object sender, EventArgs e)
        {
            if (this.Settings.Reset) _timer.Reset();
        }

        //Start timer on music confirmation
        void gameMemory_OnMusicConfirm(object sender, EventArgs e)
        {
            if (this.Settings.Start) _timer.Start();
        }

        //Pause timer on loading screen
        void gameMemory_OnLoadStarted(object sender, EventArgs e)
        {
            if (this.Settings.RemoveLoads) _timer.CurrentState.IsGameTimePaused = true;
        }

        //Resume timer after loading screen
        void gameMemory_OnLoadFinished(object sender, EventArgs e)
        {
            if (this.Settings.RemoveLoads) _timer.CurrentState.IsGameTimePaused = false;
        }

        //Split on area completed
        void gameMemory_OnAreaCompleted(object sender, EventArgs e)
        {
            if (this.Settings.Split) _timer.Split();
        }

        //Split on boss defeated
        void gameMemory_OnBossDefeated(object sender, EventArgs e)
        {
            if (this.Settings.Split) _timer.Split();
        }

        //Get config file
        public override XmlNode GetSettings(XmlDocument document)
        {
            return this.Settings.GetSettings(document);
        }

        //Get control module config
        public override Control GetSettingsControl(LayoutMode mode)
        {
            return this.Settings;
        }

        //Save to config file
        public override void SetSettings(XmlNode settings)
        {
            this.Settings.SetSettings(settings);
        }

    }

    //Debug
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
