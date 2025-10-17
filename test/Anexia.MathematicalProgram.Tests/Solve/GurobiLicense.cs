// ------------------------------------------------------------------------------------------
//  <copyright file = "GurobiLicense.cs" company = "ANEXIA Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------

using Gurobi;

namespace Anexia.MathematicalProgram.Tests.Solve;

/// <summary>
/// Detects (once per test run) whether a usable Gurobi licence is available in the current environment.
/// </summary>
internal static class GurobiLicense
{
    internal static bool IsAvailable { get; }

    internal static bool IsMissing { get; }

    static GurobiLicense()
    {
        try
        {
            // The parameterless constructor creates and starts the environment immediately,
            // which fails when no licence (or no native library) is available.
            var env = new GRBEnv();
            env.Dispose();
            IsAvailable = true;
        }
        catch (Exception exception)
        {
            // Only a missing licence triggers the solver's fallback path; other failures
            // (e.g. an expired licence) are rethrown and cannot exercise that behaviour.
            IsMissing = exception.Message.Contains("No Gurobi license found");
        }
    }
}

/// <summary>
/// A <see cref="FactAttribute" /> that is skipped unless a Gurobi licence is available.
/// </summary>
public sealed class RequiresGurobiLicenceFactAttribute : FactAttribute
{
    public RequiresGurobiLicenceFactAttribute()
    {
        if (!GurobiLicense.IsAvailable)
            Skip = "Requires a Gurobi licence; skipped because none was detected in this environment.";
    }
}

/// <summary>
/// A <see cref="FactAttribute" /> that is skipped when a Gurobi licence is available. Use for tests that
/// assert the no-licence fallback behaviour, which cannot hold on a licensed machine.
/// </summary>
public sealed class RequiresNoGurobiLicenceFactAttribute : FactAttribute
{
    public RequiresNoGurobiLicenceFactAttribute()
    {
        if (!GurobiLicense.IsMissing)
            Skip = "Asserts no-licence fallback behaviour; skipped because the environment does not " +
                   "report a missing Gurobi licence.";
    }
}
