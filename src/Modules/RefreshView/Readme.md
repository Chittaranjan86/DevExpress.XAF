![](https://img.shields.io/nuget/v/Xpand.XAF.Modules.RefreshView.svg?&style=flat) ![](https://img.shields.io/nuget/dt/Xpand.XAF.Modules.RefreshView.svg?&style=flat)

[![GitHub issues](https://img.shields.io/github/issues/eXpandFramework/expand/RefreshView.svg)](https://github.com/eXpandFramework/eXpand/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc+label%3AStandalone_xaf_modules+RefreshView) [![GitHub close issues](https://img.shields.io/github/issues-closed/eXpandFramework/eXpand/RefreshView.svg)](https://github.com/eXpandFramework/eXpand/issues?utf8=%E2%9C%93&q=is%3Aissue+is%3Aclosed+sort%3Aupdated-desc+label%3AStandalone_XAF_Modules+RefreshView)
# About 

The `RefreshView` module will refresh the View datasource periotically from a model configuration. Model in the details section
.
## Installation 
1. First you need the nuget package so issue this command to the `VS Nuget package console` 

   `Install-Package Xpand.XAF.Modules.RefreshView`.

    The above only references the dependencies and nexts steps are mandatory.

2. [Ways to Register a Module](https://documentation.devexpress.com/eXpressAppFramework/118047/Concepts/Application-Solution-Components/Ways-to-Register-a-Module)
or simply add the next call to your module constructor
    ```cs
    RequiredModuleTypes.Add(typeof(Xpand.XAF.Modules.RefreshViewModule));
    ```

The module is not integrated with any `eXpandFramework` module. You have to install it as described.

## Versioning
The module is **not bound** to **DevExpress versioning**, which means you can use the latest version with your old DevExpress projects [Read more](https://github.com/eXpandFramework/XAF/tree/master/tools/Xpand.VersionConverter).

The module follows the Nuget [Version Basics](https://docs.microsoft.com/en-us/nuget/reference/package-versioning#version-basics).
## Dependencies
`.NetFramework: net461`

|<!-- -->|<!-- -->
|----|----
|**DevExpress.ExpressApp**|**Any**
|Fasterflect.Xpand|2.0.7
 |JetBrains.Annotations|2019.1.3
 |System.Reactive|4.2.0
 |Xpand.Extensions.Reactive|2.201.5
 |[Xpand.XAF.Modules.Reactive](https://github.com/eXpandFramework/DevExpress.XAF/tree/master/src/Modules/Xpand.XAF.Modules.Reactive)|2.201.5
 |[Xpand.VersionConverter](https://github.com/eXpandFramework/DevExpress.XAF/tree/master/tools/Xpand.VersionConverter)|2.201.1

## Issues-Debugging-Troubleshooting

To `Step in the source code` you need to `enable Source Server support` in your Visual Studio/Tools/Options/Debugging/Enable Source Server Support. See also [How to boost your DevExpress Debugging Experience](https://github.com/eXpandFramework/DevExpress.XAF/wiki/How-to-boost-your-DevExpress-Debugging-Experience#1-index-the-symbols-to-your-custom-devexpresss-installation-location).

If the package is installed in a way that you do not have access to uninstall it, then you can `unload` it with the next call at the contructor of your module.
```cs
Xpand.XAF.Modules.Reactive.ReactiveModuleBase.Unload(typeof(Xpand.XAF.Modules.RefreshView.RefreshViewModule))
```

## Details
Use the next model configuration to periodically refresh a ListView.

![image](https://user-images.githubusercontent.com/159464/64825964-d5948780-d5c7-11e9-9249-27a7847e6bb9.png)

### Tests
The module is tested on Azure for each build with these [tests](https://github.com/eXpandFramework/Packages/tree/master/src/Tests/Xpand.XAF.s.RefreshView.RefreshView). 
All Tests run as per our [Compatibility Matrix](https://github.com/eXpandFramework/DevExpress.XAF#compatibility-matrix)
### Examples

