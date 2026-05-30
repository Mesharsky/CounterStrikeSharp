---
title: Dependency injection
description: Register a service and inject it into your plugin constructor.
sidebar:
  order: 9
---

Implement `IPluginServiceCollection<TPlugin>` to add services to the container.
Services are injected into the plugin constructor before `Load` runs.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithDependencyInjection)

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
