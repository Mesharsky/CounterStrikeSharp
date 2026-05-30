using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using Microsoft.Extensions.Logging;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace WithTimers;

[MinimumApiVersion(80)]
public class WithTimersPlugin : BasePlugin
{
    public override string ModuleName => "Example: With Timers";
    public override string ModuleVersion => "1.0.0";

    private Timer? _announceTimer;

    public override void Load(bool hotReload)
    {
        // begin-snippet: timers-one-shot
        // Fires once after 5 seconds.
        AddTimer(5.0f, () => Logger.LogInformation("Five seconds passed."));
        // end-snippet

        // begin-snippet: timers-repeating
        // Fires every 30 seconds and stops automatically on map change.
        _announceTimer = AddTimer(30.0f,
            () => Server.PrintToChatAll(" \x04[Server]\x01 Welcome to the server!"),
            TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
        // end-snippet
    }

    // begin-snippet: timers-stop
    [ConsoleCommand("css_stopannounce", "Stops the announcement timer")]
    public void OnStopAnnounce(CCSPlayerController? player, CommandInfo command)
    {
        _announceTimer?.Kill();
        _announceTimer = null;
        command.ReplyToCommand("Announcement timer stopped.");
    }
    // end-snippet

    // begin-snippet: timers-next-frame
    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        // Schedule work on the next game tick. Useful when reacting to events that
        // run mid frame and need a clean state to read from.
        Server.NextFrame(() => Logger.LogInformation("Round started at {Time}", Server.CurrentTime));
        return HookResult.Continue;
    }
    // end-snippet
}
