---
title: Global listeners
description: Subscribe to framework wide listeners like map start, client connect, and check transmit.
---

Global listeners are framework events that are not Source 1 style game events.
They are not auto registered by attribute; you register them at load.

```csharp
public override void Load(bool hotReload)
{
    RegisterListener<Listeners.OnMapStart>(name =>
    {
        Logger.LogInformation("Map {Name} has started", name);
    });

    RegisterListener<Listeners.OnClientConnect>((slot, name, ip) =>
    {
        Logger.LogInformation("Client {Name} from {Ip} connected", name, ip);
    });
}
```

## Entity spawn

`Listeners.OnEntitySpawned` fires for every entity the engine creates. Use the
`DesignerName` to filter and cast to a more specific wrapper.

```csharp
RegisterListener<Listeners.OnEntitySpawned>(entity =>
{
    if (entity.DesignerName != "smokegrenade_projectile") return;
    var projectile = entity.As<CSmokeGrenadeProjectile>();

    // Reads on freshly spawned entities are safer on the next frame.
    Server.NextFrame(() =>
    {
        projectile.SmokeColor.X = Random.Shared.NextSingle() * 255f;
        projectile.SmokeColor.Y = Random.Shared.NextSingle() * 255f;
        projectile.SmokeColor.Z = Random.Shared.NextSingle() * 255f;
    });
});
```

## CheckTransmit

`Listeners.CheckTransmit` runs once per player per tick, just before the
engine decides which entities are sent to them. Remove entities from
`info.TransmitEntities` to hide them.

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

The full list of listeners lives in `CounterStrikeSharp.API.Core.Listeners`.
