---
title: Defining admins
description: Configure admin entries and permission flags.
---


CounterStrikeSharp loads admins from `configs/admins.json` at startup. Each
admin entry has a SteamID and a list of permission flags.

## Adding admins

```json
{
  "ZoNiCaL": {
    "identity": "76561198808392634",
    "flags": ["@css/changemap", "@css/generic"]
  },
  "another ZoNiCaL": {
    "identity": "STEAM_0:1:1",
    "flags": ["@css/generic"]
  }
}
```

You can also grant or revoke permissions at runtime with
`AdminManager.AddPlayerPermissions` and `AdminManager.RemovePlayerPermissions`.
Runtime changes are not written back to `admins.json`.

:::note
Permission flags must start with `@`. Group names must start with `#`. Both
prefixes are required.
:::

## Standard permission flags

Flag names are free form. You can declare your own scoped to your plugin, for
example `@roflmuffin/guns`. The conventional flags below match the original
SourceMod flags.

```text
@css/reservation # Reserved slot access.
@css/generic     # Generic admin.
@css/kick        # Kick other players.
@css/ban         # Ban other players.
@css/unban       # Remove bans.
@css/vip         # General vip status.
@css/slay        # Slay or harm other players.
@css/changemap   # Change the map or major gameplay features.
@css/cvar        # Change most cvars.
@css/config      # Execute config files.
@css/chat        # Special chat privileges.
@css/vote        # Start or create votes.
@css/password    # Set a password on the server.
@css/rcon        # Use RCON commands.
@css/cheats      # Change sv_cheats or use cheating commands.
@css/root        # Implicitly enables every flag and ignores immunity.
```

:::note
CounterStrikeSharp does not ship the admin commands themselves (slay, kick,
ban). Plugins implement those and use the flags above to gate them.
:::
