﻿using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using Shouldly;
using TestsLib;
using Xpand.Source.Extensions.XAF.XafApplication;
using Xpand.XAF.Modules.Reactive;
using Xpand.XAF.Modules.Reactive.Extensions;
using Xpand.XAF.Modules.Reactive.Services;
using Xpand.XAF.Modules.RefreshView.Tests.BOModel;
using Xunit;

namespace Xpand.XAF.Modules.RefreshView.Tests{
    [Collection(nameof(RefreshView.RefreshViewModule))]
    public class RefreshViewTests : BaseTest{
        [Fact]
        public async Task Refresh_ListView_When_Root(){
            
            var application = RefreshViewModule(nameof(Refresh_ListView_When_Root)).Application;
            var items = application.Model.ToReactiveModule<IModelReactiveModuleRefreshView>().RefreshView.Items;
            
            var item = items.AddNode<IModelRefreshViewItem>();
            item.View = application.Model.BOModel.GetClass(typeof(RV)).DefaultListView;
            item.Interval = TimeSpan.FromMilliseconds(500);
            
            application.Logon();
            var listView = application.CreateObjectView<ListView>(typeof(RV));
            var reloaded = listView.CollectionSource.WhenCollectionReloaded().Select(tuple => tuple).FirstAsync().SubscribeReplay();
            application.CreateViewWindow().SetView(listView);
            
            var objectSpace = application.CreateObjectSpace();
            objectSpace.CommitChanges();

            await reloaded;
            application.CreateViewWindow().SetView(listView);
            
            objectSpace = application.CreateObjectSpace();
            var guid = objectSpace.CreateObject<RV>().Oid;
            objectSpace.CommitChanges();
            if (Debugger.IsAttached){
                await Task.Delay(3000).ConfigureAwait(false);
            }
            
            var o = ((IEnumerable) listView.CollectionSource.Collection).Cast<RV>().FirstOrDefault(rv => rv.Oid==guid);
            o.ShouldNotBeNull();
        }



        private static RefreshViewModule RefreshViewModule(string title,
            Platform platform = Platform.Win){
            var application = platform.NewApplication();
            return application.AddModule<RefreshViewModule>(title,typeof(RV));
        }
    }
}