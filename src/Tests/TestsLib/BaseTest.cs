﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using Xunit.Abstractions;
using IDisposable = System.IDisposable;

namespace TestsLib{
    public abstract class BaseTest : IDisposable{
        public const string NotImplemented = "NotImplemented";
        protected BaseTest(ITestOutputHelper output){
            Output = output;
        }

        protected BaseTest(){
        }

        public ITestOutputHelper Output{ get; }

        

        public virtual void Dispose(){
            XpoTypesInfoHelper.Reset();
            XafTypesInfo.HardReset();
        }
    }
}