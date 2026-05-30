---
title: Admin command attributes
description: Guard plugin commands with permission flags using attributes.
---

`RequiresPermissions` and `RequiresPermissionsOr` gate a command by the
caller's permissions.

- `RequiresPermissions` passes only when the caller has **every** listed
  flag.
- `RequiresPermissionsOr` passes when the caller has **any** of the listed
  flags.

```csharp
[RequiresPermissions("@css/slay", "@custom/permission")]
public void OnMyCommand(CCSPlayerController? caller, CommandInfo info) { }

[RequiresPermissionsOr("@css/ban", "@custom/permission-2")]
public void OnMyOtherCommand(CCSPlayerController? caller, CommandInfo info) { }
```

## Stacking attributes

Attributes stack. Both must pass for the command to run.

```csharp
// (@css/cvar AND @custom/permission-1) AND (@css/ban OR @custom/permission-2)
[RequiresPermissions("@css/cvar", "@custom/permission-1")]
[RequiresPermissionsOr("@css/ban", "@custom/permission-2")]
public void OnMyComplexCommand(CCSPlayerController? caller, CommandInfo info) { }
```

## Group checks

The same attribute checks for groups. Group names start with `#`.

```csharp
[RequiresPermissions("#css/simple-admin")]
public void OnMyGroupCommand(CCSPlayerController? caller, CommandInfo info) { }
```
