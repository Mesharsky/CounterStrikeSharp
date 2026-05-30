using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using Microsoft.Extensions.Logging;

namespace WithGameEventHandlers;

[MinimumApiVersion(80)]
public class WithGameEventHandlersPlugin : BasePlugin
{
    public override string ModuleName => "Example: With Game Event Handlers";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "CounterStrikeSharp & Contributors";
    public override string ModuleDescription => "A simple plugin that subscribes to game events";

    public override void Load(bool hotReload)
    {
        // begin-snippet: events-register-handler
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
        // end-snippet
    }

    // begin-snippet: events-attribute-handler
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
    // end-snippet
}
