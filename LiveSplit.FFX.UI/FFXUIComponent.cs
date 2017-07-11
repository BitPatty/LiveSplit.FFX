using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using StringList = System.Collections.Generic.List<string>;

namespace LiveSplit.FFX.UI
{
    public struct Counter
    {
        private int _size;
        public string Text { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
        public int Size { get { return this._size; } set { if (value >= 4) this._size = 0x7fffffff; else this._size = (1 << (8 * value)) - 1; } }
        public MemoryWatcher Watcher { get; set; }
    }

    public class FFXUIComponent : IComponent
    {
        public string ComponentName => "FFX Display";
        public IDictionary<string, Action> ContextMenuControls { get; protected set; }

        protected InfoTextComponent InternalComponent;

        // IComponent implementations
        public void RenameComparison(string oldName, string newName) { }
        public float MinimumWidth { get { return this.InternalComponent.MinimumWidth; } }
        public float MinimumHeight { get { return this.InternalComponent.MinimumHeight; } }
        public float VerticalHeight { get { return this.InternalComponent.VerticalHeight; } }
        public float HorizontalWidth { get { return this.InternalComponent.HorizontalWidth; } }
        public float PaddingLeft { get { return this.InternalComponent.PaddingLeft; } }
        public float PaddingRight { get { return this.InternalComponent.PaddingRight; } }
        public float PaddingTop { get { return this.InternalComponent.PaddingTop; } }
        public float PaddingBottom { get { return this.InternalComponent.PaddingBottom; } }

        // Vars
        private LiveSplitState _state;
        public FFXUISettings UISettings { get; set; }
        private Timer _updateTimer;
        private FFXUIMemory _gameMemory;

        // Display data
        public string displayName { get; set; }
        public string displayValue { get; set; }
        public int counterIndex { get; set; }

        // Possible values to display, new values can be added here
        public Counter[] counterData = new Counter[]
            {
                new Counter { Text = "Encounter Count", Count = 0, Offset = 0xD307A4,  Size = sizeof(int) },
                new Counter { Text = "Speed Spheres", Count = 0, Offset = 0x11973C0, Size = sizeof(short) } //TODO, wrong Address
            };

        //Init
        public FFXUIComponent(LiveSplitState state)
        {
            this.ContextMenuControls = new Dictionary<string, Action>();
            this.InternalComponent = new InfoTextComponent("Select Value", "0");

            StringList valueList = new StringList();

            foreach (Counter item in counterData)
                valueList.Add(item.Text);

            this.UISettings = new FFXUISettings(valueList);
            UISettings.OnSelectionChanged += uiSettings_OnSelectionChanged;

            _state = state;
            _state.OnReset += state_OnReset;

            _gameMemory = new FFXUIMemory(counterData);
            _gameMemory.OnValueChanged += gameMemory_OnValueChanged;

            _updateTimer = new Timer() { Interval = 15, Enabled = true };
            _updateTimer.Tick += updateTimer_Tick;
        }

        public void Dispose()
        {
            _state.OnReset -= state_OnReset;
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (invalidator != null && (this.InternalComponent.InformationValue != this.displayValue || this.InternalComponent.InformationName != this.displayName))
            {
                this.InternalComponent.InformationName = this.displayName;
                this.InternalComponent.InformationValue = this.displayValue;
                invalidator.Invalidate(0f, 0f, width, height);
            }
        }

        public void DrawBackground(Graphics g, LiveSplitState state, float width, float height)
        {
            if (UISettings.BackgroundColor1.A > 0 || UISettings.BackgroundGradient != GradientType.Plain && UISettings.BackgroundColor2.A > 0)
            {
                var gradientBrush = new LinearGradientBrush(
                    new PointF(0, 0),
                    UISettings.BackgroundGradient == GradientType.Horizontal ? new PointF(width, 0) : new PointF(0, height),
                    UISettings.BackgroundColor1,
                    UISettings.BackgroundGradient == GradientType.Plain ? UISettings.BackgroundColor1 : UISettings.BackgroundColor2);

                g.FillRectangle(gradientBrush, 0, 0, width, height);
            }

            this.InternalComponent.NameLabel.ForeColor = UISettings.OverrideTextColor ? UISettings.TextColor : state.LayoutSettings.TextColor;
            this.InternalComponent.ValueLabel.ForeColor = UISettings.OverrideTextColor ? UISettings.TextColor : state.LayoutSettings.TextColor;
            this.InternalComponent.NameLabel.HasShadow = this.InternalComponent.ValueLabel.HasShadow = state.LayoutSettings.DropShadows;
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region region)
        {
            DrawBackground(g, state, width, VerticalHeight);
            this.InternalComponent.DrawVertical(g, state, width, region);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float width, Region region)
        {
            DrawBackground(g, state, width, VerticalHeight);
            this.InternalComponent.DrawHorizontal(g, state, width, region);
        }

        void state_OnReset(object sender, TimerPhase t)
        {

        }

        void updateTimer_Tick(object sender, EventArgs eventArgs)
        {
            try
            {
                _gameMemory.Update(this.UISettings);
            }
            catch (Exception)
            {

            }
        }

        void gameMemory_OnValueChanged(object sender, Tuple<int, int> valueData)
        {
            int index = valueData.Item1;
            int count = valueData.Item2;

            this.counterData[index].Count = count;

            if (index == this.counterIndex)
            {
                this.displayName = counterData[counterIndex].Text;
                this.displayValue = counterData[counterIndex].Count.ToString(CultureInfo.InvariantCulture);
            }
        }

        void uiSettings_OnSelectionChanged(object sender, int index)
        {
            this.counterIndex = index;
            this.displayName = counterData[counterIndex].Text;
            this.displayValue = counterData[counterIndex].Count.ToString(CultureInfo.InvariantCulture);
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            return UISettings.GetSettings(document);
        }

        public Control GetSettingsControl(LayoutMode mode)
        {
            UISettings.Mode = mode;
            return UISettings;
        }

        public void SetSettings(XmlNode settings)
        {
            UISettings.SetSettings(settings);
        }
    }
}
