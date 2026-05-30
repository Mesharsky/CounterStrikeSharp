---
title: Voice overrides
description: Change voice flags and per player listen overrides.
sidebar:
  order: 14
---

Two flavours of voice control. `VoiceFlags` are global to the player, for
example listening across teams. `SetListenOverride` is a one to one switch
between two specific players.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithVoiceOverrides)

## Hear both teams

<!-- snippet: voice-listen-all -->
<a id='snippet-voice-listen-all'></a>
```cs
[ConsoleCommand("css_hearall", "Toggles hearing both teams")]
public void OnHearAllCommand(CCSPlayerController? caller, CommandInfo command)
{
    if (caller is null) return;

    if (caller.VoiceFlags.HasFlag(VoiceFlags.ListenAll))
    {
        caller.VoiceFlags = VoiceFlags.Normal;
        command.ReplyToCommand("Voice set back to default");
    }
    else
    {
        caller.VoiceFlags = VoiceFlags.ListenAll;
        command.ReplyToCommand("Can hear both teams");
    }
}
```
<sup><a href='/examples/WithVoiceOverrides/WithVoiceOverridesPlugin.cs#L14-L31' title='Snippet source file'>snippet source</a> | <a href='#snippet-voice-listen-all' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Per player override

<!-- snippet: voice-listen-override -->
<a id='snippet-voice-listen-override'></a>
```cs
[ConsoleCommand("css_muteothers", "Mutes the target for the caller only")]
[CommandHelper(minArgs: 1, usage: "[target]")]
public void OnMuteOthersCommand(CCSPlayerController? caller, CommandInfo command)
{
    if (caller is null) return;

    var targetResult = command.GetArgTargetResult(1);
    foreach (var player in targetResult.Players)
    {
        if (player == caller) continue;

        var current = caller.GetListenOverride(player);
        if (current == ListenOverride.Mute)
        {
            caller.SetListenOverride(player, ListenOverride.Default);
            command.ReplyToCommand($"Now hearing {player.PlayerName}");
        }
        else
        {
            caller.SetListenOverride(player, ListenOverride.Mute);
            command.ReplyToCommand($"Muted {player.PlayerName}");
        }
    }
}
```
<sup><a href='/examples/WithVoiceOverrides/WithVoiceOverridesPlugin.cs#L33-L58' title='Snippet source file'>snippet source</a> | <a href='#snippet-voice-listen-override' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
