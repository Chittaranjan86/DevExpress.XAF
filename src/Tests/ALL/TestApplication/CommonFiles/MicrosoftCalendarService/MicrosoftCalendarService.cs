﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using Xpand.Extensions.Office.Cloud;
using Xpand.Extensions.Reactive.Transform;
using Xpand.Extensions.XAF.FrameExtensions;
using Xpand.XAF.Modules.Office.Cloud.Microsoft;
using Xpand.XAF.Modules.Office.Cloud.Microsoft.Calendar;
using Xpand.XAF.Modules.Reactive;
using Xpand.XAF.Modules.Reactive.Services;
using Xpand.XAF.Modules.Reactive.Services.Actions;
using CallDirection = Xpand.XAF.Modules.Office.Cloud.Microsoft.Calendar.CallDirection;
using Event = DevExpress.Persistent.BaseImpl.Event;

namespace TestApplication.MicrosoftCalendarService{
    public static class MicrosoftCalendarService{
        public static SimpleAction CloudOperation(this (MicrosoftModule, Frame frame) tuple) 
            => tuple.frame.Action(nameof(CloudOperation)).As<SimpleAction>();
        

        public static IObservable<Unit> ConnectMicrosoftCalendarService(this ApplicationModulesManager manager) 
            => manager.InitializeModule()
                .Merge(manager.WhenApplication(application => application.UpdateCalendarDescription().ToUnit()
                    .Merge(application.DeleteAllEvents())).ToUnit());

        private static IObservable<Unit> DeleteAllEvents(this XafApplication application)
            => CalendarService.Client.FirstAsync().Select(tuple => application.AuthorizeMS().SelectMany(client => client.Me.Calendar.DeleteAllEvents())).Concat().ToUnit();

        private static IObservable<(Microsoft.Graph.Event serviceObject, MapAction mapAction, CallDirection callDirection)> UpdateCalendarDescription(this XafApplication application) 
            => CalendarService.Updated.Where(t => t.callDirection==CallDirection.Out)
                .Do(tuple => {
                    using (var objectSpace = application.CreateObjectSpace()){
                        var cloudOfficeObject = objectSpace.QueryCloudOfficeObject(tuple.serviceObject.Id, CloudObjectType.Event).FirstOrDefault();
                        if (cloudOfficeObject!=null){
                            var @event = objectSpace.GetObjectByKey<Event>(Guid.Parse(cloudOfficeObject.LocalId));
                            @event.Description = tuple.mapAction.ToString();
                            objectSpace.CommitChanges();
                        }
                    }
                });

        private static IObservable<Unit> ExecuteCloudOperation(this IObservable<SingleChoiceAction> source) 
            => source.WhenExecute().SelectMany(e => {
                var authorizeMS = e.Action.Application.AuthorizeMS();
                if (e.SelectedChoiceActionItem.Caption == "New"){
                    return authorizeMS.SelectMany(client => client.Me.Events.Request()
                        .AddAsync(new Microsoft.Graph.Event(){Subject = "Cloud"})).ObserveOn(SynchronizationContext.Current).ToUnit();
                }
                var objectSpace = e.Action.Application.CreateObjectSpace();
                var cloudOfficeObject = objectSpace.QueryCloudOfficeObject(typeof(Microsoft.Graph.Event),objectSpace.GetObjects<Event>().First()).First();
                return e.SelectedChoiceActionItem.Caption == "Update"
                    ? authorizeMS.SelectMany(c => c.Me.Events[cloudOfficeObject.CloudId].Request()
                        .UpdateAsync(new Microsoft.Graph.Event(){Subject = "Cloud-Updated"})).ObserveOn(SynchronizationContext.Current).ToUnit()
                    : authorizeMS.SelectMany(c => c.Me.Events[cloudOfficeObject.CloudId].Request()
                        .DeleteAsync().ToObservable().ObserveOn(SynchronizationContext.Current)).ToUnit();
            });

        private static IObservable<Unit> InitializeModule(this ApplicationModulesManager manager){
            manager.Modules.OfType<AgnosticModule>().First().AdditionalExportedTypes.Add(typeof(Event));
            return manager.WhenCustomizeTypesInfo().Do(_ => _.e.TypesInfo.FindTypeInfo(typeof(Event)).AddAttribute(new DefaultClassOptionsAttribute())).ToUnit()
                .FirstAsync()
                .Concat(manager.WhenGeneratingModelNodes(application => application.Views)
                    .Do(views => {
                        var modelCalendar = views.Application.ToReactiveModule<IModelReactiveModuleOffice>().Office.Microsoft().Calendar();
                        var calendarItem = modelCalendar.Items.AddNode<IModelCalendarItem>();
                        calendarItem.ObjectView=views.Application.BOModel.GetClass(typeof(Event)).DefaultDetailView;
                        calendarItem = modelCalendar.Items.AddNode<IModelCalendarItem>();
                        calendarItem.ObjectView=views.Application.BOModel.GetClass(typeof(Event)).DefaultListView;
                    }).ToUnit())
                .Merge(manager.RegisterViewSingleChoiceAction(nameof(CloudOperation), action => {
                    action.TargetObjectType = typeof(Event);
                    action.ItemType=SingleChoiceActionItemType.ItemIsOperation;
                    action.Items.Add(new ChoiceActionItem("New", "New"));
                    action.Items.Add(new ChoiceActionItem("Update", "Update"));
                    action.Items.Add(new ChoiceActionItem("Delete", "Delete"));
                }).ExecuteCloudOperation().ToUnit());
        }
    }
}