﻿using System;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using Xpand.XAF.Modules.ModelMapper.Services;
using Xpand.XAF.Modules.Reactive;
using Xpand.XAF.Modules.Reactive.Extensions;

namespace Xpand.XAF.Modules.ModelMapper {
    public sealed class ModelMapperModule : ReactiveModuleBase {
        public ModelMapperModule(){
            RequiredModuleTypes.Add(typeof(ReactiveModule));
        }

        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders){
            base.ExtendModelInterfaces(extenders);
            
            extenders.Add<IModelApplication,IModelApplicationModelMapper>();
        }

        public override void Setup(ApplicationModulesManager moduleManager){
            base.Setup(moduleManager);
            moduleManager.ConnectExtendingService()
                .Merge(moduleManager.ConnectLayoutView())
                .Merge(Application.BindConnect())
                .TakeUntilDisposed(this)
                .Subscribe();
        }

        
    }

    
}
