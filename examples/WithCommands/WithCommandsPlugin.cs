using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace WithCommands;

[MinimumApiVersion(80)]
public class WithCommandsPlugin : BasePlugin
{
    public override string ModuleName => "Example: With Commands";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "CounterStrikeSharp & Contributors";
    public override string ModuleDescription => "A simple plugin that registers some commands";

    public override void Load(bool hotReload)
    {
        // begin-snippet: commands-add-command
        // Any command prefixed with "css_" is also available as a chat trigger.
        // "css_ping" can be called from chat with "!ping" or "/ping".
        AddCommand("css_ping", "Responds with pong", (player, info) =>
        {
            if (player == null)
            {
                info.ReplyToCommand("pong server");
                return;
            }

            info.ReplyToCommand("pong");
        });
        // end-snippet
    }

    // begin-snippet: commands-attribute
    [ConsoleCommand("css_hello", "Says hello to a player")]
    [CommandHelper(minArgs: 1, usage: "[name]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void OnHelloCommand(CCSPlayerController? player, CommandInfo info)
    {
        // Arg 0 is always the command name itself.
        var name = info.GetArg(1);
        info.ReplyToCommand($"Hello {name}");
    }
    // end-snippet

    // begin-snippet: commands-permissions
    [ConsoleCommand("css_kick", "Kicks a player by id")]
    [RequiresPermissions("@css/kick")]
    [CommandHelper(minArgs: 1, usage: "[userid]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void OnKickCommand(CCSPlayerController? player, CommandInfo info)
    {
        var id = info.GetArg(1);
        Server.ExecuteCommand($"kick {id}");
    }
    // end-snippet
}
