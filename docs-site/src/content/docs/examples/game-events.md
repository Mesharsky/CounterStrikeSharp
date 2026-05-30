---
title: Game events
description: Subscribe to player death and round start events.
sidebar:
  order: 3
---

How to subscribe to legacy Source 1 game events, both at load time and with
an attribute, including pre handlers that block the broadcast.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithGameEventHandlers)

## Register at load

<!-- snippet: events-register-handler -->
<a id='snippet-events-register-handler'></a>
```cs
RegisterEventHandler<EventPlayerDeath>((@event, info) =>
{
    // info.DontBroadcast can be set in pre hooks to hide the event from clients.
    if (!@event.Headshot)
    {
        @event.Attacker?.PrintToChat("Skipping player_death broadcast");
        info.DontBroadcast = true;
    }

    return HookResult.Continue;
}, HookMode.Pre);
```
<sup><a href='/examples/WithGameEventHandlers/WithGameEventHandlersPlugin.cs#L18-L30' title='Snippet source file'>snippet source</a> | <a href='#snippet-events-register-handler' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Register with an attribute

<!-- snippet: events-attribute-handler -->
<a id='snippet-events-attribute-handler'></a>
```cs
[GameEventHandler]
public HookResult OnPlayerBlind(EventPlayerBlind @event, GameEventInfo info)
{
    Logger.LogInformation("Player was blinded for {Duration}s", @event.BlindDuration);
    return HookResult.Continue;
}

[GameEventHandler(HookMode.Pre)]
public HookResult OnEventRoundStartPre(EventRoundStart @event, GameEventInfo info)
{
    Logger.LogInformation("Round started with timelimit {Timelimit}", @event.Timelimit);
    return HookResult.Continue;
}
```
<sup><a href='/examples/WithGameEventHandlers/WithGameEventHandlersPlugin.cs#L33-L47' title='Snippet source file'>snippet source</a> | <a href='#snippet-events-attribute-handler' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
