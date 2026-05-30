---
title: User messages
description: Hook and send user messages.
sidebar:
  order: 12
---

How to intercept incoming user messages and how to construct and send one.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithUserMessages)

## Hook a user message

<!-- snippet: usermessages-hook -->
<a id='snippet-usermessages-hook'></a>
```cs
// Hook by user message id. 452 is CMsgTEFireBullets.
HookUserMessage(452, um =>
{
    // Force every weapon to sound like a silenced usp.
    um.SetUInt("weapon_id", 0);
    um.SetInt("sound_type", 9);
    um.SetUInt("item_def_index", 61);
    return HookResult.Continue;
}, HookMode.Pre);

// 118 is the chat message id.
HookUserMessage(118, um =>
{
    var author = um.ReadString("param1");
    var message = um.ReadString("param2");
    Logger.LogInformation("Chat from {Author}: {Message}", author, message);

    // Returning Stop drops the message for every recipient.
    if (message.Contains("stop")) return HookResult.Stop;

    // You can also trim recipients to selectively hide the message.
    if (message.Contains("skip")) um.Recipients.Clear();
    return HookResult.Continue;
});
```
<sup><a href='/examples/WithUserMessages/WithUserMessagesPlugin.cs#L18-L43' title='Snippet source file'>snippet source</a> | <a href='#snippet-usermessages-hook' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Send a user message

<!-- snippet: usermessages-send -->
<a id='snippet-usermessages-send'></a>
```cs
[ConsoleCommand("css_shake", "Shakes the screen for the caller or all players")]
public void OnCommandShake(CCSPlayerController? player, CommandInfo command)
{
    if (player == null) return;

    // Resolve the message by a partial network name. "Shake" matches CUserMessageShake.
    var message = UserMessage.FromPartialName("Shake");
    message.SetFloat("duration", 2);
    message.SetFloat("amplitude", 5);
    message.SetFloat("frequency", 10f);
    message.SetInt("command", 0);

    if (command.GetArg(1) == "all")
    {
        message.Recipients.AddAllPlayers();
    }
    else
    {
        message.Recipients.Add(player);
    }

    message.Send();
}
```
<sup><a href='/examples/WithUserMessages/WithUserMessagesPlugin.cs#L46-L70' title='Snippet source file'>snippet source</a> | <a href='#snippet-usermessages-send' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
