using System.Globalization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

namespace WithTranslations;

[MinimumApiVersion(80)]
public class WithTranslationsPlugin : BasePlugin
{
    public override string ModuleName => "Example: With Translations";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        // begin-snippet: translations-basic-usage
        // Localizer is provided on every BasePlugin. It uses the server culture by default.
        Logger.LogInformation(Localizer["test.translation"]);

        // Standard string formatting works.
        Logger.LogInformation(Localizer["test.format", 123.551]);

        // Print to all players in the server culture.
        Server.PrintToChatAll(Localizer["test.colors"]);
        // end-snippet

        PrintToAllPlayersLocalized("test.format", 123.456);
    }

    // begin-snippet: translations-per-player
    void PrintToAllPlayersLocalized(string key, params object[] args)
    {
        foreach (var player in Utilities.GetPlayers().Where(x => x.IsValid))
        {
            // ForPlayer picks up the language the player chose with the !lang command.
            player.PrintToChat(Localizer.ForPlayer(player, key, args));
        }
    }
    // end-snippet

    [ConsoleCommand("css_replylanguage", "Replies in the caller language")]
    public void OnCommandReplyLanguage(CCSPlayerController? player, CommandInfo command)
    {
        Logger.LogInformation("Current Culture is {Culture}", CultureInfo.CurrentCulture);
        command.ReplyToCommand(Localizer["test.translation"]);
    }
}
