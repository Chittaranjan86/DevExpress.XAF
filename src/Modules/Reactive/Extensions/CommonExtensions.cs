﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using Fasterflect;
using Xpand.Extensions.Configuration;
using Xpand.Extensions.Exception;
using Xpand.Extensions.Reactive.Transform;
using Xpand.Extensions.XAF.XafApplication;
using Xpand.XAF.Modules.Reactive.Services.Controllers;

namespace Xpand.XAF.Modules.Reactive.Extensions{
    public static class CommonExtensions{
        private static readonly object ErrorHandling;
        private static readonly Type WebWindowType;

        static CommonExtensions(){
            DXWebAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.GetName().Name.StartsWith("DevExpress.ExpressApp.Web.v"));
            var errorHandlingType = DXWebAssembly?.GetTypes().First(type => type.FullName=="DevExpress.ExpressApp.Web.ErrorHandling");
            ErrorHandling = errorHandlingType?.GetProperty("Instance")?.GetValue(null);
            
            WebWindowType = DXWebAssembly?.GetTypes().First(type => type.FullName=="DevExpress.ExpressApp.Web.WebWindow");
            
        }

        public static object CurrentRequestPage => WebWindowType?.GetProperty("CurrentRequestPage")?.GetValue(null);

        public static Assembly DXWebAssembly{ get; }

        public static IDisposable Subscribe<T>(this IObservable<T> source, ModuleBase moduleBase){
            var takeUntil = source.TakeUntil(moduleBase.WhenDisposed());
            return moduleBase.Application!=null ? takeUntil.Subscribe(moduleBase.Application) : takeUntil.Subscribe();
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, Controller controller){
            return source.TakeUntil(controller.WhenDeactivated()).Subscribe(controller.Application);
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source,XafApplication application){
            return source.HandleErrors(application)
                .Subscribe();
        }

        public static IObservable<T> Retry<T>(this IObservable<T> source, XafApplication application){
            return source.RetryWhen(_ => _.Do(application.HandleException)
                .SelectMany(e => application.GetPlatform()==Platform.Win?e.ReturnObservable():Observable.Empty<Exception>()));
        }
        
        public static IObservable<T> HandleErrors<T>(this IObservable<T> source, XafApplication application, CancelEventArgs args=null,Func<Exception, IObservable<T>> exceptionSelector=null){
            exceptionSelector ??= (exception => Observable.Empty<T>());
            return source.Catch<T, Exception>(exception => {
                if (args != null) args.Cancel = true;
                application.HandleException( exception);
                return exception.Handle(exceptionSelector);
            });
        }

        private static void HandleException(this XafApplication application, Exception exception){
            if (application.GetPlatform() == Platform.Win){
                application.CallMethod("HandleException", exception);
            }
            else{
                ErrorHandling.CallMethod("SetPageError", exception);
            }
            Tracing.Tracer.LogError(exception);
        }

        public static IObservable<T> Handle<T>(this Exception exception, Func<Exception, IObservable<T>> exceptionSelector = null) => exception is WarningException ? default(T).ReturnObservable() :
            exceptionSelector != null ? exceptionSelector(exception) : Observable.Throw<T>(exception);

        public static IObservable<T> HandleException<T>(this IObservable<T> source,Func<Exception,IObservable<T>> exceptionSelector=null){
            
            return source.Catch<T, Exception>(exception => {
                if (Tracing.IsTracerInitialized) Tracing.Tracer.LogError(exception);
                var result=Observable.Empty<T>();
                if (ConfigurationManager.AppSettings["ExceptionMailer"]!=null){
                    result = Observable.Using(() => ConfigurationManager.AppSettings.NewSmtpClient(), smtpClient => {
                        var errorMail = exception.ToMailMessage(((NetworkCredential) smtpClient.Credentials).UserName);
                        return smtpClient.SendMailAsync(errorMail).ToObservable().To(default(T));
                    });
                }
                return result.SelectMany(unit => exception is WarningException ? default(T).ReturnObservable() :
                    exceptionSelector != null ? exceptionSelector(exception) : Observable.Throw<T>(exception));
            });
        }

        public static IObservable<(BindingListBase<T> list, ListChangedEventArgs e)> WhenListChanged<T>(this BindingListBase<T> listBase){
            return Observable.FromEventPattern<ListChangedEventHandler, ListChangedEventArgs>(
                    h => listBase.ListChanged += h, h => listBase.ListChanged -= h, ImmediateScheduler.Instance)
                .Select(_ => (list: (BindingListBase<T>) _.Sender, e: _.EventArgs));
        }

        internal static bool Fits(this View view, ViewType viewType = ViewType.Any, Nesting nesting = Nesting.Any,
            Type objectType = null){
            objectType ??= typeof(object);
            return FitsCore(view, viewType) && FitsCore(view, nesting) && objectType.IsAssignableFrom(view.ObjectTypeInfo?.Type);
        }

        private static bool FitsCore(View view, ViewType viewType){
            if (view == null)
                return false;
            if (viewType == ViewType.ListView)
                return view is ListView;
            if (viewType == ViewType.DetailView)
                return view is DetailView;
            if (viewType == ViewType.DashboardView)
                return view is DashboardView;
            return true;
        }

        private static bool FitsCore(View view, Nesting nesting){
            return nesting == Nesting.Nested ? !view.IsRoot : nesting != Nesting.Root || view.IsRoot;
        }
    }
}