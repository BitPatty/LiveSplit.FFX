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
        public string Text { get; set; }                    // Display Name
        public int Count { get; set; }                      // Display Count
        public int Offset { get; set; }                     // Offset of Count
        public CategoryType Category { get; set; }         // Category for Dropdown

        public int IndexOffset { get; set; }                // Index offset if Offset is an indexed address (not pointers, used for items)
        public int IndexBufferSize { get; set; }            // Area to search for Index
        public int Index { get; set; }                      // Index
        public int IndexKey { get; set; }                   // Key

        public int Size { get { return this._size; } set { if (value >= 4) this._size = 0x7fffffff; else this._size = (1 << (8 * value)) - 1; } }       // Memory type (could use MemoryWatcher<T> instead)
        public MemoryWatcher Watcher { get; set; }          // MemoryWatcher for Count
    }

    public enum CategoryType
    {
        Item,
        Affection,
        Misc
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
        public string defaultDisplayName { get; set; }
        public string displayValue { get; set; }
        public string defaultDisplayValue { get; set; }
        public int counterIndex { get; set; }

        // Possible values to display, new values can be added here
        public Counter[] counterData = new Counter[]
            {
                // Misc
                new Counter { Text = "Encounter Count",     Offset = 0xD307A4, Category = CategoryType.Misc, Size = sizeof(int) },
                new Counter { Text = "Gil",                 Offset = 0xD307D8, Category = CategoryType.Misc, Size = sizeof(int) },

                // Items
                new Counter { Text = "Arctic Wind",         Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x18, Size = sizeof(byte) },
                new Counter { Text = "Ice Gems",            Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x19, Size = sizeof(byte) },
                new Counter { Text = "Bomb Cores",          Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x1B, Size = sizeof(byte) },
                new Counter { Text = "Fire Gems",           Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x1C, Size = sizeof(byte) },
                new Counter { Text = "Lightning Marbles",   Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x1E, Size = sizeof(byte) },
                new Counter { Text = "Lightning Gems",      Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x1F, Size = sizeof(byte) },
                new Counter { Text = "Fish Scales",         Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x20, Size = sizeof(byte) },
                new Counter { Text = "Water Gems",          Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x22, Size = sizeof(byte) },
                new Counter { Text = "Sleeping Powders",    Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x25, Size = sizeof(byte) },
                new Counter { Text = "Power Spheres",       Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x46, Size = sizeof(byte) },
                new Counter { Text = "Mana Spheres",        Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x47, Size = sizeof(byte) },
                new Counter { Text = "Speed Spheres",       Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x48, Size = sizeof(byte) },
                new Counter { Text = "Ability Spheres",     Offset = 0xD30B5C, Category = CategoryType.Item, IndexOffset = 0xD3095C, IndexBufferSize = 224, IndexKey = 0x49, Size = sizeof(byte) },

                // Affection Levels
                new Counter { Text = "Affection Yuna",      Offset = 0xD2CAC0, Category = CategoryType.Affection, Size = sizeof(int) },
                new Counter { Text = "Affection Auron",     Offset = 0xD2CAC4, Category = CategoryType.Affection, Size = sizeof(int) },
                new Counter { Text = "Affection Kimahri",   Offset = 0xD2CAC8, Category = CategoryType.Affection, Size = sizeof(int) },
                new Counter { Text = "Affection Wakka",     Offset = 0xD2CACC, Category = CategoryType.Affection, Size = sizeof(int) },
                new Counter { Text = "Affection Lulu",      Offset = 0xD2CAD0, Category = CategoryType.Affection, Size = sizeof(int) },
                new Counter { Text = "Affection Rikku",     Offset = 0xD2CAD4, Category = CategoryType.Affection, Size = sizeof(int) }
            };

        //Init
        public FFXUIComponent(LiveSplitState state)
        {
            this.defaultDisplayName = "Select Value";
            this.defaultDisplayValue = "0";

            this.ContextMenuControls = new Dictionary<string, Action>();
            this.InternalComponent = new InfoTextComponent(this.defaultDisplayName, this.defaultDisplayValue);

            this.UISettings = new FFXUISettings(counterData);
            UISettings.OnSelectionChanged += uiSettings_OnSelectionChanged;
            UISettings.OnSelectionLoaded += uiSettings_OnSelectionLoaded;

            _state = state;
            _state.OnReset += state_OnReset;

            _gameMemory = new FFXUIMemory(counterData);
            _gameMemory.OnValueChanged += gameMemory_OnValueChanged;

            _updateTimer = new Timer() { Interval = 1000, Enabled = true };
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
                this.InternalComponent.InformationName = this.displayName == null ? this.defaultDisplayName : this.displayName;
                this.InternalComponent.InformationValue = this.displayValue == null ? this.defaultDisplayValue : this.displayValue;
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
            catch (ProcessLostException)
            {
                // Reset counters
                for (int i = 0; i < counterData.Length; i++)
                    counterData[i].Count = 0;

                this.displayValue = this.defaultDisplayValue;
            }
            catch (Exception)
            {
                // Any
            }
        }

        void gameMemory_OnValueChanged(object sender, Tuple<int, int> valueData)
        {
            int index = valueData.Item1;
            int count = valueData.Item2;

            this.counterData[index].Count = count;

            if (index == this.counterIndex && !(this.displayName.Equals(this.defaultDisplayName)))
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

        void uiSettings_OnSelectionLoaded(object sender, Tuple<int, string> selectionData)
        {
            this.counterIndex = selectionData.Item1;

            // If index or value order changes because of FFXUI updates
            if (this.counterIndex >= counterData.Length || !(counterData[counterIndex].Text.Equals(selectionData.Item2)))
            {
                int newIndex;

                try
                {
                    // Try to find new Index of old value
                    newIndex = Array.FindIndex(counterData, item => item.Text.Equals(selectionData.Item2));
                }
                catch (ArgumentNullException)
                {
                    // Reset display if value not found
                    this.counterIndex = 0;
                    this.displayName = this.defaultDisplayName;
                    this.displayValue = this.defaultDisplayValue;
                    return;
                }

                this.counterIndex = newIndex;
            }

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
