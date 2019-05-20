﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AppDomainToolkit;
using DevExpress.ExpressApp;
using Shouldly;
using Tests.Artifacts;
using Tests.Modules.AutoCommit.BOModel;
using Xpand.Source.Extensions.XAF.XafApplication;
using Xpand.XAF.Modules.AutoCommit;
using Xunit;

namespace Tests.Modules.AutoCommit{
//    [Collection(nameof(XafTypesInfo))]
    public class AutoCommitTests : BaseTest{

        [Theory]
        [InlineData(Platform.Web)]
        [InlineData(Platform.Win)]
        internal async Task Signal_When_AutoCommit_Enabled_ObjectView_Created(Platform platform){
            await RemoteFuncAsync.InvokeAsync(Domain, platform, async _ => {
                var application = DefaultAutoCommitModule(_).Application;
                var objectViews = application.WhenAutoCommitObjectViewCreated().Replay();
                objectViews.Connect();
                var listView = application.CreateObjectView<ListView>(typeof(AC));
                var detailView = application.CreateObjectView<DetailView>(typeof(AC));

                (await objectViews.Take(1).WithTimeOut()).ShouldBe(listView);
                (await objectViews.Take(2).WithTimeOut()).ShouldBe(detailView);
                return Unit.Default;
            });

        }

        

        [Theory]
        [InlineData(Platform.Web)]
        [InlineData(Platform.Win)]
        internal async Task AutoCommit_When_object_view_closing(Platform platform){
            await RemoteFuncAsync.InvokeAsync(Domain, platform, async _ => {
                var application = DefaultAutoCommitModule(_).Application;
                var objectViews = application.WhenAutoCommitObjectViewCreated().Replay();
                objectViews.Connect();
                var detailView = application.CreateObjectView<DetailView>(typeof(AC));
                detailView.ObjectSpace.CreateObject<AC>();

                detailView.Close();
                await objectViews.FirstAsync().WithTimeOut();

                application.CreateObjectSpace().FindObject<AC>(null).ShouldNotBeNull();

                return Unit.Default;
            });
        }

        private static AutoCommitModule DefaultAutoCommitModule(Platform platform){
            var application = platform.NewApplication();
            application.Title = "AutoCommitModule";
            var autoCommitModule = new AutoCommitModule();
            autoCommitModule.AdditionalExportedTypes.AddRange(new[]{typeof(AC)});
            application.SetupDefaults(autoCommitModule);
            
            var modelClassAutoCommit = (IModelClassAutoCommit) application.Model.BOModel.GetClass(typeof(AC));
            modelClassAutoCommit.AutoCommit = true;
            return autoCommitModule;
        }
    }
}