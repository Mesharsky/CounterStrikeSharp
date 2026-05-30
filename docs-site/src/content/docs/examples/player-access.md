---
title: Player and entity access
description: Resolve players, read pawn state, iterate entities.
sidebar:
  order: 6
---

Common patterns for working with players and entities.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithPlayerAccess)

## Resolve a player

<!-- snippet: player-lookup -->
<a id='snippet-player-lookup'></a>
```cs
// Three ways to resolve a player. Pick the one that matches the value you have.
[ConsoleCommand("css_findplayer", "Looks up a player by slot, userid or index")]
[CommandHelper(minArgs: 2, usage: "[slot|userid|index] [value]")]
public void OnFindPlayer(CCSPlayerController? caller, CommandInfo command)
{
    var kind = command.GetArg(1);
    if (!int.TryParse(command.GetArg(2), out var value)) return;

    var player = kind switch
    {
        "slot" => Utilities.GetPlayerFromSlot(value),
        "userid" => Utilities.GetPlayerFromUserid(value),
        "index" => Utilities.GetPlayerFromIndex(value),
        _ => null
    };

    if (player == null || !player.IsValid)
    {
        command.ReplyToCommand("No matching player");
        return;
    }

    command.ReplyToCommand($"Found {player.PlayerName} (slot {player.Slot}, userid {player.UserId})");
}
```
<sup><a href='/examples/WithPlayerAccess/WithPlayerAccessPlugin.cs#L17-L42' title='Snippet source file'>snippet source</a> | <a href='#snippet-player-lookup' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Controller vs pawn

<!-- snippet: player-controller-and-pawn -->
<a id='snippet-player-controller-and-pawn'></a>
```cs
// The controller represents the player connection. The pawn represents their
// physical character. To change health, write to the pawn. To read SteamID,
// read from the controller.
[ConsoleCommand("css_heal", "Heals the caller to 100 hp")]
public void OnHeal(CCSPlayerController? player, CommandInfo command)
{
    if (player == null) return;
    var pawn = player.PlayerPawn.Value;
    if (pawn == null) return;

    pawn.Health = 100;
    Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
}
```
<sup><a href='/examples/WithPlayerAccess/WithPlayerAccessPlugin.cs#L44-L58' title='Snippet source file'>snippet source</a> | <a href='#snippet-player-controller-and-pawn' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Iterate players

<!-- snippet: player-iterate -->
<a id='snippet-player-iterate'></a>
```cs
// Utilities.GetPlayers returns every connected player controller.
[ConsoleCommand("css_list", "Lists every connected player")]
public void OnList(CCSPlayerController? caller, CommandInfo command)
{
    foreach (var player in Utilities.GetPlayers())
    {
        command.ReplyToCommand($"{player.UserId} {player.PlayerName} ({player.Team})");
    }
}
```
<sup><a href='/examples/WithPlayerAccess/WithPlayerAccessPlugin.cs#L60-L70' title='Snippet source file'>snippet source</a> | <a href='#snippet-player-iterate' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Find entities

<!-- snippet: entity-find -->
<a id='snippet-entity-find'></a>
```cs
// FindAllEntitiesByDesignerName lets you walk an entity type by class name.
[ConsoleCommand("css_doors", "Counts every door on the map")]
public void OnDoors(CCSPlayerController? caller, CommandInfo command)
{
    var doors = Utilities.FindAllEntitiesByDesignerName<CPropDoorRotating>("prop_door_rotating");
    command.ReplyToCommand($"There are {doors.Count()} doors on this map");
}
```
<sup><a href='/examples/WithPlayerAccess/WithPlayerAccessPlugin.cs#L72-L80' title='Snippet source file'>snippet source</a> | <a href='#snippet-entity-find' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Validate before reading

<!-- snippet: event-userid-safety -->
<a id='snippet-event-userid-safety'></a>
```cs
// event.Userid is a controller. It can become invalid between the event firing
// and any code that runs later, so check before you read.
[GameEventHandler]
public HookResult OnSpawn(EventPlayerSpawn @event, GameEventInfo info)
{
    var player = @event.Userid;
    if (player == null || !player.IsValid) return HookResult.Continue;
    if (!player.PlayerPawn.IsValid) return HookResult.Continue;

    Logger.LogInformation("{Name} spawned", player.PlayerName);
    return HookResult.Continue;
}
```
<sup><a href='/examples/WithPlayerAccess/WithPlayerAccessPlugin.cs#L82-L95' title='Snippet source file'>snippet source</a> | <a href='#snippet-event-userid-safety' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
