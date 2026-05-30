---
title: Fake convars
description: Declare typed plugin owned console variables with validators.
sidebar:
  order: 8
---

`FakeConVar<T>` lets your plugin own a console variable with a type, default
value, flags, and an optional validator. Subscribe to `ValueChanged` to react
to runtime changes.

[View on GitHub](https://github.com/roflmuffin/CounterStrikeSharp/tree/main/examples/WithFakeConvars)

## Declare

<!-- snippet: fakeconvars-declare -->
<a id='snippet-fakeconvars-declare'></a>
```cs
// A simple boolean. Default value is true.
public FakeConVar<bool> BoolCvar = new("example_bool", "An example boolean cvar", true);

// A bounded integer using the built in range validator.
public FakeConVar<int> ExampleIntCvar = new(
    "example_int", "An example integer cvar", 10,
    flags: ConVarFlags.FCVAR_NONE, new RangeValidator<int>(0, 100));

// Cheat flag means it can only be changed while sv_cheats is on.
public FakeConVar<float> ExampleCheatCvar = new(
    "example_cheat_float", "An example cheat float cvar", 5, ConVarFlags.FCVAR_CHEAT);

// Protected cvars do not echo their value when queried.
public FakeConVar<float> ExampleProtectedCvar = new(
    "example_protected_float", "An example protected float cvar", 5, ConVarFlags.FCVAR_PROTECTED);

// Custom validators implement IValidator<T> in your own code.
public FakeConVar<int> ExampleEvenNumberCvar = new(
    "example_even_number", "Only even numbers", 0,
    flags: ConVarFlags.FCVAR_NONE, new EvenNumberValidator());
```
<sup><a href='/examples/WithFakeConvars/WithFakeConvarsPlugin.cs#L15-L36' title='Snippet source file'>snippet source</a> | <a href='#snippet-fakeconvars-declare' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## React to a change

<!-- snippet: fakeconvars-value-changed -->
<a id='snippet-fakeconvars-value-changed'></a>
```cs
RequiresRestartCvar.ValueChanged += (sender, value) =>
{
    if (value > 5)
    {
        Server.ExecuteCommand("mp_restartgame 1");
    }
};
```
<sup><a href='/examples/WithFakeConvars/WithFakeConvarsPlugin.cs#L42-L50' title='Snippet source file'>snippet source</a> | <a href='#snippet-fakeconvars-value-changed' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
