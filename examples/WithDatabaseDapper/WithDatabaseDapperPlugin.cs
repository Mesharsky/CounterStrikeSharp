using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace WithDatabaseDapper;

[MinimumApiVersion(80)]
public class WithDatabaseDapperPlugin : BasePlugin
{
    public override string ModuleName => "Example: With Database (Dapper)";
    public override string ModuleVersion => "1.0.0";

    private SqliteConnection _connection = null!;

    // begin-snippet: db-open-connection
    public override void Load(bool hotReload)
    {
        var dbPath = Path.Join(ModuleDirectory, "database.db");
        _connection = new SqliteConnection($"Data Source={dbPath}");
        _connection.Open();

        // Run schema creation off the main thread so it does not block the server.
        Task.Run(async () =>
        {
            await _connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS `players` (
                    `steamid` UNSIGNED BIG INT NOT NULL,
                    `kills` INT NOT NULL DEFAULT 0,
                    PRIMARY KEY (`steamid`));");
        });
    }
    // end-snippet

    // begin-snippet: db-write-on-event
    [GameEventHandler]
    public HookResult OnPlayerKilled(EventPlayerDeath @event, GameEventInfo info)
    {
        if (@event.Attacker == @event.Userid) return HookResult.Continue;

        // Capture the value before leaving the event scope. The event object is
        // not safe to read inside Task.Run.
        var steamId = @event.Attacker.AuthorizedSteamID?.SteamId64;
        if (steamId == null) return HookResult.Continue;

        Task.Run(async () =>
        {
            await _connection.ExecuteAsync(@"
                INSERT INTO `players` (`steamid`, `kills`) VALUES (@SteamId, 1)
                ON CONFLICT(`steamid`) DO UPDATE SET `kills` = `kills` + 1;",
                new { SteamId = steamId });
        });

        return HookResult.Continue;
    }
    // end-snippet

    // begin-snippet: db-read-and-reply
    [ConsoleCommand("css_kills", "Shows your stored kill count")]
    public void OnKillsCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null) return;
        var steamId = player.AuthorizedSteamID?.SteamId64;
        if (steamId == null) return;

        Task.Run(async () =>
        {
            var row = await _connection.QueryFirstOrDefaultAsync(
                "SELECT `kills` FROM `players` WHERE `steamid` = @SteamId;",
                new { SteamId = steamId });

            // Hop back to the game thread to talk to the player.
            Server.NextFrame(() => player.PrintToChat($"Kills: {row?.kills ?? 0}"));
        });
    }
    // end-snippet
}
