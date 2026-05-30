---
title: Dependency injection
description: How to use the built in service collection in CounterStrikeSharp plugins.
---

CounterStrikeSharp uses a standard
[`IServiceCollection`](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
to wire up plugin services.

Some services are registered for you automatically, such as `ILogger`. To add
your own services, create a class that implements `IPluginServiceCollection<T>`
where `T` is your plugin type.

CounterStrikeSharp scans your assembly for both `IPlugin` implementations and
`IPluginServiceCollection<T>` registrations, builds the service provider, then
resolves your plugin from it. Anything you declare in your plugin constructor
is injected at that point, before `Load` is called.

## Example

<!-- snippet: di-plugin -->
<a id='snippet-di-plugin'></a>
```cs
[MinimumApiVersion(80)]
public class WithDependencyInjectionPlugin : BasePlugin
{
    public override string ModuleName => "Example: Dependency Injection";
    public override string ModuleVersion => "1.0.0";

    private readonly TestInjectedClass _testInjectedClass;

    public WithDependencyInjectionPlugin(TestInjectedClass testInjectedClass)
    {
        _testInjectedClass = testInjectedClass;
    }

    public override void Load(bool hotReload)
    {
        _testInjectedClass.SayHello();
    }
}

public class WithDependencyInjectionPluginServiceCollection : IPluginServiceCollection<WithDependencyInjectionPlugin>
{
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<TestInjectedClass>();
    }
}

public class TestInjectedClass
{
    private readonly ILogger<TestInjectedClass> _logger;

    public TestInjectedClass(ILogger<TestInjectedClass> logger)
    {
        _logger = logger;
    }

    public void SayHello()
    {
        _logger.LogInformation("Hello World from Test Injected Class");
    }
}
```
<sup><a href='/examples/WithDependencyInjection/WithDependencyInjectionPlugin.cs#L8-L50' title='Snippet source file'>snippet source</a> | <a href='#snippet-di-plugin' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## When to use it

Use dependency injection for things that need a lifecycle longer than a single
method call, like a database connection, a configuration cache, or a service
that talks to an external API. Logging a value once does not need DI.
