---
title: Config
description: Read and write a plugin config file with IPluginConfig.
sidebar:
  order: 7
---

`BasePluginConfig` plus `IPluginConfig<T>` give you automatic config loading.
`OnConfigParsed` is the right place to validate values.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithConfig)

## Config class

<!-- snippet: config-class -->
<a id='snippet-config-class'></a>
```cs
public class SampleConfig : BasePluginConfig
{
    [JsonPropertyName("ChatPrefix")] public string ChatPrefix { get; set; } = "My Cool Plugin";
    [JsonPropertyName("ChatInterval")] public float ChatInterval { get; set; } = 60;
}
```
<sup><a href='/examples/WithConfig/WithConfigPlugin.cs#L11-L17' title='Snippet source file'>snippet source</a> | <a href='#snippet-config-class' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Plugin

<!-- snippet: config-plugin -->
<a id='snippet-config-plugin'></a>
```cs
[MinimumApiVersion(80)]
public class WithConfigPlugin : BasePlugin, IPluginConfig<SampleConfig>
{
    public override string ModuleName => "Example: With Config";
    public override string ModuleVersion => "1.0.0";

    public SampleConfig Config { get; set; } = null!;

    public void OnConfigParsed(SampleConfig config)
    {
        // Validate and clamp values that came in from disk.
        if (config.ChatInterval > 60)
        {
            config.ChatInterval = 60;
        }

        if (config.ChatPrefix.Length > 25)
        {
            throw new Exception($"Invalid ChatPrefix: {config.ChatPrefix}");
        }

        Config = config;
    }

    [ConsoleCommand("css_reload_config", "Reloads the plugin config")]
    public void OnReloadConfig(CCSPlayerController? player, CommandInfo info)
    {
        info.ReplyToCommand("Chat Interval before reload: " + Config.ChatInterval);
        Config.Reload();
        info.ReplyToCommand("Chat Interval after reload: " + Config.ChatInterval);
    }

    [ConsoleCommand("css_reset_config", "Resets the plugin config")]
    public void OnResetConfig(CCSPlayerController? player, CommandInfo info)
    {
        Config.ChatInterval = 60;
        Config.Update();
        info.ReplyToCommand("Config reset and written back to disk");
    }
}
```
<sup><a href='/examples/WithConfig/WithConfigPlugin.cs#L19-L60' title='Snippet source file'>snippet source</a> | <a href='#snippet-config-plugin' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
