﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using Xpand.XAF.Modules.Reactive.Extensions;

namespace Xpand.XAF.Modules.Reactive.Services{
    static class WindowTemplateService{
        internal static IObservable<Unit> UpdateStatus<T>(TimeSpan period,IObservable<T> messages){
            return UpdateStatus(Observable.Interval(period).ToUnit(), messages);
        }

        private static IObservable<Unit> UpdateStatus<T>(IObservable<Unit> refreshSignal, IObservable<T> messages){
            throw new NotImplementedException();
//            return RxApp.MainWindow
//                .Select(_ => {
//                    var templateController = _.GetController<WindowTemplateController>();
//                    return templateController.UpdateStatusMessage(refreshSignal)
//                        .Zip(templateController.CustomizeStatusMessages(messages), (controller, tuple) => tuple);
//                }).Merge();
        }

//        private static IObservable<Unit> CustomizeStatusMessages<T>(this WindowTemplateController templateController,IObservable<T> other){
//            return templateController.WhenCustomizeWindowStatusMessages()
//                .WithLatestFrom(other, (pattern, tuple) => (pattern, tuple))
//                .SelectMany(tuple => GetMessages(tuple)
//                    .Select(o => {
//                        tuple.pattern.EventArgs.StatusMessages.Add($"{o}");
//                        return o;
//                    }))
//                .ToUnit();
//        }

//        private static IEnumerable<object> GetMessages<T>((EventPattern<CustomizeWindowStatusMessagesEventArgs> pattern, T tuple) tuple){
//            var type = tuple.tuple.GetType();
//            return type.Name.StartsWith(nameof(ValueTuple)) ? type.Fields().Select(info => info.GetValue(tuple.tuple)) : type.Properties().Select(info => info.GetValue(tuple.tuple));
//        }


//        private static IObservable<WindowTemplateController> UpdateStatusMessage(
//            this WindowTemplateController templateController, IObservable<Unit> refreshSignal){
//            return refreshSignal
//                .Select(l => {
//                    templateController.UpdateWindowStatusMessage();
//                    return templateController;
//                })
//                .Publish().RefCount();
//        }
    }
}