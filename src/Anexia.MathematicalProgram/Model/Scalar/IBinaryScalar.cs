// ------------------------------------------------------------------------------------------
//  <copyright file = "IIntegerScalar.cs" company = "ANEXIA® Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA® Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------

namespace Anexia.MathematicalProgram.Model.Scalar;

/// <summary>
/// Represents an integer scalar.
/// </summary>
public interface IBinaryScalar : IAddableScalar<IBinaryScalar, IBinaryScalar>
{
    /// <summary>
    /// The value.
    /// </summary>
    public new bool Value { get; }
}