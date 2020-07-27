﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using Microsoft.Graph;
using Xpand.Extensions.Office.Cloud;
using Xpand.Extensions.Reactive.Transform;
using Xpand.XAF.Modules.Office.Cloud.Microsoft;
using Xpand.XAF.Modules.Office.Cloud.Microsoft.Todo;
using Xpand.XAF.Modules.Reactive;
using Xpand.XAF.Modules.Reactive.Services;
using Task = DevExpress.Persistent.BaseImpl.Task;

// ReSharper disable once CheckNamespace
namespace TestApplication.MicrosoftTodoService{
    public static class MicrosoftTodoService{
        public static IObservable<Unit> ConnectMicrosoftTodoService(this ApplicationModulesManager manager) =>
            manager.InitializeModule().Merge(manager.WhenApplication(application => application.UpdateTaskDescription()).ToUnit());

        private static IObservable<(OutlookTask serviceObject, MapAction mapAction)> UpdateTaskDescription(this XafApplication application) =>
            TodoService.Updated
                .Do(tuple => {
                    using (var objectSpace = application.CreateObjectSpace()){
                        var cloudOfficeObject = objectSpace.QueryCloudOfficeObject(tuple.cloud.Id, CloudObjectType.Task).First();

                        var task = objectSpace.GetObjectByKey<Task>(Guid.Parse(cloudOfficeObject.LocalId));
                        task.Description = tuple.mapAction.ToString();
                        objectSpace.CommitChanges();
                    }
                });

        private static IObservable<Unit> InitializeModule(this ApplicationModulesManager manager){
            manager.Modules.OfType<AgnosticModule>().First().AdditionalExportedTypes.Add(typeof(Task));
            return manager.WhenCustomizeTypesInfo().Do(_ => _.e.TypesInfo.FindTypeInfo(typeof(Task)).AddAttribute(new DefaultClassOptionsAttribute())).ToUnit()
                .FirstAsync()
                .Concat(manager.WhenGeneratingModelNodes(application => application.Views)
                    .Do(views => {
                        var modelTodo = views.Application.ToReactiveModule<IModelReactiveModuleOffice>().Office.Microsoft().Todo();
                        var modelTodoItem = modelTodo.Items.AddNode<IModelTodoItem>();
                        modelTodoItem.ObjectView=views.Application.BOModel.GetClass(typeof(Task)).DefaultDetailView;
                    }).ToUnit());
        }
    }
}
