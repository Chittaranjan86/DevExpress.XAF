﻿using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using Xpand.Extensions.Assembly;
using Xpand.Extensions.Stream;

namespace Xpand.Extensions.XAF.Module{
    public static partial class ModulebaseExtensions{
        public static IEnumerable<(string id,string model)> EmbeddedModels(this ModuleBase module){
            var assembly = module.GetType().Assembly;
            var defaultModelName = $"{ModelStoreBase.ModelDiffDefaultName}.xafml";
            return assembly.GetName().PublicKeyToken() != AssemblyInfo.PublicKeyToken
                ? assembly.GetManifestResourceNames().Where(s => s.EndsWith(".xafml"))
                    .Where(s => !s.EndsWith(defaultModelName))
                    .Select(s => (id:s,model:assembly.GetManifestResourceStream(s).ReadToEnd()))
                : Enumerable.Empty<(string id,string model)>();
        }
    }
}