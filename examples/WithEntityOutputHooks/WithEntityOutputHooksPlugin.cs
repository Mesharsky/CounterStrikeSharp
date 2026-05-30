using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using Microsoft.Extensions.Logging;

namespace WithEntityOutputHooks;

[MinimumApiVersion(80)]
public class WithEntityOutputHooksPlugin : BasePlugin
{
    public override string ModuleName => "Example: With Entity Output Hooks";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        // begin-snippet: entity-output-hook-load
        HookEntityOutput("weapon_knife", "OnPlayerPickup",
            (CEntityIOOutput output, string name,
                CEntityInstance activator, CEntityInstance caller,
                CVariant value, float delay) =>
            {
                Logger.LogInformation("knife OnPlayerPickup ({Caller})", caller.DesignerName);
                return HookResult.Continue;
            });
        // end-snippet
    }

    // begin-snippet: entity-output-hook-attribute
    // Wildcards work for either the classname or the output name.
    [EntityOutputHook("*", "OnPlayerPickup")]
    public HookResult OnPickup(CEntityIOOutput output, string name,
        CEntityInstance activator, CEntityInstance caller,
        CVariant value, float delay)
    {
        Logger.LogInformation("OnPlayerPickup on {Caller}", caller.DesignerName);
        return HookResult.Continue;
    }

    [EntityOutputHook("func_buyzone", "*")]
    public HookResult OnBuyZone(CEntityIOOutput output, string name,
        CEntityInstance activator, CEntityInstance caller,
        CVariant value, float delay)
    {
        Logger.LogInformation("func_buyzone fired {Name}", name);
        return HookResult.Continue;
    }
    // end-snippet
}
