---
title: Automatic build and deploy
description: Watch sources, build, and upload to a development server as you work.
---


Adapted from the [original guide](https://github.com/uFloppyDisk/create-cssharp-plugin/blob/c8fca43f86a61a5e874624f2f3ed39c5271c9a55/templates/standard-plugin/docs/auto-live-hot-reloading.md).

A normal plugin workflow ends up being build, copy DLLs to the server over
FTP, alt tab to the game, repeat. That is slow. With a watcher and an upload
tool it becomes: edit, alt tab, test, repeat.

:::caution
This is for development servers. Build time errors are caught by the SDK, but
runtime errors can still crash the server. Do not use this on a server that
real players are on.
:::

## 1. Build on file changes

`dotnet watch` watches your source files and rebuilds on every change.

```shell
dotnet watch build --project path/to/projectName.csproj
```

`dotnet watch` defaults to `dotnet run`. The `build` argument is required so
it builds instead.

Builds go to `bin/<config>/<framework>` by default.

```text
projectDirectory
├── projectName.csproj
└── bin
    └── Debug
        └── net10.0
            └── PLUGIN BUILDS HERE
```

:::tip
Set `<OutDir>` in the `.csproj` to put builds somewhere more convenient, for
example `<OutDir>./build/$(MSBuildProjectName)</OutDir>`.
:::

## 2. Sync the build to the server

### Windows: WinSCP

In WinSCP, choose **Commands -> Keep Remote Directory up to Date**. Point
the local side at your build directory and the remote side at
`csgo/addons/counterstrikesharp/plugins/<projectName>`. Click **Start**.

:::note
WinSCP runs on Windows and cannot watch files inside WSL. If you develop in
WSL, write the build to a Windows path with the workaround below or move the
development workflow to Windows.
:::

### Linux: lsyncd

[`lsyncd`](https://github.com/lsyncd/lsyncd) watches a local directory and
syncs changes to a remote target over rsync or SSH.

## WSL with WinSCP

To make WinSCP see your build, write the output to a Windows path.

```shell
dotnet watch build --project path/to/<projectName>.csproj \
  --property:OutDir=/mnt/<drive-letter>/some/path/<projectName>
```

Have WinSCP watch that Windows path. The
[Windows filesystem mounts in WSL](https://blogs.windows.com/windowsdeveloper/2016/07/22/fun-with-the-windows-subsystem-for-linux/#Working%20with%20Windows%20files)
docs explain the path mapping.
