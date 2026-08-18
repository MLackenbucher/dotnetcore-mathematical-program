// ------------------------------------------------------------------------------------------
//  <copyright file = "SolutionValuesTest.cs" company = "ANEXIA Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using Anexia.MathematicalProgram.Model;
using Anexia.MathematicalProgram.Model.Interval;
using Anexia.MathematicalProgram.Model.Scalar;
using Anexia.MathematicalProgram.Model.Variable;
using Anexia.MathematicalProgram.Result;

namespace Anexia.MathematicalProgram.Tests.Result;

public sealed class SolutionValuesTest
{
    [Fact]
    public void EmptySolutionValuesIsEmpty()
    {
        var values = EmptySolutionValues();

        Assert.True(values.Empty);
    }

    [Fact]
    public void EmptySolutionValuesReturnsDefaultForMissingVariable()
    {
        var values = EmptySolutionValues();

        Assert.Null(values.GetSolutionValueOrDefault(CreateVariable()));
    }

    [Fact]
    public void EmptySolutionValuesTryGetReturnsFalseForMissingVariable()
    {
        var values = EmptySolutionValues();

        Assert.False(values.TryGetSolutionValue(CreateVariable(), out _));
    }

    [Fact]
    public void EmptySolutionValuesTryGetOutputsNullForMissingVariable()
    {
        var values = EmptySolutionValues();

        values.TryGetSolutionValue(CreateVariable(), out var value);

        Assert.Null(value);
    }

    [Fact]
    public void EmptySolutionValuesEnumeratesNoElements()
    {
        var values = EmptySolutionValues();

        Assert.Empty(values);
    }

    [Fact]
    public void StoredSolutionValuesAreNotEmpty()
    {
        var values = StoredSolutionValues(out _, out _);

        Assert.False(values.Empty);
    }

    [Fact]
    public void GetSolutionValueOrDefaultReturnsStoredValue()
    {
        var values = StoredSolutionValues(out var firstVariable, out _);

        Assert.Equal(new RealScalar(2.5), values.GetSolutionValueOrDefault(firstVariable));
    }

    [Fact]
    public void TryGetSolutionValueReturnsTrueForStoredVariable()
    {
        var values = StoredSolutionValues(out _, out var secondVariable);

        Assert.True(values.TryGetSolutionValue(secondVariable, out _));
    }

    [Fact]
    public void TryGetSolutionValueOutputsStoredValue()
    {
        var values = StoredSolutionValues(out _, out var secondVariable);

        values.TryGetSolutionValue(secondVariable, out var secondValue);

        Assert.Equal(new RealScalar(-3), secondValue);
    }

    [Fact]
    public void SolutionValuesEnumerateAllStoredValues()
    {
        var values = StoredSolutionValues(out var firstVariable, out var secondVariable);

        Assert.Equal(
            [
                KeyValuePair.Create(firstVariable, new RealScalar(2.5)),
                KeyValuePair.Create(secondVariable, new RealScalar(-3))
            ],
            values.OrderBy(pair => pair.Key.Name));
    }

    private static SolutionValues<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar> EmptySolutionValues() =>
        new(ReadOnlyDictionary<IIntegerVariable<IRealScalar>, RealScalar>.Empty);

    private static IIntegerVariable<IRealScalar> CreateVariable()
    {
        var model = new OptimizationModel<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>();

        return model.NewVariable<IntegerVariable<IRealScalar>>(new RealInterval(0, 1), "v1");
    }

    private static SolutionValues<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar> StoredSolutionValues(
        out IIntegerVariable<IRealScalar> firstVariable,
        out IIntegerVariable<IRealScalar> secondVariable)
    {
        var model = new OptimizationModel<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>();
        firstVariable = model.NewVariable<IntegerVariable<IRealScalar>>(new RealInterval(0, 10), "v1");
        secondVariable = model.NewVariable<IntegerVariable<IRealScalar>>(new RealInterval(-10, 10), "v2");

        return new SolutionValues<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            new ReadOnlyDictionary<IIntegerVariable<IRealScalar>, RealScalar>(
                new Dictionary<IIntegerVariable<IRealScalar>, RealScalar>
                {
                    [firstVariable] = new(2.5),
                    [secondVariable] = new(-3)
                }));
    }
}
