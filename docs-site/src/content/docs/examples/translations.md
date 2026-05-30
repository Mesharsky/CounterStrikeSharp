---
title: Translations
description: Translate plugin messages with the built in localizer.
sidebar:
  order: 11
---

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithTranslations)

## Basic usage

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

## Per player

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
