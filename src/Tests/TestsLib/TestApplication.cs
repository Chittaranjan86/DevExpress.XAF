﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Forms;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Win;
using Xpand.Extensions.AppDomain;
using Xpand.XAF.Modules.Reactive.Extensions;
using Xpand.XAF.Modules.Reactive.Logger.Hub;

namespace Xpand.TestsLib{
    public class TestWinApplication : WinApplication,ITestApplication{
        private readonly bool _transmitMessage;

        public TestWinApplication(Type sutModule,bool transmitMessage=true){
            _transmitMessage = transmitMessage;
            SUTModule = sutModule;
            TraceClientConnected = this.ClientConnect();
            TraceClientBroadcast = this.ClientBroadcast();
        }

        public IObservable<Unit> TraceClientBroadcast{ get; set; }


        public IObservable<Unit> TraceClientConnected{ get; set; }
        public Type SUTModule{ get; set; }

        protected override void Dispose(bool disposing){
            if (_transmitMessage){
                TraceClientConnected.Wait();
                TraceClientBroadcast.Wait();
            }
            base.Dispose(disposing);
        }

        readonly Subject<Form> _modelEditorForm=new Subject<Form>();

        public IObservable<Form> ModelEditorForm => Observable.AsObservable(_modelEditorForm);

        protected override Form CreateModelEditorForm(){
            var modelEditorForm = base.CreateModelEditorForm();
            _modelEditorForm.OnNext(modelEditorForm);
            return modelEditorForm;
        }

//        protected override LayoutManager CreateLayoutManagerCore(bool simple){
//            if (!simple){
//                var controlMock = new Mock<Control>(){CallBase = true};
//                var layoutManagerMock = new Mock<WinLayoutManager>(){CallBase = true};
//                layoutManagerMock.Setup(_ => _.LayoutControls(It.IsAny<IModelNode>(), It.IsAny<ViewItemsCollection>())).Returns(controlMock.Object);
//            
//                return layoutManagerMock.Object;
//            }
//
//            return new WinSimpleLayoutManager();
//        }

        protected override string GetModelCacheFileLocationPath(){
            return null;
        }

        protected override string GetDcAssemblyFilePath(){
            return null;
        }

        public override void StartSplash(){
            
        }

        protected override string GetModelAssemblyFilePath(){

            return $@"{AppDomain.CurrentDomain.ApplicationPath()}\ModelAssembly{Guid.NewGuid()}.dll";
        }
    }
    public class TestWebApplication : WebApplication,ITestApplication{
        private readonly bool _transmitMessage;

        public TestWebApplication(Type sutModule,bool transmitMessage=true){
            _transmitMessage = transmitMessage;
            SUTModule = sutModule;
            TraceClientConnected = this.ClientConnect();
            TraceClientBroadcast = this.ClientBroadcast();
        }

        public IObservable<Unit> TraceClientBroadcast{ get; set; }

        protected override bool CanLoadTypesInfo(){
            return true;
        }
        protected override void Dispose(bool disposing){
            if (_transmitMessage){
                var timeout = TimeSpan.FromMilliseconds(5000);
                TraceClientConnected.Timeout(timeout).Wait();
                TraceClientBroadcast.Timeout(timeout).Wait();
            }
            base.Dispose(disposing);
        }

        protected override bool IsSharedModel => false;
        public IObservable<Unit> TraceClientConnected{ get; set; }
        public Type SUTModule{ get; set; }
    }

    public interface ITestApplication{
        IObservable<Unit> TraceClientBroadcast{ get; set; }
        IObservable<Unit> TraceClientConnected{ get; set; }
        Type SUTModule{ get;  }
    }

    static class TestApplicationExtensions{
        public static IObservable<Unit> ClientBroadcast(this ITestApplication application){
            return Process.GetProcessesByName("Xpand.XAF.Modules.Reactive.Logger.Client.Win").Any()
                ? TraceEventHub.Broadcasted.FirstAsync(_ => _.Source==application.SUTModule.Name).ToUnit().SubscribeReplay()
                : Unit.Default.AsObservable();
        }

        public static IObservable<Unit> ClientConnect(this ITestApplication application){
            return Process.GetProcessesByName("Xpand.XAF.Modules.Reactive.Logger.Client.Win").Any()
                ? TraceEventHub.Connecting.FirstAsync().SubscribeReplay()
                : Unit.Default.AsObservable();
        }


    }
}