![](https://img.shields.io/nuget/v/Xpand.XAF.Modules.HideToolBar.svg?&style=flat) ![](https://img.shields.io/nuget/dt/Xpand.XAF.Modules.HideToolBar.svg?&style=flat)

[![GitHub issues](https://img.shields.io/github/issues/eXpandFramework/expand/HideToolBar.svg)](https://github.com/eXpandFramework/eXpand/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc+label%3AStandalone_xaf_modules+HideToolBar) [![GitHub close issues](https://img.shields.io/github/issues-closed/eXpandFramework/eXpand/HideToolBar.svg)](https://github.com/eXpandFramework/eXpand/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aclosed+sort%3Aupdated-desc+label%3AStandalone_XAF_Modules+HideToolBar)
# About 

The `HIdeToolBar` package can be used to hide the Nested ListView toolbar. The module extends the model with the `IModelListView.HIdeToolBar` attribute to true. The implemented scenarios are described in the details section.
## Installation 
1. First you need the nuget package so issue this command to the `VS Nuget package console` 

   `Install-Package Xpand.XAF.Modules.HIdeToolBar`.

    The above only references the dependencies and nexts steps are mandatory.

2. [Ways to Register a Module](https://documentation.devexpress.com/eXpressAppFramework/118047/Concepts/Application-Solution-Components/Ways-to-Register-a-Module)
or simply add the next call to your module constructor
    ```cs
    RequiredModuleTypes.Add(typeof(Xpand.XAF.Modules.HIdeToolBarModule));
    ```
## Versioning
The module is **not bound** to **DevExpress versioning**, which means you can use the latest version with your old DevExpress projects [Read more](https://github.com/eXpandFramework/XAF/tree/master/tools/Xpand.VersionConverter).

The module follows the Nuget [Version Basics](https://docs.microsoft.com/en-us/nuget/reference/package-versioning#version-basics).
## Dependencies
`.NetFramework: v4.6.1`

|<!-- -->|<!-- -->
|----|----
|**DevExpress.ExpressApp**|**Any**
|[Xpand.VersionConverter](https://github.com/eXpandFramework/DevExpress.XAF/tree/master/tools/Xpand.VersionConverter)|1.0.22
 |[Xpand.XAF.Modules.Reactive](https://github.com/eXpandFramework/DevExpress.XAF/tree/master/src/Modules/Agnostic/Xpand.XAF.Modules.Reactive)|1.2.25
 |fasterflect|2.1.3
 |System.ValueTuple|4.5.0

## Issues
For [Bugs](https://github.com/eXpandFramework/eXpand/issues/new?assignees=apobekiaris&labels=Bug%2C+Standalone_XAF_Modules,+HIdeToolBar&template=standalone-xaf-modules--bug-report.md&title=), [Questions](https://github.com/eXpandFramework/eXpand/issues/new?assignees=apobekiaris&labels=Question%2C+Standalone_XAF_Modules,+HIdeToolBar&template=standalone-xaf-modules--question.md&title=) or [Suggestions](https://github.com/eXpandFramework/eXpand/issues/new?assignees=apobekiaris&labels=Enhancement%2C+Standalone_XAF_Modules,+HIdeToolBar&template=standalone-xaf-modules--feature-request.md&title=) use main project issues.
## Details
The module satisfies the following conditions:
1. For all `Nested ListView` with `HIdeToolBar` attribute enabled a signal will be created out of the [View.ControlsCreated](https://documentation.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.View.ControlsCreated.event) and the [QueryCanChangeCurrentObject](https://documentation.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.View.QueryCanChangeCurrentObject.event) events. For each signal emit the `View.ObjectSpace.CommitChanges()` is called.
2. When the `ASPxListEditor` of any `ListView` with `AllowEdit` in `BatchEdit` mode and `HIdeToolBar` loses focus (`Client side`) then `View.ObjectSpace.CommitChanges()` is called.

![image](https://user-images.githubusercontent.com/159464/56097334-50fbeb00-5efb-11e9-921b-08f6c2d5b607.png)

### Tests
The module is tested on Azure for each build with these [tests](https://github.com/eXpandFramework/Packages/tree/master/src/Tests/Modules/HIdeToolBar)

### Examples
The module is integrated with the `ExcelImporter`.

Next screenshot is an example from ExcelImporter from the view tha maps the Excel columns with the BO members. Both the DetailView and its nested ListView have HIdeToolBar set to true.

![image](https://user-images.githubusercontent.com/159464/55381194-238e6500-552b-11e9-8314-f1b1132d09f3.png)
