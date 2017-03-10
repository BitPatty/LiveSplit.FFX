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

            _updateTimer = new Timer() { Interval = 15, Enabled = true };
            _updateTimer.Tick += updateTimer_Tick;

            _gameMemory = new FFXMemory();
            _gameMemory.OnAreaCompleted += gameMemory_OnAreaCompleted;
            _gameMemory.OnLoadFinished += gameMemory_OnLoadFinished;
            _gameMemory.OnLoadStarted += gameMemory_OnLoadStarted;
            _gameMemory.OnMusicSelect += gameMemory_OnMusicSelect;
            _gameMemory.OnMusicConfirm += gameMemory_OnMusicConfirm;
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
                _gameMemory.Update();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                MessageBox.Show("Couldn't update Game Memory");
            }
        }

        //Initialize as IGT
        void timer_OnStart(object sender, EventArgs e)
        {
            _timer.InitializeGameTime();
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {

        }

        //Reset timer on music selection screen
        void gameMemory_OnMusicSelect(object sender, EventArgs e)
        {
            if (this.Settings.AutoReset) _timer.Reset();
        }

        //Start timer on music confirmation
        void gameMemory_OnMusicConfirm(object sender, EventArgs e)
        {
            if (this.Settings.AutoStart) _timer.Start();
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
            if (this.Settings.AutoStart) _timer.Split();
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
