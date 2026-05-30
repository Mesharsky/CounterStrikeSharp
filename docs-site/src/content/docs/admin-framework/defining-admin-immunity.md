---
title: Defining admin immunity
description: Give admins immunity levels and check them in your commands.
---


Admins can have an immunity value. When two admins interact, the one with the
lower immunity cannot target the one with the higher immunity.

## Per admin

```json
{
  "ZoNiCaL": {
    "identity": "76561198808392634",
    "flags": ["@css/changemap", "@css/generic"],
    "immunity": 100
  }
}
```

## Per group

If the admin has a group with a higher immunity than their own, the group
value is used.

```json
"#css/simple-admin": {
  "flags": ["@css/generic", "@css/ban"],
  "immunity": 100
}
```

:::note
CounterStrikeSharp does not check immunity for you. Plugins call
`AdminManager.CanPlayerTarget` themselves. You can also set immunity at
runtime with `AdminManager.SetPlayerImmunity`.
:::
