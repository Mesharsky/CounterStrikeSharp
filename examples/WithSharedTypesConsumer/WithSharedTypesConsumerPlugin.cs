using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Capabilities;
using MySharedTypes.Contracts;

namespace WithSharedTypesConsumer;

[MinimumApiVersion(184)]
public class WithSharedTypesConsumerPlugin : BasePlugin
{
    public override string ModuleName => "Example: Shared Types (Consumer)";
    public override string ModuleVersion => "1.0.0";

    // begin-snippet: capabilities-consume
    // Declare the same capability with the same name to read from the producer plugin.
    public static PlayerCapability<IBalanceHandler> BalanceCapability { get; } = new("myplugin:balance");
    public static PluginCapability<IBalanceService> BalanceServiceCapability { get; } = new("myplugin:balance_service");

    public override void Load(bool hotReload)
    {
        AddCommand("css_subtract", "Subtracts 50 from your balance", (player, info) =>
        {
            if (player == null) return;
            var balance = BalanceCapability.Get(player);
            if (balance == null) return;
            balance.Subtract(50);
            player.PrintToChat($"Your balance is now {balance.Balance}");
        });

        AddCommand("css_clearbalances", "Clears all balances", (player, info) =>
        {
            var service = BalanceServiceCapability.Get();
            service?.ClearAllBalances();
        });
    }
    // end-snippet
}
