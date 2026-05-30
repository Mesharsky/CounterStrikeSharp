---
title: Translations
description: Translate plugin messages using the built in IStringLocalizer.
---

`BasePlugin` exposes a `Localizer` keyed by string. It loads from JSON files
in your plugin `lang/` folder. Server admins can pick the default language in
the core config; players can pick their own with `!lang`.

## Files

Put translation files in `lang/<culture>.json` next to your plugin DLL, for
example `lang/en.json` and `lang/pl.json`. Each file is a flat key value map.

```json
{
  "test.translation": "Hello",
  "test.format": "This has a number {0:n2}",
  "test.colors": " {green}This {default}has {red}colors"
}
```

## Read a translation

<!-- snippet: translations-basic-usage -->
<a id='snippet-translations-basic-usage'></a>
```cs
// Localizer is provided on every BasePlugin. It uses the server culture by default.
Logger.LogInformation(Localizer["test.translation"]);

// Standard string formatting works.
Logger.LogInformation(Localizer["test.format", 123.551]);

// Print to all players in the server culture.
Server.PrintToChatAll(Localizer["test.colors"]);
```
<sup><a href='/examples/WithTranslations/WithTranslationsPlugin.cs#L20-L29' title='Snippet source file'>snippet source</a> | <a href='#snippet-translations-basic-usage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Translate per player

`Localizer.ForPlayer(player, key, args)` returns the message in the player
language. If the player has not set one, the server default is used.

<!-- snippet: translations-per-player -->
<a id='snippet-translations-per-player'></a>
```cs
void PrintToAllPlayersLocalized(string key, params object[] args)
{
    foreach (var player in Utilities.GetPlayers().Where(x => x.IsValid))
    {
        // ForPlayer picks up the language the player chose with the !lang command.
        player.PrintToChat(Localizer.ForPlayer(player, key, args));
    }
}
```
<sup><a href='/examples/WithTranslations/WithTranslationsPlugin.cs#L34-L43' title='Snippet source file'>snippet source</a> | <a href='#snippet-translations-per-player' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
