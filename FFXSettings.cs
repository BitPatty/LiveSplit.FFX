using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.UI.Components;
using System.Collections.Generic;
using StringList = System.Collections.Generic.List<string>;

namespace LiveSplit.FFX
{
    public partial class FFXSettings : UserControl //ComponentSettings
    {
        //Settings
        public bool Start { get; set; }
        public bool Split { get; set; }
        public bool Reset { get; set; }
        public bool RemoveLoads { get; set; }

        //private Dictionary<string, CheckBox> _basic_settings;

        public FFXSettings()
        {
            InitializeComponent();

            listView.Items.Add("SinspawnAmmes");
            listView.Items.Add("Klikk");
            listView.Items.Add("Tros");
            listView.Items.Add("Piranhas");
            listView.Items.Add("Kimahri");
            listView.Items.Add("SinspawnEchuilles");
            listView.Items.Add("SinspawnGeneaux");
            listView.Items.Add("KilikaWoods");
            listView.Items.Add("Oblitzerator");
            listView.Items.Add("BlitzballComplete");
            listView.Items.Add("Garuda");
            listView.Items.Add("MiihenHighroad");
            listView.Items.Add("OldRoad");
            listView.Items.Add("MushroomRockRoad");
            listView.Items.Add("SinspawnGui");
            listView.Items.Add("DjoseHighroad");
            listView.Items.Add("Moonflow");
            listView.Items.Add("Extractor");
            listView.Items.Add("Guadosalam");
            listView.Items.Add("ThunderPlains");
            listView.Items.Add("Spherimorph");
            listView.Items.Add("Crawler");
            listView.Items.Add("Seymour");
            listView.Items.Add("Wendigo");
            listView.Items.Add("Home");
            listView.Items.Add("Evrae");
            listView.Items.Add("BevelleGuards");
            listView.Items.Add("BevelleTrials");
            listView.Items.Add("Isaaru");
            listView.Items.Add("SeymourNatus");
            listView.Items.Add("BiranYenke");
            listView.Items.Add("SeymourFlux");
            listView.Items.Add("SanctuaryKeeper");
            listView.Items.Add("Yunalesca");
            listView.Items.Add("SinCore");
            listView.Items.Add("OverdriveSin");
            listView.Items.Add("SeymourOmnis");
            listView.Items.Add("BraskasFinalAeon");
            listView.Items.Add("YuYevon");

            foreach (ListViewItem listViewItem in listView.Items)
                listViewItem.Checked = true;

        }

        public StringList GetSplits()
        {
            StringList splitList = new StringList();
            foreach (ListViewItem listViewItem in listView.Items)
            {
                if (listViewItem.Checked == true)
                    splitList.Add(listViewItem.Text);
            }
            return splitList;
        }

        //Get settings from config 
        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");

            //settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settingsNode.AppendChild(ToElement(doc, "Start", this.checkboxStart.Checked));
            settingsNode.AppendChild(ToElement(doc, "Reset", this.checkboxReset.Checked));
            settingsNode.AppendChild(ToElement(doc, "Split", this.checkboxSplit.Checked));
            settingsNode.AppendChild(ToElement(doc, "RemoveLoads", this.checkboxRemoveLoads.Checked));

            foreach (ListViewItem listViewItem in listView.Items)
                settingsNode.AppendChild(ToElement(doc, listViewItem.Text, listViewItem.Checked));

            return settingsNode;
        }

        //Store settings in config
        public void SetSettings(XmlNode settings)
        {
            this.Start = ParseBool(settings, "Start", true);
            this.Split = ParseBool(settings, "Reset", true);
            this.Reset = ParseBool(settings, "Split", true);
            this.RemoveLoads = ParseBool(settings, "RemoveLoads", true);

            foreach (ListViewItem listViewItem in listView.Items)
                listViewItem.Checked = ParseBool(settings, listViewItem.Text);
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

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listView.Items)
                listViewItem.Checked = true;
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listView.Items)
                listViewItem.Checked = false;
        }
    }
}
