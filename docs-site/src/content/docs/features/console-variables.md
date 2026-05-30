---
title: Console variables
description: Read and write existing ConVars and declare your own with FakeConVar.
---

## Existing ConVars

Use `ConVar.Find` to get a reference to an existing console variable. It
returns `null` when the cvar does not exist.

```csharp
var cheats = ConVar.Find("sv_cheats");
```

### Primitive values

For `bool`, `int`, and `float` cvars, `GetPrimitiveValue<T>` returns a `ref`
to the value. Assigning to that ref updates the cvar in place.

```csharp
cheats.GetPrimitiveValue<bool>() = true;

// Or the helper method:
cheats.SetValue(true);
```

### String values

Strings are marshalled across the boundary, so use the `StringValue` property.

```csharp
var sky = ConVar.Find("sv_skyname");
Console.WriteLine($"sv_skyname = {sky.StringValue}");
sky.StringValue = "foobar";
```

### Native objects

For non primitive types (vectors, colours), use `GetNativeValue<T>`.

```csharp
var fog = ConVar.Find("fog_color");
var fogColor = fog.GetNativeValue<Vector>();
fogColor.X = 0.12345;
```

## Declaring your own with FakeConVar

`FakeConVar<T>` lets your plugin own a cvar. It supports validators, flags,
and a `ValueChanged` event.

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

Register every `FakeConVar` on a type in one call:

```csharp
public override void Load(bool hotReload)
{
    RegisterFakeConVars(typeof(ConVars));
}
```

### Reacting to changes

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
