﻿using System;
using System.Diagnostics;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using NUnit.Framework;
using Xpand.Extensions.AppDomain;
using Xpand.Extensions.XAF.XafApplication;
using IDisposable = System.IDisposable;

namespace Xpand.TestsLib{
    public abstract class BaseTest : IDisposable{
        protected Platform GetPlatform(string platformName){
            return (Platform)Enum.Parse(typeof(Platform),platformName);
        }
        protected TimeSpan Timeout = TimeSpan.FromSeconds(Debugger.IsAttached?120:5);

        static BaseTest(){
            TextListener = new TextWriterTraceListener($@"{AppDomain.CurrentDomain.ApplicationPath()}\reactive.log");
            var traceSourceSwitch = new SourceSwitch("SourceSwitch", "Verbose");
            TraceSource = new TraceSource(nameof(BaseTest)){Switch = traceSourceSwitch};
            TraceSource.Listeners.Add(TextListener);
        }

        protected void WriteLine(bool value){
            TestContext.WriteLine(value);    
        }
        protected void WriteLine(char value){
            TestContext.WriteLine(value);    
        }
        protected void WriteLine(char[] value){
            TestContext.WriteLine(value);    
        }
        protected void WriteLine(decimal value){
            TestContext.WriteLine(value);    
        }

        public static TextWriterTraceListener TextListener{ get; }

        public static TraceSource TraceSource{ get; }

        public const string NotImplemented = "NotImplemented";
        
        [TearDown]
        public void Dispose(){
            XpoTypesInfoHelper.Reset();
            XafTypesInfo.HardReset();
//            GC.Collect();
        }
    }
}