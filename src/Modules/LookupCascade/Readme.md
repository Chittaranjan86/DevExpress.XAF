![](https://xpandshields.azurewebsites.net/nuget/v/Xpand.XAF.Modules.LookupCascade.svg?&style=flat) ![](https://xpandshields.azurewebsites.net/nuget/dt/Xpand.XAF.Modules.LookupCascade.svg?&style=flat)

[![GitHub issues](https://xpandshields.azurewebsites.net/github/issues/eXpandFramework/expand/LookupCascade.svg)](https://github.com/eXpandFramework/eXpand/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc+label%3AStandalone_xaf_modules+LookupCascade) [![GitHub close issues](https://xpandshields.azurewebsites.net/github/issues-closed/eXpandFramework/eXpand/LookupCascade.svg)](https://github.com/eXpandFramework/eXpand/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aclosed+sort%3Aupdated-desc+label%3AStandalone_XAF_Modules+LookupCascade)

# About

The `LookupCascade` module is designed to cascade/synchronize declaratively a PropertyEditor pair directly on the client without callbacks thus optimizing performance.

## Installation

1. First you need the nuget package so issue this command to the `VS Nuget package console`

   `Install-Package Xpand.XAF.Modules.LookupCascade`.

    The above only references the dependencies and next steps are mandatory.

2. [Ways to Register a Module](https://documentation.devexpress.com/eXpressAppFramework/118047/Concepts/Application-Solution-Components/Ways-to-Register-a-Module)
or simply add the next call to your module constructor

    ```cs
    RequiredModuleTypes.Add(typeof(Xpand.XAF.Modules.LookupCascadeModule));
    ```

## Versioning

The module is **not bound** to **DevExpress versioning**, which means you can use the latest version with your old DevExpress projects [Read more](https://github.com/eXpandFramework/XAF/tree/master/tools/Xpand.VersionConverter).

The module follows the Nuget [Version Basics](https://docs.microsoft.com/en-us/nuget/reference/package-versioning#version-basics).

## Dependencies

`.NetFramework: net461`

|<!-- -->|<!-- -->
|----|----
|**DevExpress.ExpressApp**|**Any**
 |**DevExpress.ExpressApp.Web**|**Any**
|Fasterflect.Xpand|2.0.7
 |JetBrains.Annotations|2019.1.3
 |Newtonsoft.Json|12.0.3
 |System.Reactive|4.3.2
 |Xpand.Extensions|2.201.13.2
 |Xpand.Extensions.Reactive|2.201.13.3
 |Xpand.Extensions.XAF|2.201.13.4
 |Xpand.Patcher|1.0.1
 |[Xpand.XAF.Modules.Reactive](https://github.com/eXpandFramework/DevExpress.XAF/tree/master/src/Modules/Xpand.XAF.Modules.Reactive)|2.201.13.4
 |[Xpand.VersionConverter](https://github.com/eXpandFramework/DevExpress.XAF/tree/master/tools/Xpand.VersionConverter)|2.201.5

## Issues-Debugging-Troubleshooting

To `Step in the source code` you need to `enable Source Server support` in your Visual Studio/Tools/Options/Debugging/Enable Source Server Support. See also [How to boost your DevExpress Debugging Experience](https://github.com/eXpandFramework/DevExpress.XAF/wiki/How-to-boost-your-DevExpress-Debugging-Experience#1-index-the-symbols-to-your-custom-devexpresss-installation-location).

If the package is installed in a way that you do not have access to uninstall it, then you can `unload` it with the next call at the constructor of your module.

```cs
Xpand.XAF.Modules.Reactive.ReactiveModuleBase.Unload(typeof(Xpand.XAF.Modules.LookupCascade.LookupCascadeModule))
```

## Details

**Credits:** to the Company that [sponsor](https://github.com/sponsors/apobekiaris) the initial implementation of this module.

Assuming you have a domain like:
![image](https://user-images.githubusercontent.com/159464/79579820-c3844580-80d0-11ea-9ca5-2b7d1fe16b59.png)

To cascade the Product, Accessory pair you need to follow the next steps in order.

1. Assign the `ASPxLookupCascadePropertyEditor` to both the Product and Order to an Order DetailView or ListView.
2. The module extends the `IModelPropertyEditor` with a `LookupCascade` property (visible only if the member uses the `ASPxLookupCascadePropertyEditor`). Configure the Product `LookupCascade.CascadeMemberViewItem` model attribute to point to the Accessory MemberViewItem.
3. Configure the `CascadeColumnFilter` to point to the `visible` Product key column found in the Accessory_LookupListView. To hide this key column set the column `ClientVisible` in the Accessory_LookupListView.
4. Optionally set the `Synchronize` attribute to true, so when you chose an Accessory with a blank Product it will fill the matching Product for you.
5. Configure the LookupViews client datasources using the `Application/ReactiveModules/LookupCascade/ClientDatasource/LookupViews` model node. The module will serialize the participating lookupviews and send them to the client once where the client scripts will then use to cascade and synchronize the editors.

Below is the model describing the above configuration used from the related EayTest on each build.

```xml
<Application>
  <ReactiveModules>
    <LookupCascade>
      <ClientDatasource>
        <LookupViews>
          <ClientDatasourceLookupView Id="Accesories" LookupListView="LookupCascade_Accessory_LookupListView" IsNewNode="True" />
          <ClientDatasourceLookupView Id="Products" LookupListView="Product_LookupListView" IsNewNode="True" />
        </LookupViews>
      </ClientDatasource>
    </LookupCascade>
  </ReactiveModules>
  <Views>
    <ListView Id="LookupCascade_Accessory_LookupListView">
      <Columns>
        <ColumnInfo Id="Product" PropertyName="Product.Oid" Index="2" Caption="Product" ClientVisible="False" IsNewNode="True" Removed="True" />
      </Columns>
    </ListView>
    <DetailView Id="LookupCascade_Order_DetailView">
      <Items>
        <PropertyEditor Id="Accessory" PropertyEditorType="Xpand.XAF.Modules.LookupCascade.ASPxLookupCascadePropertyEditor" View="LookupCascade_Accessory_LookupListView" />
        <PropertyEditor Id="AggregatedOrders" View="LookupCascade_Order_ListView" />
        <PropertyEditor Id="Product" PropertyEditorType="Xpand.XAF.Modules.LookupCascade.ASPxLookupCascadePropertyEditor">
          <LookupCascade CascadeMemberViewItem="Accessory" CascadeColumnFilter="Product" />
        </PropertyEditor>
      </Items>
    </DetailView>
    <ListView Id="LookupCascade_Order_ListView" AllowEdit="True" NewItemRowPosition="Top" DetailViewID="LookupCascade_Order_DetailView">
      <Columns>
        <ColumnInfo Id="Product" PropertyEditorType="Xpand.XAF.Modules.LookupCascade.ASPxLookupCascadePropertyEditor" SortIndex="0" SortOrder="Ascending">
          <LookupCascade Synchronize="True" CascadeMemberViewItem="Accessory" CascadeColumnFilter="Product" />
        </ColumnInfo>
        <ColumnInfo Id="Accessory" PropertyEditorType="Xpand.XAF.Modules.LookupCascade.ASPxLookupCascadePropertyEditor" View="LookupCascade_Accessory_LookupListView">
          <LookupCascade SynchronizeMemberViewItem="Product" SynchronizeMemberLookupColumn="ProductName" />
        </ColumnInfo>
      </Columns>
    </ListView>
  </Views>
</Application>
```

Limitations: The module stores the datasources to the [sessionStorage](https://developer.mozilla.org/en-US/docs/Web/API/Window/sessionStorage) which has a limit of 5mb. However they are compressed so you can store a really large number of objects. The sessionStorage is not persistent and dies when the browser is closed.

Possible future improvements:

1. Live datasource synchronization instead of restarting the browser.
2. Earlier datasource transmission e.g. on XafApplication.SetupCompleted event.
3. Support for creating new objects.

We estimate the above improvements to 15-20 hours. Let us know if you want us to implement them for you, or if you have other ideas and needs.
### Tests

The module is tested on Azure for each build with these [tests](https://github.com/eXpandFramework/Packages/tree/master/src/Tests/Xpand.XAF.Modules.LookupCascade).
All Tests run as per our [Compatibility Matrix](https://github.com/eXpandFramework/DevExpress.XAF#compatibility-matrix)

### Examples