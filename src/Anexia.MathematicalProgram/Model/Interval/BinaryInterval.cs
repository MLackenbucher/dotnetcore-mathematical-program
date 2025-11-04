// ------------------------------------------------------------------------------------------
//  <copyright file = "BinaryInterval.cs" company = "ANEXIA® Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA® Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Anexia.MathematicalProgram.Model.Scalar;
using Anexia.MathematicalProgram.Model.Variable;

namespace Anexia.MathematicalProgram.Model.Interval;

/// <summary>
/// Represents a binary interval.
/// </summary>
public sealed class BinaryInterval : MemberwiseEquatable<BinaryInterval>, IInterval<BinaryScalar>
{
    /// <summary>
    /// The lower bound.
    /// </summary>
    public BinaryScalar LowerBound => false;

    /// <summary>
    /// The upper bound.
    /// </summary>
    public BinaryScalar UpperBound => true;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{nameof(LowerBound)}: {LowerBound}, {nameof(UpperBound)}: {UpperBound}";
}