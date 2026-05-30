---
title: Menus
description: Chat menu and center HTML menu examples.
sidebar:
  order: 5
---

Two ways to show a menu to a player.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithMenus)

## Chat menu

<!-- snippet: menus-chat-menu -->
<a id='snippet-menus-chat-menu'></a>
```cs
[ConsoleCommand("css_colormenu", "Opens a chat menu of colors")]
public void OnColorMenu(CCSPlayerController? player, CommandInfo command)
{
    if (player == null) return;

    var menu = new ChatMenu("Pick a color");
    menu.AddMenuOption("Red", (p, option) => p.PrintToChat($"You picked {option.Text}"));
    menu.AddMenuOption("Green", (p, option) => p.PrintToChat($"You picked {option.Text}"));
    menu.AddMenuOption("Blue", (p, option) => p.PrintToChat($"You picked {option.Text}"));

    // A disabled option stays visible but cannot be selected.
    menu.AddMenuOption("Coming soon", (_, _) => { }, disabled: true);

    MenuManager.OpenChatMenu(player, menu);
}
```
<sup><a href='/examples/WithMenus/WithMenusPlugin.cs#L15-L31' title='Snippet source file'>snippet source</a> | <a href='#snippet-menus-chat-menu' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Center HTML menu

<!-- snippet: menus-center-html -->
<a id='snippet-menus-center-html'></a>
```cs
[ConsoleCommand("css_classmenu", "Opens a center HTML menu of classes")]
public void OnClassMenu(CCSPlayerController? player, CommandInfo command)
{
    if (player == null) return;

    var menu = new CenterHtmlMenu("Choose a class", this);
    menu.AddMenuOption("Rifler", (p, option) => p.PrintToChat($"You picked {option.Text}"));
    menu.AddMenuOption("AWPer", (p, option) => p.PrintToChat($"You picked {option.Text}"));
    menu.AddMenuOption("Support", (p, option) => p.PrintToChat($"You picked {option.Text}"));

    MenuManager.OpenCenterHtmlMenu(this, player, menu);
}
```
<sup><a href='/examples/WithMenus/WithMenusPlugin.cs#L33-L46' title='Snippet source file'>snippet source</a> | <a href='#snippet-menus-center-html' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
