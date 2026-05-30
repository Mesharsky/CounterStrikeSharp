---
title: Game events
description: Listen to Source 1 style game events from a plugin.
---


CounterStrikeSharp surfaces every game event as a strongly typed class. Event
names follow the
[game event list](https://cs2.poggu.me/dumped-data/game-events). For example
`player_spawn` becomes `EventPlayerSpawn`.

## Register at load

Use `RegisterEventHandler` when you want to capture local variables in the
handler.

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

Methods decorated with `[GameEventHandler]` are registered on plugin load.
The event type is inferred from the first parameter. The `HookMode` argument
chooses between pre and post hooks; without it, post is used.

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

## Mutating events

Event properties are mutable. Assigning to a property writes back to the
underlying event before downstream handlers run.

## Blocking the event from reaching clients

`info.DontBroadcast = true` in a pre handler prevents the event from being
broadcast to clients. Useful for hiding kill feed messages.

## Cancelling further handlers

Returning `HookResult.Handled` or `HookResult.Stop` from a pre handler
prevents later handlers from running.

:::caution
Event objects (`@event`) and their values become invalid as soon as the
handler returns. If you need the data inside a timer or `Server.NextFrame`,
copy it to a local variable first.
:::
