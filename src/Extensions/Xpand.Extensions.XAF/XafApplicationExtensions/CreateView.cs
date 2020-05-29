﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using Fasterflect;
using JetBrains.Annotations;

namespace Xpand.Extensions.XAF.XafApplicationExtensions{
    [PublicAPI]
    public static partial class XafApplicationExtensions{
        public static DetailView NewDetailView(this XafApplication application,object currentObject,IModelDetailView modelDetailView=null,bool isRoot=true){
            var objectSpace = application.CreateObjectSpace();
            modelDetailView ??= application.FindModelDetailView(currentObject.GetType());
            var detailView = application.CreateDetailView(objectSpace, modelDetailView,isRoot);
            detailView.CurrentObject = objectSpace.GetObject(currentObject);
            return detailView;
        }

        public static ObjectView NewObjectView(this XafApplication application,
            System.Type viewType,System.Type objectType) {
            if (viewType == typeof(ListView)){
                var listViewId = application.FindListViewId(objectType);
                var collectionSource = application.CreateCollectionSource(application.CreateObjectSpace(),objectType,listViewId);
                return application.CreateListView((IModelListView) application.Model.Views[listViewId], collectionSource, true);
            }
            var modelDetailView = application.Model.BOModel.GetClass(objectType).DefaultDetailView;
            return application.CreateDetailView(application.CreateObjectSpace(), modelDetailView,true);
        }

        public static CompositeView NewView(this XafApplication application,string viewId) => application.NewView(application.Model.Views[viewId]);

        public static CompositeView NewView(this XafApplication application,IModelView modelView,IObjectSpace objectSpace=null) => 
	        (CompositeView) (objectSpace==null?(CompositeView) application.CallMethod("CreateView", modelView):application.CreateView(modelView, objectSpace));

        static View CreateView(this XafApplication application,IModelView viewModel,IObjectSpace objectSpace) {
	        View view = null;
	        switch (viewModel){
		        case IModelListView listViewModel:{
			        var collectionSource = application.CreateCollectionSource(objectSpace, listViewModel.ModelClass.TypeInfo.Type, listViewModel.Id);
			        view = application.CreateListView(listViewModel, collectionSource, true);
			        break;
		        }
		        case IModelDetailView detailViewModel:
			        view = application.CreateDetailView(objectSpace, detailViewModel, true);
			        break;
		        case IModelDashboardView _:
			        view = application.CreateDashboardView(objectSpace, viewModel.Id, true);
			        break;
	        }
	        return view;
        }



        public static TObjectView NewObjectView<TObjectView>(this XafApplication application,System.Type objectType) where TObjectView:ObjectView =>
            (TObjectView) application.NewObjectView(typeof(TObjectView), objectType);
    }
}