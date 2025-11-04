// ------------------------------------------------------------------------------------------
//  <copyright file = "IntegerVariable.cs" company = "ANEXIA® Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA® Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Anexia.MathematicalProgram.Model.Interval;
using Anexia.MathematicalProgram.Model.Scalar;

namespace Anexia.MathematicalProgram.Model.Variable;

/// <summary>
/// Represents a binary variable.
/// </summary>
public sealed class BinaryVariable : MemberwiseEquatable<BinaryVariable>,
    IBinaryVariable<BinaryScalar>, ICreatableVariable<BinaryVariable, IBinaryScalar>, ICreatableVariable<BinaryVariable, IRealScalar>, IIntegerVariable<IRealScalar>
{
    internal BinaryVariable(IInterval<BinaryScalar> interval, string name)
    {
        Name = name;
        Interval = interval;
    }

    public static BinaryVariable Create(IInterval<IBinaryScalar> interval, string name)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{Interval.LowerBound} <= {Name} <= {Interval.UpperBound}";

    public IInterval<BinaryScalar> Interval { get; }
    public string Name { get; }
}