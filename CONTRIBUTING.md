# Contributing

## Local Development

This project is aligned with LiveSplit and as such uses .NET Framework. It is therefore recommended to work on Windows with [Visual Studio](https://visualstudio.microsoft.com/). The community version is free for individual use.

During installation you should have the option to add support for .NET Framework 4.8. If not, you can download the developer pack from [Microsoft directly](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48).

### Testing the components

To test the components build them in Debug Mode and add them to your LiveSplit's "Components" directory. You can also configure LiveSplit to be the default executable on Launch, in case you want to attach the Visual Studio debugger, for using breakpoints, watches and such.

## Building / Releasing a new version

Once your changes are ready, make sure you follow the steps below if you made changes to the respective project.

On Startup, a user's LiveSplit generally loads the update files `Components/update.LiveSplit.FFX.xml` and `LiveSplit.FFX.UI/Components/update.LiveSplit.FFX.UI.xml`, cross check them with the local assembly version and ask the user to update if there is a newer version available. The URL from where it pulls the XML files is defined in the [LiveSplit.AutoSplitters repository](https://github.com/LiveSplit/LiveSplit.AutoSplitters/blob/f9431416c83c900b53107d3056536a8af9c0a530/LiveSplit.AutoSplitters.xml#L13450-L13461). The configuration points to the master branch of this (BitPatty/LiveSplit.FFX) repository.

> [!CAUTION]
> LiveSplit downloads the update XML and the components (DLLs) from the `master` branch.
> Make sure you double check any modifications before pushing them to `master`.

### LiveSplit.FFX

1. Add one to the minor version in `Properties/AssemblyInfo.cs`. Update both the `AssemblyVersion` and the `AssemblyFileVersion` field. E.g. From `1.10.0.0` to `1.11.0.0`.
2. Add a new `update` section to the top of the the changelog in `Components/update.LiveSplit.FFX.xml`, with the version you put into `Properties/AssemblyInfo.cs` as well as a short description on what has changed.
3. Change your build mode to "Release" and rebuild the project
4. Copy the resulting `LiveSplit.FFX.dll` into the `Components/` directory.

### LiveSplit.FFX.UI

1. Add one to the minor version in `LiveSplit.FFX.UI/Properties/AssemblyInfo.cs`. Update both the `AssemblyVersion` and the `AssemblyFileVersion` field. E.g. From `1.10.0.0` to `1.11.0.0`.
2. Add a new `update` section to the top of the the changelog in `LiveSplit.FFX.UI/Components/update.LiveSplit.FFX.UI.xml`, with the version you put into `LiveSplit.FFX.UI/Properties/AssemblyInfo.cs` as well as a short description on what has changed.
3. Change your build mode to "Release" and rebuild the project
4. Copy the resulting `LiveSplit.FFX.UI.dll` into the `LiveSplit.FFX.UI/Components/` directory.
