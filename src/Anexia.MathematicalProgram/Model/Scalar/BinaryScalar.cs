// ------------------------------------------------------------------------------------------
//  <copyright file = "IntegerScalar.cs" company = "ANEXIA® Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA® Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------


using System.Diagnostics.CodeAnalysis;

namespace Anexia.MathematicalProgram.Model.Scalar;

/// <summary>
/// Represents an binary scalar.
/// </summary>
/// <param name="Value">false, if the binary value is 0, true otherwise</param>
public record BinaryScalar(bool Value) : IBinaryScalar,
    IAddableScalar<BinaryScalar, BinaryScalar>
{
    public IRealScalar Add(IRealScalar scalar) => new RealScalar((Convert.ToDouble(Value) + scalar.Value) % 2);


    public IRealScalar Subtract(IRealScalar scalar) => new RealScalar((Convert.ToDouble(Value) + scalar.Value) % 2);

    public IIntegerScalar Add(IIntegerScalar scalar) => new IntegerScalar((Convert.ToInt32(Value) + scalar.Value) % 2);

    public IIntegerScalar Subtract(IIntegerScalar scalar) =>
        new IntegerScalar((Convert.ToInt32(Value) + scalar.Value) % 2);


    public IBinaryScalar Add(IBinaryScalar scalar) => new BinaryScalar(Value ^ scalar.Value);

    public IBinaryScalar Subtract(IBinaryScalar scalar) => new BinaryScalar(Value ^ scalar.Value);

    public BinaryScalar Add(BinaryScalar scalar) => new(Value ^ scalar.Value);

    public BinaryScalar Subtract(BinaryScalar scalar) => new(Value ^ scalar.Value);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{(Value ? 1 : 0)}";

    public static implicit operator BinaryScalar(bool other) => new(other);

}