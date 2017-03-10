using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.FFX
{
    public partial class FFXSettings : UserControl
    {
        //Settings
        public bool AutoStart { get; set; }
        public bool UseLegacySplits { get; set; }
        public bool AutoReset { get; set; }
        public bool RemoveLoads { get; set; }

        public FFXSettings()
        {
            InitializeComponent();

            this.checkAutoStart.DataBindings.Add("Checked", this, "AutoStart", false, DataSourceUpdateMode.OnPropertyChanged);
            this.checkLegacySplits.DataBindings.Add("Checked", this, "UseLegacySplits", false, DataSourceUpdateMode.OnPropertyChanged);
            this.checkAutoReset.DataBindings.Add("Checked", this, "AutoReset", false, DataSourceUpdateMode.OnPropertyChanged);
            this.checkRemoveLoads.DataBindings.Add("Checked", this, "RemoveLoads", false, DataSourceUpdateMode.OnPropertyChanged);

            this.checkAutoReset.Checked = true;
            this.checkLegacySplits.Checked = true;
            this.checkLegacySplits.Enabled = false;

            //Defaults
            this.AutoStart = true;
            this.UseLegacySplits = true;
            this.AutoReset = false;
            this.RemoveLoads = false;

            //For future options
            this.checkBox5.Visible = false;
            this.checkBox6.Visible = false;
        }


        //Get settings from config 
        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");

            settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settingsNode.AppendChild(ToElement(doc, "AutoStart", this.AutoStart));
            settingsNode.AppendChild(ToElement(doc, "UseLegacySplits", this.UseLegacySplits));
            settingsNode.AppendChild(ToElement(doc, "AutoReset", this.AutoReset));
            settingsNode.AppendChild(ToElement(doc, "RemoveLoads", this.RemoveLoads));

            return settingsNode;
        }

        //Store settings in config
        public void SetSettings(XmlNode settings)
        {
            this.AutoStart = ParseBool(settings, "AutoStart", true);
            this.UseLegacySplits = ParseBool(settings, "UseLegacySplits");
            this.AutoReset = ParseBool(settings, "AutoReset");
            this.RemoveLoads = ParseBool(settings, "RemoveLoads");
        }

        //Parse settings
        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            bool val;
            return settings[setting] != null ? (Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_) : default_;
        }

        //Convert settings to obj
        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }


    }
}
