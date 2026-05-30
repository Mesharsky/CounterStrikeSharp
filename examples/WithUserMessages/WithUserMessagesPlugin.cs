using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.UserMessages;
using Microsoft.Extensions.Logging;

namespace WithUserMessages;

[MinimumApiVersion(80)]
public class WithUserMessagesPlugin : BasePlugin
{
    public override string ModuleName => "Example: With User Messages";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        // begin-snippet: usermessages-hook
        // Hook by user message id. 452 is CMsgTEFireBullets.
        HookUserMessage(452, um =>
        {
            // Force every weapon to sound like a silenced usp.
            um.SetUInt("weapon_id", 0);
            um.SetInt("sound_type", 9);
            um.SetUInt("item_def_index", 61);
            return HookResult.Continue;
        }, HookMode.Pre);

        // 118 is the chat message id.
        HookUserMessage(118, um =>
        {
            var author = um.ReadString("param1");
            var message = um.ReadString("param2");
            Logger.LogInformation("Chat from {Author}: {Message}", author, message);

            // Returning Stop drops the message for every recipient.
            if (message.Contains("stop")) return HookResult.Stop;

            // You can also trim recipients to selectively hide the message.
            if (message.Contains("skip")) um.Recipients.Clear();
            return HookResult.Continue;
        });
        // end-snippet
    }

    // begin-snippet: usermessages-send
    [ConsoleCommand("css_shake", "Shakes the screen for the caller or all players")]
    public void OnCommandShake(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null) return;

        // Resolve the message by a partial network name. "Shake" matches CUserMessageShake.
        var message = UserMessage.FromPartialName("Shake");
        message.SetFloat("duration", 2);
        message.SetFloat("amplitude", 5);
        message.SetFloat("frequency", 10f);
        message.SetInt("command", 0);

        if (command.GetArg(1) == "all")
        {
            message.Recipients.AddAllPlayers();
        }
        else
        {
            message.Recipients.Add(player);
        }

        message.Send();
    }
    // end-snippet
}
