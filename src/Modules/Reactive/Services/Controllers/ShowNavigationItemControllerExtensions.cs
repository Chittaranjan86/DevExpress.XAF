﻿using System;
using System.Reactive.Linq;
using DevExpress.ExpressApp.SystemModule;
using Xpand.XAF.Modules.Reactive.Extensions;

namespace Xpand.XAF.Modules.Reactive.Services.Controllers{
    public static class ShowNavigationItemControllerExtensions{
        public static IObservable<(ShowNavigationItemController controller, CustomShowNavigationItemEventArgs e)>
            WhenCustomShowNavigationItem(this ShowNavigationItemController controller){
            return Observable.FromEventPattern<EventHandler<CustomShowNavigationItemEventArgs>, CustomShowNavigationItemEventArgs>(
                    h => controller.CustomShowNavigationItem += h, h => controller.CustomShowNavigationItem -= h)
                .TransformPattern<CustomShowNavigationItemEventArgs, ShowNavigationItemController>();
        }

        public static IObservable<(ShowNavigationItemController controller, CustomGetStartupNavigationItemEventArgs e)>
            WhenCustomGetStartupNavigationItem(this ShowNavigationItemController controller){
            return Observable.FromEventPattern<EventHandler<CustomGetStartupNavigationItemEventArgs>, CustomGetStartupNavigationItemEventArgs>(
                    h => controller.CustomGetStartupNavigationItem += h, h => controller.CustomGetStartupNavigationItem -= h)
                .TransformPattern<CustomGetStartupNavigationItemEventArgs, ShowNavigationItemController>();
        }

    }
}
