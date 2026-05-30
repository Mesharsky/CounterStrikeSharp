---
title: Defining admin groups
description: Group permissions together and assign admins to groups.
---


Groups bundle a set of permissions under a single name. They live in
`configs/admin_groups.json`.

```json
"#css/simple-admin": {
  "flags": [
    "@css/generic",
    "@css/reservation",
    "@css/ban",
    "@css/slay"
  ]
}
```

Assign an admin to one or more groups in `configs/admins.json`.

```json
{
  "erikj": {
    "identity": "76561198808392634",
    "flags": ["@mycustomplugin/admin"],
    "groups": ["#css/simple-admin"]
  }
}
```

:::note
Group names must start with `#`. Without it CounterStrikeSharp will not
recognise the group.
:::

Admins inherit every flag from every group they belong to. You can also assign
groups at runtime with `AdminManager.AddPlayerToGroup` and
`AdminManager.RemovePlayerFromGroup`.
