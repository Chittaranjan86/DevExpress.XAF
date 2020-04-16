﻿using System;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace Xpand.Extensions.AppDomain{
    public static partial class AppDomainExtensions{
        public static System.Reflection.Assembly AssemblySystemWeb(this global::System.AppDomain appDomain){
            return appDomain.GetAssemblies().FirstOrDefault(_ => _.GetName().Name == "System.Web");
        }
        public static System.Type TypeUnit(this System.Reflection.Assembly assembly){
            return assembly.GetType("System.Web.UI.WebControls.Unit");
        }
        public static MethodInvoker Percentage(this System.Type type){
            return type.GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.Static).First(info => info.Name=="Percentage").DelegateForCallMethod();
        }

    }
}