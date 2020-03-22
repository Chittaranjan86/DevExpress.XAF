﻿using DevExpress.ExpressApp;

namespace Xpand.Extensions.XAF.XafApplication{
    public static partial class XafApplicationExtensions{
        public static Window CreateViewWindow(this DevExpress.ExpressApp.XafApplication application,bool isMain=true,params Controller[] controllers){
            return application.CreateWindow(TemplateContext.View, controllers, isMain);
        }
        public static Window CreatePopupWindow(this DevExpress.ExpressApp.XafApplication application,bool isMain=true,params Controller[] controllers){
            return application.CreateWindow(TemplateContext.PopupWindow, controllers, isMain);
        }
    }
}