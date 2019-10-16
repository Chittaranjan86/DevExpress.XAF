﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fasterflect;
using HarmonyLib;
using Xpand.Extensions.Linq;

namespace Xpand.Extensions.XAF.XafApplication{
    public static partial class XafApplicationExtensions{
        public static void AddAdditionalSecuredType(this DevExpress.ExpressApp.XafApplication application,params Type[] objectTypes){
            if (application.Security != null && application.Security.GetType().FromHierarchy(type => type.BaseType)
                    .Any(type => type.Name == "SecurityStrategy")){
                
                var isSecuredTypeMethod = application.Security.GetType().Method("IsSecuredType",Flags.Static|Flags.Public);
                var postfix = new HarmonyMethod(typeof(XafApplicationExtensions).Method(nameof(IsSecuredType),Flags.Static|Flags.NonPublic));
                _harmony.Patch(isSecuredTypeMethod,postfix);
                
                var types = ((IList<Type>) _additionalSecuredTypes.Invoke(null));
                types.AddRange(objectTypes);
            }
        }

        // ReSharper disable once InconsistentNaming
        private static bool IsSecuredType(ref bool __result, Type type){
            if (((IList<Type>) _additionalSecuredTypes.Invoke(null)).Contains(type)){
                __result = false;
                return false;
            }

            return true;
        }

    }
}