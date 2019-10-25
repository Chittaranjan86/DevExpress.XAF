﻿using DevExpress.ExpressApp;
using Xpand.Extensions.XAF.Module;


namespace TestApplication.Win{
    public class WinModule:ModuleBase{
        public WinModule(){
            this.AddModulesFromPath("Xpand.XAF.Modules*.dll");
            this.AddModulesFromPath("DevExpress.ExpressApp*.dll");
            AdditionalExportedTypes.Add(typeof(Customer));
        }
    }
}