using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Config;
using CounterStrikeSharp.API.Modules.Extensions;

namespace WithConfig;

// begin-snippet: config-class
public class SampleConfig : BasePluginConfig
{
    [JsonPropertyName("ChatPrefix")] public string ChatPrefix { get; set; } = "My Cool Plugin";
    [JsonPropertyName("ChatInterval")] public float ChatInterval { get; set; } = 60;
}
// end-snippet

// begin-snippet: config-plugin
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
// end-snippet
