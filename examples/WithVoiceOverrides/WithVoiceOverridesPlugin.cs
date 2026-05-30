using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace WithVoiceOverrides;

[MinimumApiVersion(80)]
public class WithVoiceOverridesPlugin : BasePlugin
{
    public override string ModuleName => "Example: With Voice Overrides";
    public override string ModuleVersion => "1.0.0";

    // begin-snippet: voice-listen-all
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
    // end-snippet

    // begin-snippet: voice-listen-override
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
    // end-snippet
}
