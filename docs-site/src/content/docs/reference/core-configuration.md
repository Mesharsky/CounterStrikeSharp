---
title: Core configuration
description: Settings in core.json that control CounterStrikeSharp itself.
---


These settings live in the CounterStrikeSharp core config file.

## PublicChatTrigger

List of characters used for public chat triggers, for example `!`.

## SilentChatTrigger

List of characters used for silent chat triggers, for example `/`. Silent
triggers do not print the original message to chat.

## FollowCS2ServerGuidelines

When `true`, CounterStrikeSharp blocks plugin functionality that is known to
get all of the server's
[Game Server Login Tokens (GSLTs)](https://blog.counter-strike.net/index.php/server_guidelines/)
banned. This setting only affects CS2.

:::caution
Disabling this setting is at your own risk and does not guarantee you will
not be banned.
:::

## PluginHotReloadEnabled

When enabled, plugins are reloaded automatically when their DLL is replaced
on disk.

## PluginAutoLoadEnabled

When enabled, plugins are loaded from the plugins directory on server start.

## ServerLanguage

Default culture for server commands and messages. The format is
`languagecode2-country/regioncode2`, for example `en-US` or `ja-JP`. Defaults
to `en`.

## UnlockConCommands

When enabled, removes `FCVAR_HIDDEN`, `FCVAR_DEVELOPMENTONLY`,
`FCVAR_MISSING0`, `FCVAR_MISSING1`, `FCVAR_MISSING2`, and `FCVAR_MISSING3`
from every console command.

## UnlockConVars

Same as above but applied to console variables instead of commands.

## AutoUpdateEnabled

When enabled, CounterStrikeSharp checks for `gamedata.json` updates and
applies them automatically.

## AutoUpdateURL

The URL the auto updater checks. Must point at a JSON file in the
`gamedata.json` format.

## MaximumFrameTasksExecutedPerTick

Caps how many `NextFrame` and `NextWorldUpdate` tasks run in a single tick.
The two queues are tracked separately. Defaults to `1024`. Lower the value to
smooth out frame bursts.

## PluginResolveNugetPackages

When `true`, plugins can resolve missing assemblies from the local NuGet
packages cache instead of needing them copied to `shared/`. Defaults to
`false`. See [shared plugin API](/features/shared-plugin-api/) for details.
