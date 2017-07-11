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

        private LiveSplitState _state;
        //private int _encounters;
        public FFXUISettings UISettings { get; set; }

        public Dictionary<string, int> MemoryValues { get; private set; } = new Dictionary<string, int>
        {
            {"Encounter Count", 0 },
            {"Speed Spheres", 0 }
        };

        public FFXUIComponent(LiveSplitState state)
        {
            this.ContextMenuControls = new Dictionary<string, Action>();
            this.InternalComponent = new InfoTextComponent("Value", "0");

            _state = state;
            _state.OnReset += state_OnReset;

            UISettings = new FFXUISettings(new StringList(this.MemoryValues.Keys));
        }

        public void Dispose()
        {
            _state.OnReset -= state_OnReset;
        }

        public void SetEncounters(int count)
        {
            MemoryValues["Encounter Count"] = count;
        }

        public void SetSpeedSpheres(int count)
        {
            MemoryValues["Speed Spheres"] = count;
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {

            string selectedValue = UISettings.ValueString;

            if (invalidator != null && (this.InternalComponent.InformationValue != MemoryValues[selectedValue].ToString(CultureInfo.InvariantCulture) || this.InternalComponent.InformationName != selectedValue))
            {
                this.InternalComponent.InformationName = selectedValue;
                this.InternalComponent.InformationValue = MemoryValues[selectedValue].ToString(CultureInfo.InvariantCulture);
                invalidator.Invalidate(0f, 0f, width, height);
            } 
        }

        public void DrawBackground(Graphics g, LiveSplitState state, float width, float height)
        {
            if(UISettings.BackgroundColor1.A > 0 || UISettings.BackgroundGradient != GradientType.Plain && UISettings.BackgroundColor2.A > 0)
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
