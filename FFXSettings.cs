using System;
using System.Windows.Forms;
using System.Xml;
using StringList = System.Collections.Generic.List<string>;

namespace LiveSplit.FFX
{
  public partial class FFXSettings : UserControl
  {
    public bool Start { get; set; }
    public bool Split { get; set; }
    public bool Reset { get; set; }
    public bool RemoveLoads { get; set; }

    public bool HasChanged;

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

      HasChanged = true;

      checkboxStart.DataBindings.Add("Checked", this, "Start", false, DataSourceUpdateMode.OnPropertyChanged);
      checkboxReset.DataBindings.Add("Checked", this, "Reset", false, DataSourceUpdateMode.OnPropertyChanged);
      checkboxSplit.DataBindings.Add("Checked", this, "Split", false, DataSourceUpdateMode.OnPropertyChanged);
      checkboxRemoveLoads.DataBindings.Add("Checked", this, "RemoveLoads", false, DataSourceUpdateMode.OnPropertyChanged);
    }

    /// <summary>
    /// Updates the list with the activated splits everytime the settings window is closed.
    /// </summary>
    private void ConfirmSplits(object sender, EventArgs e) => HasChanged = true;

    /// <summary>
    /// Returns a StringList of the activated splits and resets the appropriate flag.
    /// </summary>
    public StringList GetSplits()
    {
      StringList _activatedSplits = new StringList();     // List of selected splits

      foreach (ListViewItem listViewItem in listView.Items)
      {
        if (listViewItem.Checked)
          _activatedSplits.Add(listViewItem.Text);
      }
      HasChanged = false;
      return _activatedSplits;
    }

    /// <summary>
    /// Generates the XML serialization of the component's settings.
    /// </summary>
    /// <param name="doc">XML Document</param>
    /// <returns>Returns the XML serialization of the component's settings.</returns>
    public XmlNode GetSettings(XmlDocument doc)
    {
      XmlElement settingsNode = doc.CreateElement("Settings");

      settingsNode.AppendChild(ToElement(doc, "Start", Start));
      settingsNode.AppendChild(ToElement(doc, "Reset", Reset));
      settingsNode.AppendChild(ToElement(doc, "Split", Split));
      settingsNode.AppendChild(ToElement(doc, "RemoveLoads", RemoveLoads));

      foreach (ListViewItem listViewItem in listView.Items)
        settingsNode.AppendChild(ToElement(doc, listViewItem.Text, listViewItem.Checked));

      return settingsNode;
    }

    /// <summary>
    /// Sets the component settings based on the serialized version of the settings.
    /// </summary>
    public void SetSettings(XmlNode settings)
    {
      Start = ParseBool(settings, "Start");
      Split = ParseBool(settings, "Split");
      Reset = ParseBool(settings, "Reset");
      RemoveLoads = ParseBool(settings, "RemoveLoads");

      foreach (ListViewItem listViewItem in listView.Items)
        listViewItem.Checked = ParseBool(settings, listViewItem.Text);

      HasChanged = true;
    }

    /// <summary>
    /// Parses the boolean values based on the serialized version of the settings.
    /// </summary>
    private static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
      => settings[setting] != null ? (Boolean.TryParse(settings[setting].InnerText, out bool val) ? val : default_) : default_;

    /// <summary>
    /// Returns a serialized version of a setting based on its identifier.
    /// </summary>
    private static XmlElement ToElement<T>(XmlDocument document, string name, T value)
    {
      XmlElement str = document.CreateElement(name);
      str.InnerText = value.ToString();
      return str;
    }

    private void BtnCheckAll_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in listView.Items)
        listViewItem.Checked = true;
    }

    private void BtnUncheckAll_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in listView.Items)
        listViewItem.Checked = false;
    }
  }
}
