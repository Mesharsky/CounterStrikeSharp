---
title: Shared plugin API
description: Expose and consume APIs between CounterStrikeSharp plugins.
---


A plugin can expose typed APIs to other plugins through **capabilities**. A
capability is identified by a string name. One plugin registers an
implementation, any other plugin asks for the same name and gets the
implementation back.

## Contract library

The shared API lives in a separate class library that exposes only interfaces.
That library has no implementation and no business logic.

```csharp
public interface IBalanceHandler
{
    decimal Balance { get; }
    decimal Add(decimal amount);
    decimal Subtract(decimal amount);
}
```

The compiled DLL goes into `addons/counterstrikesharp/shared/<name>/<name>.dll`,
for example `shared/MySharedApi/MySharedApi.dll`.

## Declare capabilities

`PlayerCapability<T>` is scoped to a player. `PluginCapability<T>` is global.

<!-- snippet: capabilities-declare -->
<a id='snippet-capabilities-declare'></a>
```cs
// A player capability is keyed per player. Other plugins use the same name to reach it.
// IBalanceHandler lives in MySharedTypes.Contracts, which sits in the shared/ folder.
public static PlayerCapability<IBalanceHandler> BalanceCapability { get; } = new("myplugin:balance");

// A plugin capability is a single service for the whole plugin.
public static PluginCapability<IBalanceService> BalanceServiceCapability { get; } = new("myplugin:balance_service");
```
<sup><a href='/examples/WithSharedTypes/WithSharedTypesPlugin.cs#L14-L21' title='Snippet source file'>snippet source</a> | <a href='#snippet-capabilities-declare' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Register the implementation

The plugin that owns the data registers an implementation.

<!-- snippet: capabilities-register -->
<a id='snippet-capabilities-register'></a>
```cs
Capabilities.RegisterPlayerCapability(BalanceCapability, player => new BalanceHandler(player));
Capabilities.RegisterPluginCapability(BalanceServiceCapability, () => new BalanceService());
```
<sup><a href='/examples/WithSharedTypes/WithSharedTypesPlugin.cs#L25-L28' title='Snippet source file'>snippet source</a> | <a href='#snippet-capabilities-register' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Consume from another plugin

A consumer declares a capability with the same name and calls `Get`.

<!-- snippet: capabilities-consume -->
<a id='snippet-capabilities-consume'></a>
```cs
// Declare the same capability with the same name to read from the producer plugin.
public static PlayerCapability<IBalanceHandler> BalanceCapability { get; } = new("myplugin:balance");
public static PluginCapability<IBalanceService> BalanceServiceCapability { get; } = new("myplugin:balance_service");

public override void Load(bool hotReload)
{
    AddCommand("css_subtract", "Subtracts 50 from your balance", (player, info) =>
    {
        if (player == null) return;
        var balance = BalanceCapability.Get(player);
        if (balance == null) return;
        balance.Subtract(50);
        player.PrintToChat($"Your balance is now {balance.Balance}");
    });

    AddCommand("css_clearbalances", "Clears all balances", (player, info) =>
    {
        var service = BalanceServiceCapability.Get();
        service?.ClearAllBalances();
    });
}
```
<sup><a href='/examples/WithSharedTypesConsumer/WithSharedTypesConsumerPlugin.cs#L14-L36' title='Snippet source file'>snippet source</a> | <a href='#snippet-capabilities-consume' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

`Get` returns `null` if no plugin has registered an implementation. Always
guard against `null`.

## Dependency resolution

Plugins can resolve external assemblies in two ways:

1. **Shared folder**: copy DLLs into `shared/<PackageName>/<Assembly>.dll`.
2. **NuGet resolver**: when enabled, missing assemblies are resolved from the
   local NuGet packages cache.

To enable the NuGet resolver, set the following in the core config:

```json
{
  "PluginResolveNugetPackages": true
}
```

The resolver checks the `NUGET_PACKAGES` environment variable first, then
falls back to the default user cache (`~/.nuget/packages` on Linux and macOS,
`%UserProfile%\.nuget\packages` on Windows).

Resolution order:

1. The plugin directory itself.
2. The `shared/` folder.
3. The NuGet cache (if the resolver is enabled).

:::note
The NuGet resolver is disabled by default. Existing `shared/` workflows keep
working with or without it.
:::
