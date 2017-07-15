using System;
using System.Drawing;
using System.Windows.Forms;
using LiveSplit.UI;
using System.Xml;
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
        public LayoutMode Mode { get; set; }

        public FFXUISettings()
        {
            InitializeComponent();

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

        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;

            Version = SettingsHelper.ParseString(element["Version"]);

            if (!String.IsNullOrEmpty(Version))
            {
                TextColor = SettingsHelper.ParseColor(element["TextColor"]);
                OverrideTextColor = SettingsHelper.ParseBool(element["OverrideTextColor"]);
                BackgroundColor1 = SettingsHelper.ParseColor(element["BackgroundColor1"]);
                BackgroundColor2 = SettingsHelper.ParseColor(element["BackgroundColor2"]);
                try
                {
                    GradientString = SettingsHelper.ParseString(element["BackgroundGradient"]);
                }
                catch (ArgumentException)
                {
                    BackgroundGradient = GradientType.Plain;
                }
            }
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        private int CreateSettingsNode(XmlDocument document, XmlElement parent)
        {
            return SettingsHelper.CreateSetting(document, parent, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)) ^
            SettingsHelper.CreateSetting(document, parent, "TextColor", TextColor) ^
            SettingsHelper.CreateSetting(document, parent, "OverrideTextColor", OverrideTextColor) ^
            SettingsHelper.CreateSetting(document, parent, "BackgroundColor1", BackgroundColor1) ^
            SettingsHelper.CreateSetting(document, parent, "BackgroundColor2", BackgroundColor2) ^
            SettingsHelper.CreateSetting(document, parent, "BackgroundGradient", BackgroundGradient);
        }

        private void ColorButtonClick(object sender, EventArgs e)
        {
            SettingsHelper.ColorButtonClick((Button)sender, this);
        }
    }
}
