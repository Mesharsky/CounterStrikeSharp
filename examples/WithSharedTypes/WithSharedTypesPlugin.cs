using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Capabilities;
using MySharedTypes.Contracts;

namespace WithSharedTypes;

[MinimumApiVersion(184)]
public class WithSharedTypesPlugin : BasePlugin
{
    public override string ModuleName => "Example: Shared Types";
    public override string ModuleVersion => "1.0.0";

    // begin-snippet: capabilities-declare
    // A player capability is keyed per player. Other plugins use the same name to reach it.
    // IBalanceHandler lives in MySharedTypes.Contracts, which sits in the shared/ folder.
    public static PlayerCapability<IBalanceHandler> BalanceCapability { get; } = new("myplugin:balance");

    // A plugin capability is a single service for the whole plugin.
    public static PluginCapability<IBalanceService> BalanceServiceCapability { get; } = new("myplugin:balance_service");
    // end-snippet

    public override void Load(bool hotReload)
    {
        // begin-snippet: capabilities-register
        Capabilities.RegisterPlayerCapability(BalanceCapability, player => new BalanceHandler(player));
        Capabilities.RegisterPluginCapability(BalanceServiceCapability, () => new BalanceService());
        // end-snippet

        AddCommand("css_balance", "Gets your current balance", (player, info) =>
        {
            if (player == null) return;
            player.PrintToChat($"Your balance is {BalanceCapability.Get(player)?.Balance}");
        });

        AddCommand("css_give", "Gives you money", (player, info) =>
        {
            if (player == null) return;
            var balance = BalanceCapability.Get(player);
            if (balance == null) return;
            balance.Add(100);
            player.PrintToChat($"Your balance is now {balance.Balance}");
        });
    }
}
