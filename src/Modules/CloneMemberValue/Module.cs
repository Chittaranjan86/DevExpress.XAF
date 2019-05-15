﻿using System;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using Xpand.XAF.Modules.Reactive.Extensions;

namespace Xpand.XAF.Modules.CloneMemberValue {
    public sealed partial class CloneMemberValueModule : ModuleBase {
        public CloneMemberValueModule() {
            InitializeComponent();
        }

        public override void Setup(ApplicationModulesManager moduleManager){
            base.Setup(moduleManager);
            moduleManager.Connect(Application)
                .TakeUntil(this.WhenDisposed())
                .Subscribe();
        }

        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders){
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelMember,IModelMemberCloneValue>();
            extenders.Add<IModelPropertyEditor, IModelCommonMemberViewItemCloneValue>();
            extenders.Add<IModelColumn, IModelCommonMemberViewItemCloneValue>();
        }
    }
}
