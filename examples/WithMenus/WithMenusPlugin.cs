using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace WithMenus;

[MinimumApiVersion(80)]
public class WithMenusPlugin : BasePlugin
{
    public override string ModuleName => "Example: With Menus";
    public override string ModuleVersion => "1.0.0";

    // begin-snippet: menus-chat-menu
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
    // end-snippet

    // begin-snippet: menus-center-html
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
    // end-snippet
}
