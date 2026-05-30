---
title: Menus
description: Build chat menus and center HTML menus for players.
---

CounterStrikeSharp ships two built in menu styles.

- **Chat menu**: a list of options printed to chat. The player picks with
  the chat keys `!1` through `!6`. Numbers `!7`, `!8`, `!9` are used for
  previous page, next page, and close.
- **Center HTML menu**: a panel that floats in the center of the screen. The
  same keys apply.

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

The center HTML menu needs a reference to your plugin so it can hook the
per tick redraw.

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

## Selection behaviour

`menu.PostSelectAction` controls what happens after an option is picked:

- `Close` closes the menu after a pick.
- `Reset` resets back to the first page after a pick.
- `Nothing` leaves the menu open.

`menu.ExitButton = false` hides the close option.

## Closing manually

```csharp
MenuManager.CloseActiveMenu(player);
```
