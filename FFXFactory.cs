using LiveSplit.FFX;
using LiveSplit.UI.Components;
using LiveSplit.Model;
using System;
using System.Reflection;

[assembly: ComponentFactory(typeof(FFXFactory))]

namespace LiveSplit.FFX
{
    class FFXFactory : IComponentFactory
    {
        public string ComponentName => "Final Fantasy X Autosplitter";
        public string Description => "Automates splitting and load removal for FFX HD Remaster (PC)";
        public ComponentCategory Category => ComponentCategory.Control;
        public IComponent Create(LiveSplitState state)
        {
            return new FFXComponent(state);
        }

        public string UpdateName => this.ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/BitPatty/LiveSplit.FFX/devel/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => this.UpdateURL + "Components/update.LiveSplit.FFX.xml";
    }
}
