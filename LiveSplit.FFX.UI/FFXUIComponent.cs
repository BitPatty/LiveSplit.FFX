using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public float MinimumWidth { get { return this.InternalComponent.MinimumWidth; } }
        public float MinimumHeight { get { return this.InternalComponent.MinimumHeight; } }
        public float VerticalHeight { get { return this.InternalComponent.VerticalHeight; } }
        public float HorizontalWidth { get { return this.InternalComponent.HorizontalWidth; } }
        public float PaddingLeft { get { return this.InternalComponent.PaddingLeft; } }
        public float PaddingRight { get { return this.InternalComponent.PaddingRight; } }
        public float PaddingTop { get { return this.InternalComponent.PaddingTop; } }
        public float PaddingBottom { get { return this.InternalComponent.PaddingBottom; } }

        private LiveSplitState _state;
        private int _encounters;

        public FFXUIComponent(LiveSplitState state)
        {
            this.ContextMenuControls = new Dictionary<string, Action>();
            this.InternalComponent = new InfoTextComponent("Encounter Count", "0");

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

            if (invalidator != null && this.InternalComponent.InformationValue != encounters)
            {
                this.InternalComponent.InformationValue = encounters;
                invalidator.Invalidate(0f, 0f, width, height);
            }

        }

        void PrepareDraw(LiveSplitState state)
        {
            this.InternalComponent.NameLabel.ForeColor = state.LayoutSettings.TextColor;
            this.InternalComponent.ValueLabel.ForeColor = state.LayoutSettings.TextColor;
            this.InternalComponent.NameLabel.HasShadow = this.InternalComponent.ValueLabel.HasShadow = state.LayoutSettings.DropShadows;
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region region)
        {
            this.PrepareDraw(state);
            this.InternalComponent.DrawVertical(g, state, width, region);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float width, Region region)
        {
            this.PrepareDraw(state);
            this.InternalComponent.DrawHorizontal(g, state, width, region);
        }

        void state_OnReset(object sender, TimerPhase t)
        {
            _encounters = 0;
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            return document.CreateElement("Settings");
        }

        public Control GetSettingsControl(LayoutMode mode)
        {
            return null;
        }

        public void SetSettings(XmlNode settings)
        {

        }
    }
}
