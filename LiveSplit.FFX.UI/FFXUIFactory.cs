﻿using LiveSplit.UI.Components;
using LiveSplit.Model;
using System;
using System.Reflection;

namespace LiveSplit.FFX.UI
{
    class FFXUIFactory : IComponentFactory
    {
        public string ComponentName => "FFX Display";
        public string Description => "Additional features for the Final Fantasy X Autosplitter (PC HD Remaster)";
        public ComponentCategory Category => ComponentCategory.Information;
        public IComponent Create(LiveSplitState state)
        {
            return new FFXUIComponent(state);
        }

        public string UpdateName => this.ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/BitPatty/LiveSplit.FFX/devel/LiveSplit.FFX.UI/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => this.UpdateURL + "Components/update.LiveSplit.FFX.UI.xml";
    }
}
