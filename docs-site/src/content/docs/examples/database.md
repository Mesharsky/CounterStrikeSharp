---
title: Database (Dapper)
description: Read and write a SQLite database with Dapper without blocking the game thread.
sidebar:
  order: 10
---

A SQLite database keyed by SteamID. Writes happen on a background thread.
Reads pop back to the game thread with `Server.NextFrame` before printing to
the player.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithDatabaseDapper)

## Open the connection on load

<!-- snippet: db-open-connection -->
<a id='snippet-db-open-connection'></a>
```cs
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
```
<sup><a href='/examples/WithDatabaseDapper/WithDatabaseDapperPlugin.cs#L20-L37' title='Snippet source file'>snippet source</a> | <a href='#snippet-db-open-connection' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Write on a game event

<!-- snippet: db-write-on-event -->
<a id='snippet-db-write-on-event'></a>
```cs
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
```
<sup><a href='/examples/WithDatabaseDapper/WithDatabaseDapperPlugin.cs#L39-L60' title='Snippet source file'>snippet source</a> | <a href='#snippet-db-write-on-event' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Read and reply

<!-- snippet: db-read-and-reply -->
<a id='snippet-db-read-and-reply'></a>
```cs
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
```
<sup><a href='/examples/WithDatabaseDapper/WithDatabaseDapperPlugin.cs#L62-L80' title='Snippet source file'>snippet source</a> | <a href='#snippet-db-read-and-reply' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
