using System;
using System.Drawing;
using System.Windows.Forms;
using LiveSplit.UI;
using System.Xml;
using StringList = System.Collections.Generic.List<string>;
using System.Reflection;

namespace LiveSplit.FFX.UI
{
    public partial class FFXUISettings : UserControl
    {
        public Color BackgroundColor1 { get; set; }
        public Color BackgroundColor2 { get; set; }
        public GradientType BackgroundGradient { get; set; }
        public Color TextColor { get; set; }
        public bool OverrideTextColor { get; set; }

        public string GradientString
        {
            get { return BackgroundGradient.ToString(); }
            set { BackgroundGradient = (GradientType)Enum.Parse(typeof(GradientType), value); }
        }

        public string Version { get; set; }
        public Counter[] CounterData { get; set; }
        public StringList ValueList { get; set; }
        public string TypeString { get; set; }
        public string ValueString { get; set; }
        public int CounterIndex { get; set; }
        public LayoutMode Mode { get; set; }

        public event EventHandler<int> OnSelectionChanged;
        public event EventHandler<Tuple<int, string>> OnSelectionLoaded;

        public FFXUISettings(Counter[] counterData)
        {
            this.CounterData = new Counter[counterData.Length];
            Array.Copy(counterData, CounterData, counterData.Length);

            InitializeComponent();

            this.ValueList = new StringList();
            foreach (Counter item in this.CounterData)
                this.ValueList.Add(item.Text);

            foreach (CategoryType categoryType in Enum.GetValues(typeof(CategoryType)))
                cmbType.Items.Add(categoryType.ToString("G"));

            TextColor = Color.FromArgb(255, 255, 255);
            OverrideTextColor = false;
            BackgroundColor1 = Color.Transparent;
            BackgroundColor2 = Color.Transparent;
            BackgroundGradient = GradientType.Plain;

            cbOverrideTextColor.DataBindings.Add("Checked", this, "OverrideTextColor", false, DataSourceUpdateMode.OnPropertyChanged);
            btnBackgroundColor1.DataBindings.Add("BackColor", this, "BackgroundColor1", false, DataSourceUpdateMode.OnPropertyChanged);
            btnBackgroundColor2.DataBindings.Add("BackColor", this, "BackgroundColor2", false, DataSourceUpdateMode.OnPropertyChanged);
            btnTextColor.DataBindings.Add("BackColor", this, "TextColor", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbGradientType.DataBindings.Add("SelectedItem", this, "GradientString", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbValue.DataBindings.Add("SelectedItem", this, "ValueString", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbType.DataBindings.Add("SelectedItem", this, "TypeString", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        void UISettings_Load(object sender, EventArgs e)
        {
            cbOverrideTextColor.Checked = OverrideTextColor;
            cbOverrideTextColor_CheckedChanged(null, null);

            if (TypeString != null && TypeString != "")
            {
                cmbType.SelectedItem = TypeString;
                SetupCmbValue();
            }
        }

        void SetupCmbValue()
        {
            cmbValue.Items.Clear();

            CategoryType cat = (CategoryType)Enum.Parse(typeof(CategoryType), TypeString, true);

            foreach (Counter item in CounterData)
            {
                if (item.Category == cat)
                    cmbValue.Items.Add(item.Text);
            }

            if (ValueString != null && ValueString != "" && cmbValue.Items.Contains(ValueString))
                cmbValue.SelectedItem = ValueString;
            else
                cmbValue.SelectedIndex = 0;

            cmbValue_SelectedIndexChanged(null, null);
        }

        void cbOverrideTextColor_CheckedChanged(object sender, EventArgs e)
        {
            lblTextColor.Enabled = btnTextColor.Enabled = cbOverrideTextColor.Checked;
        }

        void cmbGradientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnBackgroundColor1.Visible = cmbGradientType.SelectedItem.ToString() != "Plain";
            btnBackgroundColor2.DataBindings.Clear();
            btnBackgroundColor2.DataBindings.Add("BackColor", this, btnBackgroundColor1.Visible ? "BackgroundColor2" : "BackgroundColor1", false, DataSourceUpdateMode.OnPropertyChanged);
            GradientString = cmbGradientType.SelectedItem.ToString();
        }

        void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem != null)
            {
                TypeString = cmbType.SelectedItem.ToString();
                SetupCmbValue();
            }
        }

        void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbValue.SelectedItem != null)
            {
                ValueString = cmbValue.SelectedItem.ToString();

                this.CounterIndex = ValueList.IndexOf(ValueString);
                this.OnSelectionChanged?.Invoke(this, CounterIndex);
            }
        }

        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;

            Version = SettingsHelper.ParseString(element["Version"]);

            if (Version == "")
            {
                // Version 1.0.0 Upgrade
                this.ValueString = "Encounter Count";
            }
            else
            {
                this.TextColor = SettingsHelper.ParseColor(element["TextColor"]);
                this.OverrideTextColor = SettingsHelper.ParseBool(element["OverrideTextColor"]);
                this.BackgroundColor1 = SettingsHelper.ParseColor(element["BackgroundColor1"]);
                this.BackgroundColor2 = SettingsHelper.ParseColor(element["BackgroundColor2"]);
                this.ValueString = SettingsHelper.ParseString(element["ValueString"]);
                try
                {
                    this.GradientString = SettingsHelper.ParseString(element["BackgroundGradient"]);
                }
                catch (ArgumentException)
                {
                    BackgroundGradient = GradientType.Plain;
                }
            }


            // Upgrade safety
            if (!(this.ValueList.Contains(ValueString)))
            {
                this.ValueString = "";
            }
            else if (this.ValueString != "")
            {
                this.CounterIndex = ValueList.IndexOf(ValueString);
                this.TypeString = CounterData[CounterIndex].Category.ToString("G");
                this.OnSelectionLoaded?.Invoke(this, new Tuple<int, string>(CounterIndex, ValueString));
            }
        }


        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");

            SettingsHelper.CreateSetting(document, parent, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
            SettingsHelper.CreateSetting(document, parent, "TextColor", TextColor);
            SettingsHelper.CreateSetting(document, parent, "OverrideTextColor", OverrideTextColor);
            SettingsHelper.CreateSetting(document, parent, "BackgroundColor1", BackgroundColor1);
            SettingsHelper.CreateSetting(document, parent, "BackgroundColor2", BackgroundColor2);
            SettingsHelper.CreateSetting(document, parent, "BackgroundGradient", BackgroundGradient);
            SettingsHelper.CreateSetting(document, parent, "ValueString", ValueString);

            return parent;
        }


        private void ColorButtonClick(object sender, EventArgs e)
        {
            SettingsHelper.ColorButtonClick((Button)sender, this);
        }
    }
}
