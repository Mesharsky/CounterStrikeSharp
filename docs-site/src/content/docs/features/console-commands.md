---
title: Console commands
description: Register chat and console commands in a CounterStrikeSharp plugin.
---


Any command prefixed with `css_` is also exposed as a chat command without the
prefix. `css_ping` can be called from chat with `!ping` or `/ping`.

## Register at load

Use `AddCommand` to register a command at runtime. This is the right choice
when the command name or behaviour is computed from a config or another
runtime value.

<!-- snippet: commands-add-command -->
<a id='snippet-commands-add-command'></a>
```cs
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
```
<sup><a href='/examples/WithCommands/WithCommandsPlugin.cs#L20-L33' title='Snippet source file'>snippet source</a> | <a href='#snippet-commands-add-command' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

If `player` is `null` the command was called from the server console.

## Register with an attribute

The `[ConsoleCommand]` attribute registers a command method on plugin load.
CounterStrikeSharp registers and deregisters these for you on hot reload.

<!-- snippet: commands-attribute -->
<a id='snippet-commands-attribute'></a>
```cs
[ConsoleCommand("css_hello", "Says hello to a player")]
[CommandHelper(minArgs: 1, usage: "[name]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
public void OnHelloCommand(CCSPlayerController? player, CommandInfo info)
{
    // Arg 0 is always the command name itself.
    var name = info.GetArg(1);
    info.ReplyToCommand($"Hello {name}");
}
```
<sup><a href='/examples/WithCommands/WithCommandsPlugin.cs#L36-L45' title='Snippet source file'>snippet source</a> | <a href='#snippet-commands-attribute' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## CommandHelper

`[CommandHelper]` checks the argument count and restricts who can run the
command. Valid `CommandUsage` values are `CLIENT_AND_SERVER`, `CLIENT_ONLY`,
and `SERVER_ONLY`.

If a client runs a command with too few arguments, they see the usage:

```text
[CSS] Expected usage: "!freeze [target]".
```

If a command is restricted and the caller does not match, they see:

```text
[CSS] This command can only be executed by clients.
```

## Requiring permissions

`[RequiresPermissions]` blocks the command unless the caller has every listed
permission. `[RequiresPermissionsOr]` allows the command if the caller has any
of them. See the [admin framework](/admin-framework/admin-command-attributes/)
docs for details.

<!-- snippet: commands-permissions -->
<a id='snippet-commands-permissions'></a>
```cs
[ConsoleCommand("css_kick", "Kicks a player by id")]
[RequiresPermissions("@css/kick")]
[CommandHelper(minArgs: 1, usage: "[userid]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
public void OnKickCommand(CCSPlayerController? player, CommandInfo info)
{
    var id = info.GetArg(1);
    Server.ExecuteCommand($"kick {id}");
}
```
<sup><a href='/examples/WithCommands/WithCommandsPlugin.cs#L47-L56' title='Snippet source file'>snippet source</a> | <a href='#snippet-commands-permissions' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Reading arguments

`CommandInfo` exposes the raw arguments. `GetArg(0)` is always the command
name itself. Quoted arguments come back without their quotes.

```csharp
[ConsoleCommand("custom_command", "Example")]
public void OnCommand(CCSPlayerController? player, CommandInfo command)
{
    command.ReplyToCommand($@"
ArgCount: {command.ArgCount}
ArgString: {command.ArgString}
GetCommandString: {command.GetCommandString}
Arg 0: {command.ArgByIndex(0)}
Arg 1: {command.ArgByIndex(1)}");
}
```
