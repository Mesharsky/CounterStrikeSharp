---
title: Defining command overrides
description: Override the permissions of a specific command per admin, group, or globally.
---

## Per admin or group override

Command overrides let a specific admin or group run a command they would not
normally have access to, or block them from one they would. Set them under
`command_overrides` in `configs/admins.json` or `configs/admin_groups.json`.

```json
{
  "ZoNiCaL": {
    "identity": "76561198808392634",
    "flags": ["@css/changemap", "@css/generic"],
    "command_overrides": {
      "example_command": true
    }
  }
}
```

```json
"#css/simple-admin": {
  "flags": ["@css/generic", "@css/ban"],
  "command_overrides": {
    "example_command_2": false
  }
}
```

Set the value to `true` to allow the command, or `false` to block it. At
runtime, `AdminManager.SetPlayerCommandOverride` does the same.

## Replacing a command's required permissions

To replace the required permissions of a command for **every** caller, use
`configs/admin_overrides.json`.

```json
"css_special": {
  "flags": ["@css/custom-permission"],
  "check_type": "all",
  "enabled": true
}
```

- `check_type` is `all` for an AND check or `any` for an OR check.
- `enabled` toggles the override on or off.

Runtime helpers: `AdminManager.CommandIsOverriden`,
`AdminManager.AddPermissionOverride`, `AdminManager.RemovePermissionOverride`,
and `AdminManager.SetCommandOverrideState`.
