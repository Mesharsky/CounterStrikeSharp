using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Cvars.Validators;

namespace WithFakeConvars;

[MinimumApiVersion(175)]
public class WithFakeConvarsPlugin : BasePlugin
{
    public override string ModuleName => "Example: With Fake Convars";
    public override string ModuleVersion => "1.0.0";

    // begin-snippet: fakeconvars-declare
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
    // end-snippet

    public FakeConVar<int> RequiresRestartCvar = new("example_requires_restart", "Restarts on change");

    public override void Load(bool hotReload)
    {
        // begin-snippet: fakeconvars-value-changed
        RequiresRestartCvar.ValueChanged += (sender, value) =>
        {
            if (value > 5)
            {
                Server.ExecuteCommand("mp_restartgame 1");
            }
        };
        // end-snippet

        RegisterFakeConVars(typeof(ConVars));
    }
}
