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


namespace LiveSplit.FFX.UI
{
    public class FFXUIComponent : IComponent
    {
        public string ComponentName => "FFX Display";
        public IDictionary<string, Action> ContextMenuControls { get; protected set; }

        protected InfoTextComponent InternalComponent;

        // IComponent implementations
        public void RenameComparison(string oldName, string newName) { }
        public float MinimumWidth { get { return InternalComponent.MinimumWidth; } }
        public float MinimumHeight { get { return InternalComponent.MinimumHeight; } }
        public float VerticalHeight { get { return InternalComponent.VerticalHeight; } }
        public float HorizontalWidth { get { return InternalComponent.HorizontalWidth; } }
        public float PaddingLeft { get { return InternalComponent.PaddingLeft; } }
        public float PaddingRight { get { return InternalComponent.PaddingRight; } }
        public float PaddingTop { get { return InternalComponent.PaddingTop; } }
        public float PaddingBottom { get { return InternalComponent.PaddingBottom; } }


        public FFXUISettings UISettings { get; set; }
        private LiveSplitState _state;
        private int _encounters;

        public FFXUIComponent(LiveSplitState state)
        {
            ContextMenuControls = new Dictionary<string, Action>();
            InternalComponent = new InfoTextComponent("Encounter Count", "0");

            UISettings = new FFXUISettings();

            _state = state;
            _state.OnReset += state_OnReset;
        }

        public void Dispose()
        {
            _state.OnReset -= state_OnReset;
        }

        public void SetEncounters(int count)
        {
            _encounters = count;
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            string encounters = _encounters.ToString(CultureInfo.InvariantCulture);

            if (invalidator != null && InternalComponent.InformationValue != encounters)
            {
                InternalComponent.InformationValue = encounters;
                invalidator.Invalidate(0f, 0f, width, height);
            }
        }

        void PrepareDraw(Graphics g, LiveSplitState state, float width, float height)
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

            InternalComponent.NameLabel.ForeColor = UISettings.OverrideTextColor ? UISettings.TextColor : state.LayoutSettings.TextColor;
            InternalComponent.ValueLabel.ForeColor = UISettings.OverrideTextColor ? UISettings.TextColor : state.LayoutSettings.TextColor;
            InternalComponent.NameLabel.HasShadow = InternalComponent.ValueLabel.HasShadow = state.LayoutSettings.DropShadows;
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region region)
        {
            PrepareDraw(g, state, width, VerticalHeight);
            InternalComponent.DrawVertical(g, state, width, region);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float width, Region region)
        {
            PrepareDraw(g, state, width, VerticalHeight);
            InternalComponent.DrawHorizontal(g, state, width, region);
        }

        void state_OnReset(object sender, TimerPhase t)
        {
            _encounters = 0;
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
