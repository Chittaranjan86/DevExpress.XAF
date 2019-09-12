using System;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Utils.Extensions;
using Xpand.XAF.Modules.Reactive;

namespace Xpand.XAF.Modules.OneView{
    public interface IModelReactiveModuleOneView : IModelReactiveModule{
        IModelOneView OneView{ get; }
    }

    public interface IModelOneView : IModelNode{
        [DataSourceProperty("Application.Views")]
        IModelView View{ get; set; }
    }

    
    public static class ModelReactiveModuleOneView{
        public static IObservable<IModelOneView> OneViewModel(this IObservable<IModelReactiveModules> source){
            return source.Select(modules => modules.OneViewModel());
        }

        public static IModelOneView OneViewModel(this IModelReactiveModules reactiveModules){
            return reactiveModules.CastTo<IModelReactiveModuleOneView>().OneView;
        }

    }
}