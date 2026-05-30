---
title: Timers
description: Schedule one shot and repeating work on the game thread.
---


Timers run on the game thread, so anything they do can call into the API
safely.

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

Pass `TimerFlags.REPEAT` to keep the timer firing. `STOP_ON_MAPCHANGE` is
useful for cleanup so the timer does not survive a map change.

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

## Stopping

Keep the returned `Timer` reference so you can stop it.

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

`Server.NextFrame` is the right choice when you need to run code on the game
thread one tick later. It is cheaper than a timer and is the standard way to
hop back to the game thread from `Task.Run`.

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

:::caution
Never call into the game API from `Task.Run` directly. Always wrap the call in
`Server.NextFrame` so it lands on the game thread.
:::
