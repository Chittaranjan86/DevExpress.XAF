﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using DevExpress.ExpressApp.DC;

namespace Xpand.Extensions.XAF.TypesInfoExtensions{
    public static partial class TypesInfoExtensions{
	    public static bool RuntimeMode(this ITypesInfo typeInfo){
            var devProcceses = new[]{".ExpressApp.ModelEditor", "devenv","Xpand.XAF.ModelEditor"};
            var processName = Process.GetCurrentProcess().ProcessName;
            var isInProccess = devProcceses.Any(s => processName.IndexOf(s, StringComparison.Ordinal) > -1);
            return !isInProccess && LicenseManager.UsageMode != LicenseUsageMode.Designtime;
        }
    }
}