using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace WithPlayerAccess;

[MinimumApiVersion(80)]
public class WithPlayerAccessPlugin : BasePlugin
{
    public override string ModuleName => "Example: Player Access";
    public override string ModuleVersion => "1.0.0";

    // begin-snippet: player-lookup
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
    // end-snippet

    // begin-snippet: player-controller-and-pawn
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
    // end-snippet

    // begin-snippet: player-iterate
    // Utilities.GetPlayers returns every connected player controller.
    [ConsoleCommand("css_list", "Lists every connected player")]
    public void OnList(CCSPlayerController? caller, CommandInfo command)
    {
        foreach (var player in Utilities.GetPlayers())
        {
            command.ReplyToCommand($"{player.UserId} {player.PlayerName} ({player.Team})");
        }
    }
    // end-snippet

    // begin-snippet: entity-find
    // FindAllEntitiesByDesignerName lets you walk an entity type by class name.
    [ConsoleCommand("css_doors", "Counts every door on the map")]
    public void OnDoors(CCSPlayerController? caller, CommandInfo command)
    {
        var doors = Utilities.FindAllEntitiesByDesignerName<CPropDoorRotating>("prop_door_rotating");
        command.ReplyToCommand($"There are {doors.Count()} doors on this map");
    }
    // end-snippet

    // begin-snippet: event-userid-safety
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
    // end-snippet
}
