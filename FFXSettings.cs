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

            foreach(ListViewItem listViewItem in listView.Items)
                listViewItem.Checked = true;


            this.checkboxStart.DataBindings.Add("Checked", this, "Start", false, DataSourceUpdateMode.OnPropertyChanged);
            this.checkboxReset.DataBindings.Add("Checked", this, "Reset", false, DataSourceUpdateMode.OnPropertyChanged);
            this.checkboxSplit.DataBindings.Add("Checked", this, "Split", false, DataSourceUpdateMode.OnPropertyChanged);
            this.checkboxRemoveLoads.DataBindings.Add("Checked", this, "RemoveLoads", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private StringList _activatedSplits = new StringList();
        public bool hasChanged = true;

        private void ConfirmSplits(object sender, EventArgs e)
        {
            _activatedSplits.Clear();

            foreach(ListViewItem listViewItem in listView.Items)
            {
                if(listViewItem.Checked)
                    _activatedSplits.Add(listViewItem.Text);
            }

            hasChanged = true;
        }

        public StringList GetSplits()
        {
            hasChanged = false;
            return _activatedSplits;
        }

        //Get settings from config 
        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");

            _activatedSplits.Clear();

            //settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settingsNode.AppendChild(ToElement(doc, "Start", this.Start));
            settingsNode.AppendChild(ToElement(doc, "Reset", this.Reset));
            settingsNode.AppendChild(ToElement(doc, "Split", this.Split));
            settingsNode.AppendChild(ToElement(doc, "RemoveLoads", this.RemoveLoads));

            foreach (ListViewItem listViewItem in listView.Items)
            {
                settingsNode.AppendChild(ToElement(doc, listViewItem.Text, listViewItem.Checked));
                if (listViewItem.Checked)
                    _activatedSplits.Add(listViewItem.Text);
            }

            return settingsNode;
        }

        //Store settings in config
        public void SetSettings(XmlNode settings)
        {
            this.Start = ParseBool(settings, "Start");
            this.Split = ParseBool(settings, "Split");
            this.Reset = ParseBool(settings, "Reset");
            this.RemoveLoads = ParseBool(settings, "RemoveLoads");

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
