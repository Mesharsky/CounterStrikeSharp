---
title: Timers
description: One shot, repeating, and next frame scheduling.
sidebar:
  order: 4
---

Common timer patterns: a one shot delay, a repeating announcement, stopping
a timer, and using `Server.NextFrame` to return to the game thread.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithTimers)

## One shot

<!-- snippet: timers-one-shot -->
<a id='snippet-timers-one-shot'></a>
```cs
// Fires once after 5 seconds.
AddTimer(5.0f, () => Logger.LogInformation("Five seconds passed."));
```
<sup><a href='/examples/WithTimers/WithTimersPlugin.cs#L22-L25' title='Snippet source file'>snippet source</a> | <a href='#snippet-timers-one-shot' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Repeating

<!-- snippet: timers-repeating -->
<a id='snippet-timers-repeating'></a>
```cs
// Fires every 30 seconds and stops automatically on map change.
_announceTimer = AddTimer(30.0f,
    () => Server.PrintToChatAll(" \x04[Server]\x01 Welcome to the server!"),
    TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
```
<sup><a href='/examples/WithTimers/WithTimersPlugin.cs#L27-L32' title='Snippet source file'>snippet source</a> | <a href='#snippet-timers-repeating' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Stop a timer

<!-- snippet: timers-stop -->
<a id='snippet-timers-stop'></a>
```cs
[ConsoleCommand("css_stopannounce", "Stops the announcement timer")]
public void OnStopAnnounce(CCSPlayerController? player, CommandInfo command)
{
    _announceTimer?.Kill();
    _announceTimer = null;
    command.ReplyToCommand("Announcement timer stopped.");
}
```
<sup><a href='/examples/WithTimers/WithTimersPlugin.cs#L35-L43' title='Snippet source file'>snippet source</a> | <a href='#snippet-timers-stop' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Run on the next tick

<!-- snippet: timers-next-frame -->
<a id='snippet-timers-next-frame'></a>
```cs
[GameEventHandler]
public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
{
    // Schedule work on the next game tick. Useful when reacting to events that
    // run mid frame and need a clean state to read from.
    Server.NextFrame(() => Logger.LogInformation("Round started at {Time}", Server.CurrentTime));
    return HookResult.Continue;
}
```
<sup><a href='/examples/WithTimers/WithTimersPlugin.cs#L45-L54' title='Snippet source file'>snippet source</a> | <a href='#snippet-timers-next-frame' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
