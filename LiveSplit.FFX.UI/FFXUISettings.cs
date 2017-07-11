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

        public string ValueString { get; set; }
        public int ValueIndex { get; set; }
        public LayoutMode Mode { get; set; }

        public event EventHandler<int> OnSelectionChanged;

        public FFXUISettings(StringList valueList)
        {
            InitializeComponent();

            foreach (string Item in valueList)
            {
                cmbValue.Items.Add(Item);
            }

            TextColor = Color.FromArgb(255, 255, 255);
            OverrideTextColor = false;
            BackgroundColor1 = Color.Transparent;
            BackgroundColor2 = Color.Transparent;
            BackgroundGradient = GradientType.Plain;

            //cmbGradientType.SelectedIndexChanged += cmbGradientType_SelectedIndexChanged;
            //cmbValue.SelectedIndexChanged += cmbValue_SelectedIndexChanged;

            cbOverrideTextColor.DataBindings.Add("Checked", this, "OverrideTextColor", false, DataSourceUpdateMode.OnPropertyChanged);
            btnBackgroundColor1.DataBindings.Add("BackColor", this, "BackgroundColor1", false, DataSourceUpdateMode.OnPropertyChanged);
            btnBackgroundColor2.DataBindings.Add("BackColor", this, "BackgroundColor2", false, DataSourceUpdateMode.OnPropertyChanged);
            btnTextColor.DataBindings.Add("BackColor", this, "TextColor", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbGradientType.DataBindings.Add("SelectedItem", this, "GradientString", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbValue.DataBindings.Add("SelectedItem", this, "ValueString", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        void UISettings_Load(object sender, EventArgs e)
        {
            cbOverrideTextColor.Checked = OverrideTextColor;
            cbOverrideTextColor_CheckedChanged(null, null);
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

        void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValueString = cmbValue.SelectedItem.ToString();
            ValueIndex = cmbValue.SelectedIndex;
            this.OnSelectionChanged?.Invoke(this, ValueIndex);
        }

        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;
            TextColor = SettingsHelper.ParseColor(element["TextColor"]);
            OverrideTextColor = SettingsHelper.ParseBool(element["OverrideTextColor"]);
            BackgroundColor1 = SettingsHelper.ParseColor(element["BackgroundColor1"]);
            BackgroundColor2 = SettingsHelper.ParseColor(element["BackgroundColor2"]);
            GradientString = SettingsHelper.ParseString(element["BackgroundGradient"]);
            ValueString = SettingsHelper.ParseString(element["ValueString"]);
            ValueIndex = SettingsHelper.ParseInt(element["ValueIndex"]);

            this.OnSelectionChanged?.Invoke(this, ValueIndex);
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
            SettingsHelper.CreateSetting(document, parent, "ValueIndex", ValueIndex);

            return parent;
        }


        private void ColorButtonClick(object sender, EventArgs e)
        {
            SettingsHelper.ColorButtonClick((Button)sender, this);
        }
    }
}
