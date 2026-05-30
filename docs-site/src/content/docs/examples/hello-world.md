---
title: Hello world
description: A minimal plugin that logs on load and unload.
sidebar:
  order: 1
---

The smallest possible plugin. It logs a message on load and unload. Use it as
the starting point for a new project.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/HelloWorld)

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
