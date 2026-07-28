---
title: Upgrading
description: How to upgrade CounterStrikeSharp and what .NET versions are supported.
---


## Upgrading CounterStrikeSharp

To upgrade, download the latest release and copy it over the existing
installation, the same way you installed it the first time.

CounterStrikeSharp does not overwrite your configuration files when you do
this. Once installed the first time, later releases can use the smaller build
that does not include the .NET runtime, as long as you keep your runtime up
to date yourself.

## .NET version support

CounterStrikeSharp is built on .NET 10. The example plugins in the repository
all target `net10.0`.

:::note[Existing .NET 8 plugins keep working]
Existing .NET 8 plugins are still fully supported and do not need to be
recompiled to run on the current framework. For better performance,
retargeting a plugin to .NET 10 is recommended, but it is not required.
:::

To retarget an existing plugin, change the `TargetFramework` in the
`.csproj` from `net8.0` to `net10.0`, then rebuild.

```diff
- <TargetFramework>net10.0</TargetFramework>
+ <TargetFramework>net10.0</TargetFramework>
```

## Breaking changes

Breaking changes are listed in
[CHANGELOG.md](https://github.com/roflmuffin/CounterStrikeSharp/blob/main/CHANGELOG.md).
Review it before upgrading a production server.
