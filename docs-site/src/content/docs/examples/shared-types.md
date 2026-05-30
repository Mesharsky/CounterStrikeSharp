---
title: Shared types
description: Expose a typed API to other plugins through capabilities.
sidebar:
  order: 15
---

The producer plugin registers an implementation of a contract interface. Any
consumer plugin asks for the same capability name and uses it.

Producer: [View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithSharedTypes)

Consumer: [View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithSharedTypesConsumer)

## Declare capabilities

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

<!-- snippet: capabilities-register -->
<a id='snippet-capabilities-register'></a>
```cs
Capabilities.RegisterPlayerCapability(BalanceCapability, player => new BalanceHandler(player));
Capabilities.RegisterPluginCapability(BalanceServiceCapability, () => new BalanceService());
```
<sup><a href='/examples/WithSharedTypes/WithSharedTypesPlugin.cs#L25-L28' title='Snippet source file'>snippet source</a> | <a href='#snippet-capabilities-register' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Consume from another plugin

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
