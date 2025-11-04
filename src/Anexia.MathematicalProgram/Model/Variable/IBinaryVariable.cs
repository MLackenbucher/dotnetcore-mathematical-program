// ------------------------------------------------------------------------------------------
//  <copyright file = "IIntegerVariable.cs" company = "ANEXIA® Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA® Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------


using Anexia.MathematicalProgram.Model.Scalar;

namespace Anexia.MathematicalProgram.Model.Variable;

/// <summary>
/// Represents an binary variable.
/// </summary>
public interface IBinaryVariable<out TIntervalValue> : IVariable<TIntervalValue>
    where TIntervalValue : IAddableScalar<TIntervalValue, TIntervalValue>
{
}