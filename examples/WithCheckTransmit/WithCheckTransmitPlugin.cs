using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;

namespace WithCheckTransmit;

[MinimumApiVersion(276)]
public class WithCheckTransmitPlugin : BasePlugin
{
    public override string ModuleName => "Example: With CheckTransmit";
    public override string ModuleVersion => "1.0.0";

    private readonly Dictionary<int, bool> _shouldSeeDoors = new();

    public override void Load(bool hotReload)
    {
        AddCommand("nodoors", "Toggle door visibility for the caller", (player, info) =>
        {
            if (player == null) return;
            _shouldSeeDoors[player.Slot] = !_shouldSeeDoors.GetValueOrDefault(player.Slot, true);
            info.ReplyToCommand($"You should {(_shouldSeeDoors[player.Slot] ? "see" : "not see")} doors");
        });

        // begin-snippet: check-transmit-hide-doors
        // Hide every prop_door_rotating for every player who toggled doors off.
        RegisterListener<Listeners.CheckTransmit>(infoList =>
        {
            var doors = Utilities.FindAllEntitiesByDesignerName<CPropDoorRotating>("prop_door_rotating").ToList();
            if (doors.Count == 0) return;

            foreach ((CCheckTransmitInfo info, CCSPlayerController? player) in infoList)
            {
                if (player == null) continue;
                if (_shouldSeeDoors.GetValueOrDefault(player.Slot, true)) continue;

                foreach (var door in doors)
                {
                    info.TransmitEntities.Remove(door);
                }
            }
        });
        // end-snippet
    }
}
