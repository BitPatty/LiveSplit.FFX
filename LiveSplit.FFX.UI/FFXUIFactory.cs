using LiveSplit.FFX.UI;
using LiveSplit.UI.Components;
using LiveSplit.Model;
using System;
using System.Reflection;

[assembly: ComponentFactory(typeof(FFXUIFactory))]

namespace LiveSplit.FFX.UI
{
  internal class FFXUIFactory : IComponentFactory
  {
    public string ComponentName => "FFX Display";
    public string Description => "Additional features for the Final Fantasy X Autosplitter (PC HD Remaster)";
    public ComponentCategory Category => ComponentCategory.Information;
    public IComponent Create(LiveSplitState state) => new FFXUIComponent(state);
    public string UpdateName => ComponentName;
    public string UpdateURL => "https://raw.githubusercontent.com/BitPatty/LiveSplit.FFX/master/LiveSplit.FFX.UI/";
    public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    public string XMLURL => UpdateURL + "Components/update.LiveSplit.FFX.UI.xml";
  }
}
