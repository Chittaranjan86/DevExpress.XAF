﻿using DevExpress.ExpressApp.Model;

namespace Xpand.Extensions.XAF.Model{
    public static partial class ModelExtensions{
        public static IModelView FindModelView(this IModelApplication modelApplication, System.String viewId){
            return modelApplication?.Application.Views?[viewId];
        }
    }
}