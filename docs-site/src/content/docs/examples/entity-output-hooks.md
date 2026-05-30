---
title: Entity output hooks
description: Listen for entity I/O outputs like OnPlayerPickup.
sidebar:
  order: 13
---

Entity output hooks listen for the I/O outputs that map and entity logic
fire, like `OnPlayerPickup` on `weapon_knife`. Wildcards (`*`) work for both
the classname and the output name.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithEntityOutputHooks)

## Register at load

<!-- snippet: entity-output-hook-load -->
<a id='snippet-entity-output-hook-load'></a>
```cs
HookEntityOutput("weapon_knife", "OnPlayerPickup",
    (CEntityIOOutput output, string name,
        CEntityInstance activator, CEntityInstance caller,
        CVariant value, float delay) =>
    {
        Logger.LogInformation("knife OnPlayerPickup ({Caller})", caller.DesignerName);
        return HookResult.Continue;
    });
```
<sup><a href='/examples/WithEntityOutputHooks/WithEntityOutputHooksPlugin.cs#L16-L25' title='Snippet source file'>snippet source</a> | <a href='#snippet-entity-output-hook-load' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Register with an attribute

<!-- snippet: entity-output-hook-attribute -->
<a id='snippet-entity-output-hook-attribute'></a>
```cs
// Wildcards work for either the classname or the output name.
[EntityOutputHook("*", "OnPlayerPickup")]
public HookResult OnPickup(CEntityIOOutput output, string name,
    CEntityInstance activator, CEntityInstance caller,
    CVariant value, float delay)
{
    Logger.LogInformation("OnPlayerPickup on {Caller}", caller.DesignerName);
    return HookResult.Continue;
}

[EntityOutputHook("func_buyzone", "*")]
public HookResult OnBuyZone(CEntityIOOutput output, string name,
    CEntityInstance activator, CEntityInstance caller,
    CVariant value, float delay)
{
    Logger.LogInformation("func_buyzone fired {Name}", name);
    return HookResult.Continue;
}
```
<sup><a href='/examples/WithEntityOutputHooks/WithEntityOutputHooksPlugin.cs#L28-L47' title='Snippet source file'>snippet source</a> | <a href='#snippet-entity-output-hook-attribute' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
