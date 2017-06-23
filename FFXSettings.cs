using System;
using System.Windows.Forms;
using System.Xml;
using StringList = System.Collections.Generic.List<string>;

namespace LiveSplit.FFX
{
    public partial class FFXSettings : UserControl
    {
        //Settings
        public bool Start { get; set; }
        public bool Split { get; set; }
        public bool Reset { get; set; }
        public bool RemoveLoads { get; set; }

        public FFXSettings()
        {
            InitializeComponent();

            listView.Items.Add(FFXComponent.SINSPAWN_AMMES);
            listView.Items.Add(FFXComponent.KLIKK);
            listView.Items.Add(FFXComponent.TROS);
            listView.Items.Add(FFXComponent.PIRANHAS);
            listView.Items.Add(FFXComponent.KIMAHRI);
            listView.Items.Add(FFXComponent.SINSPAWN_ECHUILLES);
            listView.Items.Add(FFXComponent.SINSPAWN_GENEAUX);
            listView.Items.Add(FFXComponent.KILIKA_WOODS);
            listView.Items.Add(FFXComponent.OBLITZERATOR);
            listView.Items.Add(FFXComponent.BLITZBALL_COMPLETE);
            listView.Items.Add(FFXComponent.GARUDA);
            listView.Items.Add(FFXComponent.MIIHEN_HIGHROAD);
            listView.Items.Add(FFXComponent.OLD_ROAD);
            listView.Items.Add(FFXComponent.MUSHROOM_ROCK_ROAD);
            listView.Items.Add(FFXComponent.SINSPAWN_GUI);
            listView.Items.Add(FFXComponent.DJOSE_HIGHROAD);
            listView.Items.Add(FFXComponent.MOONFLOW);
            listView.Items.Add(FFXComponent.EXTRACTOR);
            listView.Items.Add(FFXComponent.GUADOSALAM);
            listView.Items.Add(FFXComponent.THUNDER_PLAINS);
            listView.Items.Add(FFXComponent.SPHERIMORPH);
            listView.Items.Add(FFXComponent.CRAWLER);
            listView.Items.Add(FFXComponent.SEYMOUR);
            listView.Items.Add(FFXComponent.WENDIGO);
            listView.Items.Add(FFXComponent.HOME);
            listView.Items.Add(FFXComponent.EVRAE);
            listView.Items.Add(FFXComponent.BEVELLE_GUARDS);
            listView.Items.Add(FFXComponent.BEVELLE_TRIALS);
            listView.Items.Add(FFXComponent.ISAARU);
            listView.Items.Add(FFXComponent.SEYMOUR_NATUS);
            listView.Items.Add(FFXComponent.BIRAN_YENKE);
            listView.Items.Add(FFXComponent.SEYMOUR_FLUX);
            listView.Items.Add(FFXComponent.SANCTUARY_KEEPER);
            listView.Items.Add(FFXComponent.YUNALESCA);
            listView.Items.Add(FFXComponent.SIN_CORE);
            listView.Items.Add(FFXComponent.OVERDRIVE_SIN);
            listView.Items.Add(FFXComponent.SEYMOUR_OMNIS);
            listView.Items.Add(FFXComponent.BRASKAS_FINAL_AEON);
            listView.Items.Add(FFXComponent.YU_YEVON);

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
