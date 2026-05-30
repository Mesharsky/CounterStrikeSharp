---
title: Getting started
description: Install Metamod and CounterStrikeSharp on a Counter-Strike 2 server.
---


This guide walks through installing CounterStrikeSharp on a vanilla
Counter-Strike 2 dedicated server. CounterStrikeSharp uses Metamod:Source to
talk to the game server, so both have to be installed.

If you prefer a video, there is a
[YouTube walkthrough](https://www.youtube.com/watch?v=FlsKzStHJuY) that covers
the same steps.

## Prerequisites

- [Metamod:Source 2.x dev build](https://www.metamodsource.net/downloads.php/?branch=master)
- [CounterStrikeSharp with runtime](https://github.com/roflmuffin/CounterStrikeSharp/releases)

## Install Metamod

1. Extract Metamod and copy its `addons/` directory into `game/csgo/`.
2. Open `game/csgo/gameinfo.gi`.
3. Below the `Game_LowViolence csgo_lv` line, add `Game csgo/addons/metamod`.
4. Restart the server.

Your `gameinfo.gi` should look like
[this](/gameinfogi-example.png). Type `meta list` in your server console to
check Metamod loaded.

## Install CounterStrikeSharp

1. Extract CounterStrikeSharp and copy its `addons/` directory into `game/csgo/`.
2. Restart the server.

Running `meta list` again should now show one loaded plugin.

```shell
meta list
Listing 1 plugin:
  [01] CounterStrikeSharp (0.1.0) by Roflmuffin
```

:::caution
On Windows you also need the
[Visual Studio C++ Redistributables](https://aka.ms/vs/17/release/vc_redist.x64.exe).
Without them CounterStrikeSharp will not load.
:::

## Folder layout

After installing both, the server should look like this.

```text
<server_path>/game/csgo/addons
├── counterstrikesharp
│   ├── api
│   ├── bin
│   ├── dotnet
│   ├── plugins
│   └── gamedata
│
├── metamod
│   ├── bin
│   ├── counterstrikesharp.vdf
│   ├── metaplugins.ini
│   └── README.txt
├── metamod.vdf
└── metamod_x64.vdf
```

## Target framework

CounterStrikeSharp is built on .NET 10. New example plugins in the repository
target `net10.0`. Existing plugins built against .NET 8 still load and run on
the current framework without recompiling. Retargeting an older plugin to
.NET 10 gives better performance but is optional.

## Troubleshooting

- First time installs must use the `with-runtime` build. It bundles the
  .NET runtime that CounterStrikeSharp needs.
- On Linux you may also need `libicu` / `icu-libs` / `libicu-dev` from your
  package manager.
- If `meta list` returns `Unknown Command`, the addons folder or the
  `gameinfo.gi` edit is not correct.

See [upgrading](/guides/upgrading/) for notes on moving between
CounterStrikeSharp releases.
