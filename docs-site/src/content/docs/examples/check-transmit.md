---
title: Check transmit
description: Hide entities from specific players using the CheckTransmit listener.
sidebar:
  order: 16
---

`Listeners.CheckTransmit` runs once per player per tick. Removing an entity
from `info.TransmitEntities` hides it from that player only.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithCheckTransmit)

<!-- snippet: check-transmit-hide-doors -->
<a id='snippet-check-transmit-hide-doors'></a>
```cs
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
```
<sup><a href='/examples/WithCheckTransmit/WithCheckTransmitPlugin.cs#L24-L42' title='Snippet source file'>snippet source</a> | <a href='#snippet-check-transmit-hide-doors' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
