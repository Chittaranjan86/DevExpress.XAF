﻿using System;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace Xpand.Extensions.AppDomain{
    public static partial class AppDomainExtensions{
        public static System.Reflection.Assembly AssemblyDevExpressExpressAppWeb(this global::System.AppDomain appDomain){
            return appDomain.GetAssemblies().FirstOrDefault(_ => _.GetName().Name.StartsWith("DevExpress.ExpressApp.Web.v"));
        }
        public static Type TypeClientSideEventsHelper(this System.Reflection.Assembly assembly){
            return assembly.GetType("DevExpress.ExpressApp.Web.Utils.ClientSideEventsHelper");
        }
        public static MethodInvoker AsssignClientHanderSafe(this Type type){
            return type.Methods(Flags.Static|Flags.Instance|Flags.Public,"AssignClientHandlerSafe").First(info => info.Parameters().Count==4).DelegateForCallMethod();
        }

    }
}