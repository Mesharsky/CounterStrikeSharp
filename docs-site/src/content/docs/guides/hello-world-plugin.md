---
title: Hello world plugin
description: Build, install, and run your first CounterStrikeSharp plugin.
---


This guide walks through building a minimal plugin from scratch.

## Create the project

Install the .NET 10 SDK from the
[Microsoft download page](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
and then create a class library.

```shell
dotnet new classlib --name HelloWorldPlugin
```

Add a reference to `CounterStrikeSharp.API`. The easiest way is to install the
NuGet package.

```shell
dotnet add package CounterStrikeSharp.API
```

If you prefer a direct DLL reference, point at the one shipped with the server.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include="CounterStrikeSharp.API">
      <HintPath>[server]/addons/counterstrikesharp/api/CounterStrikeSharp.API.dll</HintPath>
    </Reference>
  </ItemGroup>
</Project>
```

:::tip
.NET 8 plugins still work on the current framework. If you have an older
project, you do not have to retarget right away. Retargeting to .NET 10 gives
better performance but is optional. See [upgrading](/guides/upgrading/) for
details.
:::

## Write the plugin

Rename the default `Class1.cs` to `HelloWorldPlugin.cs` and replace its
contents with the snippet below.

<!-- snippet: hello-world-plugin -->
<a id='snippet-hello-world-plugin'></a>
```cs
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace HelloWorld;

[MinimumApiVersion(80)]
public class HelloWorldPlugin : BasePlugin
{
    public override string ModuleName => "Example: Hello World";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "CounterStrikeSharp & Contributors";
    public override string ModuleDescription => "A simple plugin that says hello world!";

    public override void Load(bool hotReload)
    {
        Logger.LogInformation("Hello World! We are loading!");
    }

    public override void Unload(bool hotReload)
    {
        Logger.LogInformation("Hello World! We are unloading!");
    }
}
```
<sup><a href='/examples/HelloWorld/HelloWorldPlugin.cs#L1-L26' title='Snippet source file'>snippet source</a> | <a href='#snippet-hello-world-plugin' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Build the project.

```shell
dotnet build
```

The output binary lives in `bin/Debug/net10.0/HelloWorldPlugin.dll`.

## Install the plugin

In your dedicated server, open the plugins folder at
`game/csgo/addons/counterstrikesharp/plugins`. Create a folder with the same
name as the DLL (`HelloWorldPlugin`). Copy `HelloWorldPlugin.deps.json`,
`HelloWorldPlugin.dll`, and `HelloWorldPlugin.pdb` into it.

```text
.
└── HelloWorldPlugin
    ├── HelloWorldPlugin.deps.json
    ├── HelloWorldPlugin.dll
    └── HelloWorldPlugin.pdb
```

:::caution
If your plugin pulls in extra NuGet dependencies, copy their DLLs alongside
your plugin DLL too.
:::

## Start the server

Start the dedicated server. Just before the
`CounterStrikeSharp.API Loaded Successfully.` message you should see the
`Hello World!` line logged from the `Load` method.

:::note
CounterStrikeSharp hot reloads plugins when you replace the DLL. On a reload
it calls `Unload` then `Load` with the `hotReload` flag set to `true`. The
framework deregisters event handlers and listeners for you, so re-registering
on every load is safe.
:::
