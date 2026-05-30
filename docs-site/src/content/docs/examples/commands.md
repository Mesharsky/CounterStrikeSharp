---
title: Commands
description: Register chat and console commands, parse arguments, and require permissions.
sidebar:
  order: 2
---

Three styles of command registration: registered at load, registered with an
attribute, and gated by a permission flag.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithCommands)

## Register at load

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

## Register with an attribute

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

## Require a permission flag

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
